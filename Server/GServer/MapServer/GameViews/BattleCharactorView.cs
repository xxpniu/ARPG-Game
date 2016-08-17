using System;
using EngineCore;
using EngineCore.Simulater;
using GameLogic;
using GameLogic.Game.Elements;
using Layout;
using Vector3 = OpenTK.Vector3;

namespace MapServer.GameViews
{
    public class BattleCharactorView : BattleElement,IBattleCharacter
    {
        public BattleCharactorView(GVector3 pos, GVector3 forword,BattlePerceptionView view) : base(view)
        {
            transform = new Transform();
            transform.Position = pos;
            transform.Forward = forword;
        }

        private float speed;
        private Vector3? moveTarget;

        private Transform transform;
        public ITransform Transform
        {
            get
            {
                return transform;
            }
        }

        public void Death()
        {
            //Do nothing
        }

        public void LookAt(ITransform target)
        {
            Transform.LookAt(target);
        }

        public void MoveTo(GVector3 position)
        {
            moveTarget = position.ToVector3();
        }

        public void PlayMotion(string motion)
        {
            //donothing
        }

        public void SetForward(GVector3 eulerAngles)
        {
            var v = eulerAngles.ToVector3();
            v.Normalize();
            transform.Forward = new GVector3(v.X, v.Y, v.Z);
        }

        public void SetPosition(GVector3 pos)
        {
            transform.Position = pos;
        }

        public void SetPriorityMove(float priorityMove)
        {
            //donothing server don't support
        }

        public void SetScale(float scale)
        {
            //server don't support
        }

        public void SetSpeed(float _speed)
        {
            speed = _speed;
        }



        public void ShowHPChange(int hp, int cur, int max)
        {
            //do nothing
        }

        public void StopMove()
        {
            moveTarget = null;
        }

        public override void Update(GTime time)
        {
            base.Update(time);

            if (moveTarget.HasValue)
            {
                var dis = (moveTarget.Value - this.transform.Position.ToVector3());
                if (dis.Length > 0.5f)
                {
                    var pos = transform.Position.ToVector3();
                    var offset = dis.Normalized() * speed * time.DetalTime;
                    transform.Position = (pos + offset).ToGVector3();
                }
                else {
                    transform.Position = moveTarget.Value.ToGVector3();
                    moveTarget = null;
                }
            }
        }

        public void ShowMPChange(int mp, int cur, int maxMP)
        {
            //throw new NotImplementedException();
        }
    }
}

