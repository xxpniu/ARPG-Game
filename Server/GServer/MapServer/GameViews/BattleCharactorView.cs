using System;
using System.Collections.Generic;
using Astar;
using EngineCore;
using EngineCore.Simulater;
using GameLogic;
using GameLogic.Game.Elements;
using Layout;
using Vector3 = OpenTK.Vector3;
using System.Linq;

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
            lastPost = pos.ToVector3();
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

        public void Death()
        {
            //Do nothing
        }

        public void LookAt(ITransform target)
        {
            Transform.LookAt(target);
        }

        public List<GVector3> MoveTo(GVector3 position)
        {
            nextWaypoint = 0;
            lastWaypoint = 0;
            finalWaypoint = 0;
            var pos = this.Transform.Position;
            var nodeStart = Finder.WorldPosToNode(pos.x, 0, pos.z);
            var nodeEnd = Finder.WorldPosToNode(position.x, 0, position.z);
            var path = Finder.FindPathActual(nodeStart, nodeEnd);
            //path.Reverse();
            if (path != null)
            {
                CurrentPath = path.Select(t => GetNodeVer(t)).ToList();
                if (CurrentPath.Count > 0)
                {
                    CurrentPath.RemoveAt(0);
                    CurrentPath.Insert(0, this.transform.Position.ToVector3());
                }
                finalWaypoint = CurrentPath.Count - 1;
                nextWaypoint = 1;
                lastWaypoint = 0;
                faction_of_path_traveled = 0;
                return CurrentPath.Select(t => t.ToGVector3()).ToList();
            }
            else 
            {
                CurrentPath = null;
                this.SetPosition(position);
            }
            return new List<GVector3>();
        }

        private Vector3 GetNodeVer(Node n)
        {
            var v= Finder.ToWorldPos(n);
            return new Vector3(v.x, v.y, v.z);
        }

        private List<Vector3> CurrentPath;


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
            CurrentPath = null;
        }

        public override void Update(GTime time)
        {
            base.Update(time);
            if (lastNotifyPositionTime + ((float)(Appliaction.SERVER_TICK*5) /1000f)< time.Time)
            {
                if (posChanged)
                {
                    posChanged = false;
                    lastNotifyPositionTime = time.Time;
                    var notify = new Proto.Notify_CharacterPosition
                    {
                        LastPosition = lastPost.ToGVector3().ToV3(),
                        TargetPosition = this.transform.Position.ToV3(),
                        Speed = this.speed,
                        Index = this.Index,
                        StartForward = this.transform.Position.ToV3()
                    };
                    this.PerceptionView.AddNotify(notify);
                    lastPost = this.transform.Position.ToVector3();
                }
            }

            if (CurrentPath == null || CurrentPath.Count == 0) return;
            UpdatePath(time);
        }

        float faction_of_path_traveled;
        int lastWaypoint, nextWaypoint, finalWaypoint;

        private float lastNotifyPositionTime;
        private Vector3 lastPost;
        private bool posChanged = false;
        //...
        void UpdatePath(GTime time)
        {
            

            if (nextWaypoint > finalWaypoint)
            {
                CurrentPath = null;
                return;
            }

            if (nextWaypoint > finalWaypoint) return;
            Vector3 fullPath =CurrentPath[nextWaypoint]-CurrentPath[lastWaypoint]; //defines the path between lastWaypoint and nextWaypoint as a Vector3
            faction_of_path_traveled += speed * time.DetalTime;//animate along the path
            if (faction_of_path_traveled>fullPath.Length) //move to next waypoint
            {
                lastWaypoint++; nextWaypoint++;
                faction_of_path_traveled = 0;
                UpdatePath(time);
                return;
            }
            //we COULD use Translate at this point, but it's simpler to just compute the current position
            var pos = (fullPath.Normalized() * faction_of_path_traveled) + CurrentPath[lastWaypoint];
            this.SetPosition(pos.ToGVector3());
        
            posChanged = true;
        }

        public void ShowMPChange(int mp, int cur, int maxMP)
        {
            //throw new NotImplementedException();
        }
    }
}

