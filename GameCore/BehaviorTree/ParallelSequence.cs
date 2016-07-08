using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorTree
{
    /// <summary>
    /// 同时启动所有的子节点
    /// 并且一直运行直到任何一个子节点返回 failure 跳出
    /// 如果所有的节点返回success返回success
    /// </summary>
    public class ParallelSequence : GroupComposite
    {
        public ParallelSequence(params Composite[] children)
            : base(children)
        {
        }

        public ParallelSequence(ContextChangeHandler contextChange, params Composite[] children)
            : this(children)
        {
            ContextChanger = contextChange;
        }

        public override IEnumerable<RunStatus> Execute(object context)
        {
            if (ContextChanger != null)
            {
                context = ContextChanger(context);
            }

            foreach (var i in Children)
            {
                i.Start(context);
            }
            var status = RunStatus.Running;
            //如果没有为failure的返回
            while (status == RunStatus.Running)
            {
                //默认执行完成
                status = RunStatus.Success;
                foreach (var i in Children)
                {
                    //如果已经执行完跳过
                    if (i.LastStatus.HasValue && i.LastStatus.Value != RunStatus.Running)
                    {
                        continue;
                    }
                    //运行结束
                    if (i.Tick(context) != RunStatus.Running)
                    {
                        i.Stop(context);
                        if (i.LastStatus.Value == RunStatus.Failure)
                        {
                            status = RunStatus.Failure;
                            break;
                        }
                    }
                    else
                    {
                        status = RunStatus.Running;
                    }
                }
                yield return status;
                if (status != RunStatus.Running)
                {
                    foreach (var i in Children)
                    {
                        if (i.LastStatus.HasValue && i.LastStatus == RunStatus.Running)
                        {
                            i.Stop(context);
                        }
                    }
                    yield break;
                }
            }

            yield return status;
        }
    }
}