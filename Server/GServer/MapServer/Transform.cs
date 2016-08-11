using System;
using EngineCore;
using GameLogic;

namespace MapServer
{
    public class Transform:ITransform
    {
        public Transform()
        {
            
        }



        public GVector3 Forward
        {
            get;
            set;
        }

        public GVector3 ForwardEulerAngles
        {
            get
            {
                return Forward;
            }
        }

        public GVector3 Position
        {
            get;
            set;
        }

        public void LookAt(ITransform trans)
        {
            var dr = trans.Position - this.Position;
            var t = dr.ToVector3().Normalized();
            this.Forward = new GVector3(t.X, t.Y, t.Z);// dr.y, dr.z);
        }
    }
}

