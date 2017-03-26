using System;
using System.Collections.Concurrent;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace ServerUtility
{
    public interface IUpdateThread
    {
        bool Update();
        void Exit();
        void Begin();
    }

    public enum WorkState
    {
        NONE,
        WORKING,
        Exiting,
        FINISHED
    }

    /// <summary>
    /// 处理线程管理
    /// </summary>
    public class WorkThread<T> where T : IUpdateThread
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sleepPerTick"></param>
        /// <param name="maxUpdaterPerThread"></param>
        public WorkThread(int sleepPerTick, int maxUpdaterPerThread)
        {
            SleepTime = sleepPerTick;
            MaxUpdaterPerThread = maxUpdaterPerThread;
            State = WorkState.NONE;
        }

        public int SleepTime { private set; get; }
        public int MaxUpdaterPerThread { private set; get; }
        public int Count { private set; get; }
        public Action<T> OnExitHandler;
        public WorkState State { private set; get; }
        public bool AddThread(T thread)
        {
            if (State != WorkState.WORKING) return false;
            this._addQueue.Enqueue(thread);
            Count++;
            return true;
        }
        private ConcurrentQueue<T> _addQueue = new ConcurrentQueue<T>();
        private Queue<T> _delQueue = new Queue<T>();
        private List<T> _worker = new List<T>();

        private void Update()
        {
            while (State == WorkState.WORKING)
            {
                while (_addQueue.Count > 0)
                {

                    T t;
                    if (_addQueue.TryDequeue(out t))
                    { 
                        _worker.Add(t);
                        t.Begin();
                    }
                }

                for (var i = 0; i < _worker.Count; i++)
                {
                    var w = _worker[i];
                    if (w.Update())
                    { 
                        _delQueue.Enqueue(w);
                    }
                }

                while (_delQueue.Count > 0)
                {
                    var d = _delQueue.Dequeue();
                    ExitThread(d);
                    _worker.Remove(d);
                }
            }
            switch (State)
            { 
                case WorkState.Exiting:
                    {
                        foreach (var i in _worker)
                        { 
                            ExitThread(i);
                        }
                        _worker.Clear();
                        _delQueue.Clear();
                        State = WorkState.FINISHED;
                    }
                    break;
                case  WorkState.FINISHED:
                    break;
            }
        }

        private void ExitThread(T update)
        {
            Count--;
            update.Exit();
            if (OnExitHandler != null)
                OnExitHandler(update);
        }

        public void Start()
        {
            if (State != WorkState.NONE)
            {
                throw new Exception("This thread has started. Now it is:" + State);
            }
            State = WorkState.WORKING;
            thread = new Thread(Update);
            thread.IsBackground = true;
            thread.Start();
        }

        public void Stop()
        {
            State = WorkState.Exiting;
            thread.Join(10000);
        }

        private Thread thread;
    }


}
