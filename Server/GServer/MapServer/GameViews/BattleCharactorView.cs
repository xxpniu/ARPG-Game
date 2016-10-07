using System;
using System.Collections.Generic;
using Astar;
using EngineCore;
using EngineCore.Simulater;
using GameLogic;
using GameLogic.Game.Elements;
using Layout;
using System.Linq;
using Proto;
using XNet.Libs.Utility;

namespace MapServer.GameViews
{
    public class BattleCharactorView : BattleElement,IBattleCharacter
    {
        public BattleCharactorView(GVector3 pos, 
                                   GVector3 forword,BattlePerceptionView view,
                                   Pathfinder pathfiner) : base(view)
        {
            transform = new Transform();
            transform.Position = pos;
            transform.Forward = forword;
            Finder = pathfiner;
            //lastPost = pos.ToVector3();
        }


        public Astar.Pathfinder Finder { private set; get; }

        private float speed;

        private Transform transform;
        public ITransform Transform
        {
            get
            {
                return transform;
            }
        }

        public bool IsMoving
        {
            get
            {
                return isMoving;
            }
        }

        private bool isMoving = false;

        public void Death()
        {
            //Do nothing
        }

        public void LookAt(ITransform target)
        {
            Transform.LookAt(target);
          
        }

        public void LookAtTarget(IBattleCharacter target)
        {
            var notify = new Proto.Notify_LookAtCharacter
            {
                Own = Index,
                Target = target.Index
            };
            this.PerceptionView.AddNotify(notify);
            LookAt(target.Transform);
        }

        public void MoveTo(GVector3 position)
        {
            //_findStarted = DateTime.Now;

            isMoving = true;
            nextWaypoint = 0;
            lastWaypoint = 0;
            finalWaypoint = 0;
            syncIndex = 0;
            CurrentPath = null;
            var pos = this.Transform.Position;
            var nodeStart = Finder.WorldPosToNode(pos.x, 0, pos.z);
            var nodeEnd = Finder.WorldPosToNode(position.x, 0, position.z);
            var path= Finder.FindPathActual(nodeStart, nodeEnd);
            if (path != null && path.Count > 0)
            {
                CurrentPath = path.Select(t => GetNodeVer(t)).ToList();
                if (CurrentPath.Count > 0)
                {
                    CurrentPath.RemoveAt(0);
                    CurrentPath.Insert(0, this.transform.Position);
                }
                finalWaypoint = CurrentPath.Count - 1;
                nextWaypoint = 1;
                lastWaypoint = 0;
                faction_of_path_traveled = 0;
                isMoving = true;
            }
            else
            {
                CurrentPath = null;
                isMoving = false;
                //this.SetPosition(position);
            }
        }

       

        private GVector3 GetNodeVer(Node n)
        {
            var v= Finder.ToWorldPos(n);
            return new GVector3(v.x, v.y, v.z);
        }

        private List<GVector3> CurrentPath;


        public void PlayMotion(string motion)
        {
            var notify = new Proto.Notify_LayoutPlayMotion
            {
                Index = Index,
                Motion = motion
            };
            this.PerceptionView.AddNotify(notify);
            //donothing
        }

        public void SetForward(GVector3 eulerAngles)
        {
            var v = eulerAngles;
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
            //Send To Client?
        }

        public void SetSpeed(float _speed)
        {
            speed = _speed;
        }

        public void ShowHPChange(int hp, int cur, int max)
        {
            var notify = new Proto.Notify_HPChange
            {
                Index = Index,
                HP = hp,
                TargetHP = cur,
                Max = max
            };
            PerceptionView.AddNotify(notify);
        }

        public void StopMove()
        {
            CurrentPath = null;
            if (syncIndex > 0)
            {
                var notify = new Proto.Notify_CharacterPosition()
                {
                    LastPosition = this.Transform.Position.ToV3(),
                    TargetPosition = this.transform.Position.ToV3(),
                    Index = this.Index
                };
                PerceptionView.AddNotify(notify);
            }
            syncIndex = 0;
        }

        private int syncIndex = 0;

        public override void Update(GTime time)
        {
            base.Update(time);

            if (CurrentPath == null || CurrentPath.Count == 0) 
                return;
            
            UpdatePath(time);
        }

        float faction_of_path_traveled;
        int lastWaypoint, nextWaypoint, finalWaypoint;

        //...
        void UpdatePath(GTime time)
        {
            isMoving = false;
            if (nextWaypoint > finalWaypoint)
            {
                CurrentPath = null;
                return;
            }
            if (nextWaypoint > finalWaypoint) return;

            if (this.syncIndex != nextWaypoint)
            {
                this.syncIndex = nextWaypoint;
                var notify = new Proto.Notify_CharacterPosition
                {
                    LastPosition = this.transform.Position.ToV3(),
                    TargetPosition = CurrentPath[nextWaypoint].ToV3(),
                    Index = this.Index
                };
                this.PerceptionView.AddNotify(notify);
            }

            isMoving = true;
            var fullPath = CurrentPath[nextWaypoint] - CurrentPath[lastWaypoint]; //defines the path between lastWaypoint and nextWaypoint as a Vector3
            faction_of_path_traveled += speed * time.DetalTime;//animate along the path
            if (faction_of_path_traveled > fullPath.Length) //move to next waypoint
            {
                lastWaypoint++; nextWaypoint++;
                faction_of_path_traveled = 0;
                UpdatePath(time);
                return;
            }
            //we COULD use Translate at this point, but it's simpler to just compute the current position
            var pos = (fullPath.Normalized() * faction_of_path_traveled) + CurrentPath[lastWaypoint];
            this.SetPosition(pos);
        }

        public void ShowMPChange(int mp, int cur, int maxMP)
        {
            var notify = new Proto.Notify_MPChange
            {
                Index = Index,
                MP = mp,
                TargetMP = cur,
                Max = maxMP
            };
            PerceptionView.AddNotify(notify);
        }

        public void ProtertyChange(HeroPropertyType type, int finalValue)
        {
            var notify = new Notify_PropertyValue { Index = Index, FinallyValue = finalValue, Type = type };
            PerceptionView.AddNotify(notify);
        }

        public void AttachMaigc(int magicID, float cdCompletedTime)
        {
            var notify = new Notify_ReleaseMagic
            {
                Index = Index,
                MagicID = magicID,
                CdCompletedTime = cdCompletedTime
            };
            PerceptionView.AddNotify(notify);
        }

        public void SetAlpha(float alpha)
        {
            var notify = new Notify_CharacterAlpha { Index = Index, Alpha =alpha };
            PerceptionView.AddNotify(notify);
        }
    }
}

