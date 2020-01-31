using System;
using EngineCore;
using GameLogic;
using UMath;

namespace MapServer
{
    public class Transform:ITransform
    {
        public Transform()
        {
            
        }



        public UVector3 Forward
        {
            get;
            set;
        }

        public UVector3 ForwardEulerAngles
        {
            get
            {
                return Forward;
            }
        }

        public UVector3 Position
        {
            get;
            set;
        }


        public void LookAt(ITransform trans)
        {
            var dr = trans.Position - this.Position;
            if (dr.sqrMagnitude > 0)
            {
                this.Forward = dr.normalized;// dr.y, dr.z);
            }
        }
    }
}

