using System;
using System.Collections.Generic;
using Proto;

namespace GameLogic.Game
{
    public class StateChangedEventArgs : EventArgs
    {
        public StateChangedEventArgs() 
        { 
            //
        }
        public ActionLockType Type { set; get; }
        public bool IsLocked { set; get; }
    }

    public class ActionLock
    {
        private int value = 0xFFFFFFF;

        public int Value
        {
            get { return value; }
        }

        private Dictionary<ActionLockType, int> Locks { set; get; }

        public ActionLock()
        {
            Locks = new Dictionary<ActionLockType, int>();
            var values = Enum.GetValues(typeof(ActionLockType));
            foreach (var i in values)
            {
                Locks.Add((ActionLockType)i, 0);
            }
        }

        public bool IsLock(ActionLockType type)
        {
            return Locks[type] > 0;
        }

        public void Lock(ActionLockType type)
        {
            bool isLocked = IsLock(type);
            Locks[type]++;
            if (isLocked != IsLock(type)){
                if (OnStateOnchanged != null)
                {
                    var args = new StateChangedEventArgs { Type = type, IsLocked = IsLock(type) };
                    OnStateOnchanged(this, args);
                }
            }
        }

        public void Unlock(ActionLockType type)
        {
            bool isLocked = IsLock(type);
            Locks[type]--;
            if (isLocked != IsLock(type))
            {
                if (OnStateOnchanged != null)
                {
                    var args = new StateChangedEventArgs { Type = type, IsLocked = IsLock(type) };
                    OnStateOnchanged(this, args);
                }
            }
        }

        //发生变化
        public EventHandler<StateChangedEventArgs> OnStateOnchanged;
    }
}

