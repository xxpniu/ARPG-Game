using System;
using System.Collections.Generic;
using System.Reflection;
using EngineCore.Simulater;
using GameLogic;
using GameLogic.Game.Elements;
using Google.Protobuf;
using Layout.LayoutElements;
using Proto;

namespace MapServer.GameViews
{

    public class NotifyElementCreateAttribute : Attribute
    {
        public NotifyElementCreateAttribute(Type elementType)
        {
            ElementType = elementType;
        }

        public Type ElementType { set; get; }
    }

    public sealed class NotifyProcessor
    {
        public NotifyProcessor()
        {
            //Perception = perception;
            _methodes = new Dictionary<Type, MethodInfo>();
            var ms = typeof(NotifyProcessor).GetMethods();
            foreach (var i in ms)
            {
                var attrs = i.GetCustomAttributes(typeof(NotifyElementCreateAttribute), false) as NotifyElementCreateAttribute[];
                if (attrs.Length == 0) continue;
                _methodes.Add(attrs[0].ElementType, i);
            }
        }

        private readonly Dictionary<Type, MethodInfo> _methodes;

        [NotifyElementCreate(typeof(BattleCharacter))]
        public IMessage CreateBattleCharacter(BattleCharacter battleCharacter)
        {
            
            var createNotity = new Proto.Notify_CreateBattleCharacter
            {
                Index = battleCharacter.Index,
                AccountUuid = battleCharacter.AcccountUuid,
                ConfigID = battleCharacter.ConfigID,
                Position = battleCharacter.View.Transform.position.ToV3(),
                Forward = battleCharacter.View.Transform.forward.ToV3(),
                HP = battleCharacter.HP,
                Level = battleCharacter.Level,
                TDamage = battleCharacter.TDamage,
                TDefance = battleCharacter.TDefance,
                Name = battleCharacter.Name,
                Category = battleCharacter.Category,
                TeamIndex = battleCharacter.TeamIndex,
                Speed = battleCharacter.Speed
            };


            foreach (var i in Enum.GetValues(typeof(HeroPropertyType)))
            {
                var p = (HeroPropertyType)i;
                createNotity. Properties.Add(new HeroProperty { Property = p, Value = battleCharacter[p].FinalValue });
            }


            foreach (var i in battleCharacter.Magics)
            {
                var time = battleCharacter.GetCoolDwon(i.ID);
                createNotity.Magics.Add(new HeroMagicData { CDTime = time, MagicID = i.ID });
            }

            return createNotity;
        }

        [NotifyElementCreate(typeof(BattleMissile))]
        public IMessage CreateMissile(BattleMissile missile)
        {
            var createNotify = new Proto.Notify_CreateMissile
            {
                Index = missile.Index,
                Position = missile.View.Transform.position.ToV3(),
                ResourcesPath = missile.Layout.resourcesPath,
                Speed = missile.Layout.speed,
                ReleaserIndex = missile.Releaser.Index,
                FormBone = missile.Layout.fromBone,
                ToBone = missile.Layout.toBone,
                Offset = missile.Layout.offset.ToV3()
            };
            return (createNotify);
        }

        [NotifyElementCreate(typeof(MagicReleaser))]
        public IMessage CreateReleaser(MagicReleaser mReleaser)
        {
            var createNotify = new Proto.Notify_CreateReleaser
            {
                Index = mReleaser.Index,
                ReleaserIndex = mReleaser.ReleaserTarget.Releaser.Index,
                TargetIndex = mReleaser.ReleaserTarget.ReleaserTarget.Index,
                MagicKey = mReleaser.Magic.key
            };
            return (createNotify);
        }

        public IMessage NotityElementCreate(GObject el)
        {
            if (_methodes.TryGetValue(el.GetType(), out MethodInfo m))
            {
                return m.Invoke(this, new object[] { el }) as IMessage;
            }
            else
            {
                throw new Exception("Can't Create :" + el.GetType());
            }
        }
    }
}

