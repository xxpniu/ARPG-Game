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
using UMath;

namespace MapServer.GameViews
{
    public class BattleCharactorView : BattleElement,IBattleCharacter
    {
        public BattleCharactorView(UVector3 pos, 
                                   UVector3 forword,BattlePerceptionView view,
                                   Pathfinder pathfiner) : base(view)
        {
            transform = new  UTransform();
            transform.position = pos;
            transform.forward = forword;
            Finder = pathfiner;
            //lastPost = pos.ToVector3();
        }


        public Pathfinder Finder { private set; get; }

       
        private List<UVector3> CurrentPath;

        private float speed;

        private UTransform transform;
        private bool isMoving = false;

        private UVector3 GetNodeVer(Node n)
        {
            var v = Finder.ToWorldPos(n);
            return new UVector3(v.x, v.y, v.z);
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
                    LastPosition = this.transform.position.ToV3(),
                    TargetPosition = CurrentPath[nextWaypoint].ToV3(),
                    Index = this.Index
                };
                this.PerceptionView.AddNotify(notify);
            }

            isMoving = true;
            var fullPath = CurrentPath[nextWaypoint] - CurrentPath[lastWaypoint]; //defines the path between lastWaypoint and nextWaypoint as a Vector3
            faction_of_path_traveled += speed * time.DetalTime;//animate along the path
            if (faction_of_path_traveled > fullPath.magnitude) //move to next waypoint
            {
                lastWaypoint++; nextWaypoint++;
                faction_of_path_traveled = 0;
                UpdatePath(time);
                return;
            }
            //we COULD use Translate at this point, but it's simpler to just compute the current position
            var pos= (fullPath.normalized * faction_of_path_traveled) + CurrentPath[lastWaypoint];
            transform.position = pos;
        }

        public override ISerializerable GetInitNotify()
        {
            var battleCharacter = this.Element as BattleCharacter;
            var properties = new List<HeroProperty>();
            foreach (var i in Enum.GetValues(typeof(HeroPropertyType)))
            {
                var p = (HeroPropertyType)i;
                properties.Add(new HeroProperty { Property = p, Value = battleCharacter[p].FinalValue });
            }
            var createNotity = new Notify_CreateBattleCharacter
            {
                Index = battleCharacter.Index,
                UserID = battleCharacter.UserID,
                ConfigID = battleCharacter.ConfigID,
                Position = battleCharacter.View.Transform.position.ToV3(),
                Forward = battleCharacter.View.Transform.forward.ToV3(),
                HP = battleCharacter.HP,
                Properties = properties,
                Level = battleCharacter.Level,
                TDamage = battleCharacter.TDamage,
                TDefance = battleCharacter.TDefance,
                Name = battleCharacter.Name,
                Category = battleCharacter.Category,
                TeamIndex = battleCharacter.TeamIndex,
                Speed = battleCharacter.Speed
            };


            foreach (var i in battleCharacter.Magics)
            {
                var time = battleCharacter.GetCoolDwon(i.ID);
                createNotity.Magics.Add(new HeroMagicData { CDTime = time, MagicID = i.ID });
            }

            return createNotity;
        }


        #region IBattleCharacter
        UTransform IBattleCharacter.Transform
        {
            get
            {
                return transform;
            }
        }

        bool IBattleCharacter.IsMoving
        {
            get
            {
                return isMoving;
            }
        }



        void  IBattleCharacter. Death()
        {
            //Do nothing
        }

        void IBattleCharacter.LookAtTarget(IBattleCharacter target)
        {
            var notify = new Proto.Notify_LookAtCharacter
            {
                Own = Index,
                Target = target.Index
            };
            this.PerceptionView.AddNotify(notify);
            this.transform.LookAt(target.Transform);
        }

        void IBattleCharacter.MoveTo(UVector3 position)
        {
            //_findStarted = DateTime.Now;

            isMoving = true;
            nextWaypoint = 0;
            lastWaypoint = 0;
            finalWaypoint = 0;
            syncIndex = 0;
            CurrentPath = null;
            var pos = this.transform.position;
            var nodeStart = Finder.WorldPosToNode(pos.x, 0, pos.z);
            var nodeEnd = Finder.WorldPosToNode(position.x, 0, position.z);
            var path= Finder.FindPathActual(nodeStart, nodeEnd);
            if (path != null && path.Count > 0)
            {
                CurrentPath = path.Select(t => GetNodeVer(t)).ToList();
                if (CurrentPath.Count > 0)
                {
                    CurrentPath.RemoveAt(0);
                    CurrentPath.Insert(0, this.transform.position);
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

        void IBattleCharacter.SetForward(UVector3 forward)
        {
            var v = forward;
            v.Normalized();
            transform.forward = v;
        }

        void IBattleCharacter.SetPosition(UVector3 pos)
        {
            transform.position = pos;
        }

        void IBattleCharacter.SetPriorityMove(float priorityMove)
        {
            //donothing server don't support
        }

        void IBattleCharacter.SetScale(float scale)
        {
            //Send To Client?
        }

        void IBattleCharacter.SetSpeed(float _speed)
        {
            speed = _speed;
        }

        void IBattleCharacter.ShowHPChange(int hp, int cur, int max)
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

        void IBattleCharacter.StopMove()
        {
            CurrentPath = null;
            if (syncIndex > 0)
            {
                var notify = new Proto.Notify_CharacterPosition()
                {
                    LastPosition = this.transform.position.ToV3(),
                    TargetPosition = this.transform.position.ToV3(),
                    Index = this.Index
                };
                PerceptionView.AddNotify(notify);
            }
            syncIndex = 0;
        }


        void IBattleCharacter.ShowMPChange(int mp, int cur, int maxMP)
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

        void IBattleCharacter.ProtertyChange(HeroPropertyType type, int finalValue)
        {
            var notify = new Notify_PropertyValue { Index = Index, FinallyValue = finalValue, Type = type };
            PerceptionView.AddNotify(notify);
        }

        void IBattleCharacter.AttachMaigc(int magicID, float cdCompletedTime)
        {
            var notify = new Notify_ReleaseMagic { Index = Index, MagicID = magicID, CdCompletedTime = cdCompletedTime };
            PerceptionView.AddNotify(notify);
        }

        void IBattleCharacter.SetAlpha(float alpha)
        {
             
        }


        #endregion
    }
}

