using System;
using Layout.LayoutElements;
using GameLogic.Game.Elements;
using System.Collections.Generic;
using System.Reflection;
using GameLogic.Game.Perceptions;
using EngineCore;

namespace GameLogic.Game.LayoutLogics
{

	public class HandleLayoutAttribute:Attribute
	{
		public HandleLayoutAttribute(Type handleType)
		{
			HandleType = handleType;
		}

		public Type HandleType{set;get;}
	}

	public class LayoutBaseLogic
	{
		static  LayoutBaseLogic ()
		{
			var type = typeof(LayoutBaseLogic);
			var methods=type.GetMethods (BindingFlags.Public | BindingFlags.Static);
			foreach (var i in methods) {
				
				var atts = i.GetCustomAttributes(typeof(HandleLayoutAttribute), false) as HandleLayoutAttribute[];
				if (atts == null || atts.Length ==0)
					continue;
				_handler.Add (atts[0].HandleType, i);
			}
		}

		private static Dictionary<Type,MethodInfo> _handler = new Dictionary<Type, MethodInfo> ();

		public static void EnableLayout(LayoutBase layout, TimeLinePlayer player)
		{
			MethodInfo m;
			if (_handler.TryGetValue (layout.GetType (), out m)) {
				m.Invoke (null, new object[]{ player, layout });
			} else {
				throw new Exception ("No Found handle Type :"+ layout.GetType());
			}
		}

		//MissileLayout
		[HandleLayout(typeof(MissileLayout))]
		public static void MissileActive(TimeLinePlayer linePlayer, LayoutBase layoutBase)
		{
			var layout = layoutBase as MissileLayout;
			var per = linePlayer.Releaser.Controllor.Perception as BattlePerception;
			var missile = per.CreateMissile (layout, linePlayer.Releaser);
			per.State.AddElement (missile);
			linePlayer.Releaser.AttachElement(missile);
		}

		[HandleLayout(typeof(MotionLayout))]
		public static void MotionActive(TimeLinePlayer linePlayer, LayoutBase layoutBase)
		{
			var layout = layoutBase as MotionLayout;
			var releaser = linePlayer.Releaser;
			if (layout.targetType == Layout.TargetType.Releaser) {
				releaser.ReleaserTarget.Releaser.View.PlayMotion (layout.motionName);
			} else if (layout.targetType == Layout.TargetType.Target) {
				if (releaser.ReleaserTarget.ReleaserTarget == null)
					return;
				releaser.ReleaserTarget.ReleaserTarget.View.PlayMotion (layout.motionName);
			}
			//target.View.PlayMotion (motion);
		}

		[HandleLayout(typeof(DamageLayout))]
		public static void DamageActive(TimeLinePlayer linePlayer, LayoutBase layoutBase)
		{
			var releaser = linePlayer.Releaser;
			var layout = layoutBase as DamageLayout;

			BattleCharacter orginTarget = null;
			if (layout.target == Layout.TargetType.Releaser) {
				orginTarget = releaser.ReleaserTarget.Releaser;
			} else if (layout.target == Layout.TargetType.Target) 
			{
				//release At Position?
				if(releaser.ReleaserTarget.ReleaserTarget ==null) return ;
				orginTarget = releaser.ReleaserTarget.ReleaserTarget;
			}
			if (orginTarget == null) {
				throw new Exception ("Do not have target of orgin. type:" + layout.target.ToString ());
			}
			var offsetPos = new GVector3 (layout.offsetPosition.x, 
				layout.offsetPosition.y, layout.offsetPosition.z);
			var per = releaser.Controllor.Perception  as BattlePerception;
			var targets = per.FindTarget (orginTarget,
				layout.fiterType, 
				layout.damageType, 
				layout.radius,
				layout.angle, 
				layout.offsetAngle,
				offsetPos);
			
			if (string.IsNullOrEmpty (layout.effectKey))
			{
				return;
				//throw new Exception ("No Found effect key!");
			}

			//完成一次目标判定
			if (targets.Count > 0) 
			{
				if (layout.effectType == EffectType.EffectGroup) {
					var group = linePlayer.TypeEvent.FindGroupByKey (layout.effectKey);
					if (group == null)
						return;
					//相应效果处理
					foreach (var i in group.effects) {
						foreach (var t in targets)
							EffectBaseLogic.EffectActive (t, i, releaser);
					}
				}
			}

		}

		[HandleLayout(typeof(ParticleLayout))]
		public static void ParticleActive(TimeLinePlayer linePlayer,LayoutBase layoutBase)
		{
			var layout = layoutBase as ParticleLayout;
			var per = linePlayer.Releaser.Controllor.Perception as BattlePerception;

			BattleCharacter from =null, to =null;

			if (layout.fromTarget == Layout.TargetType.Releaser) {
				from = linePlayer.Releaser.ReleaserTarget.Releaser;
			} else if (layout.fromTarget == Layout.TargetType.Target) {
				from = linePlayer.Releaser.ReleaserTarget.ReleaserTarget;
			}

			if (layout.toTarget == Layout.TargetType.Releaser) {
				to = linePlayer.Releaser.ReleaserTarget.Releaser;
			} else if (layout.toTarget == Layout.TargetType.Target) {
				to = linePlayer.Releaser.ReleaserTarget.ReleaserTarget;
			}



			var particle = per.View.CreateParticlePlayer (from.View, layout.fromBoneName, to.View, layout.toBoneName);

			switch (layout.destoryType) 
			{
			case  ParticleDestoryType.LayoutTimeOut:
				linePlayer.AttachParticle (particle);
				break;
			case ParticleDestoryType.Time:
				particle.AutoDestory (layout.destoryTime);
				break;
			case ParticleDestoryType.Normal:
				//自动销亡
				break;
			}
		}

	}
}

