using System;
namespace GameLogic.Game.Elements
{
    public class ReleaseHistory
    {
        public int MagicDataID;
        public float LastTime;
        public float CdTime;

        public bool IsCoolDown(float time)
        {
            return time > LastTime + CdTime;
        }

        public float TimeToCd(float time)
        {
            return Math.Max(0, (LastTime + CdTime) - time);
        }
    }
}

