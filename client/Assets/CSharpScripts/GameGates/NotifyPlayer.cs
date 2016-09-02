using System;
using Proto;
using UGameTools;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using ExcelConfig;

public class NotifyPlayer
{
    public NotifyPlayer()
    {
        
    }

    public List<PlayerItem> Items{ set; get; }

    public int gold { set; get; }

    private  Dictionary<long,UElementView> views = new Dictionary<long, UElementView>();
  
    public Action<UCharacterView> OnCreateUser;
    public Action<UCharacterView> OnDeath;

    public void Process(Proto.ISerializerable notify)
    {
        if (notify is Proto.Notify_CreateBattleCharacter)
        {
            var createcharacter = notify as Notify_CreateBattleCharacter;
            var resources = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigByID<ExcelConfig.CharacterData>(createcharacter.ConfigID);
            var view = UPerceptionView.Singleton.CreateBattleCharacterView(
                           resources.ResourcesPath,
                           createcharacter.Position.ToGVer3(), 
                           createcharacter.Forward.ToGVer3()) as UCharacterView;
            view.Index = createcharacter.Index;
            view.UserID = createcharacter.UserID;
            views.Add(view.Index, view);

            if (OnCreateUser != null)
            {
                OnCreateUser(view);
            }
    
        }
        else if (notify is Proto.Notify_CreateReleaser)
        {
            var creater = notify as Notify_CreateReleaser;
            var releaer = views[creater.ReleaserIndex] as UCharacterView;
            var target = views[creater.TargetIndex] as UCharacterView;
            var viewer = UPerceptionView.Singleton.CreateReleaserView(releaer, target, null) as UMagicReleaserView;
            viewer.SetCharacter(releaer, target);
            viewer.Index = creater.Index;
            views.Add(viewer.Index, viewer);
        }
        else if (notify is Proto.Notify_CreateMissile)
        {
            var create = notify as Notify_CreateMissile;
            var releaser = views[create.ReleaserIndex] as UMagicReleaserView;
            var layout = new Layout.LayoutElements.MissileLayout
            {
                fromBone = create.formBone,
                toBone = create.toBone,
                offset = create.offset.ToLVer3(),
                resourcesPath = create.ResourcesPath,
                speed = create.Speed
            };
            var view = UPerceptionView.Singleton.CreateMissile(releaser, layout) as UBattleMissileView;
            view.Index = create.Index;
            views.Add(view.Index, view);
        }
        else if (notify is Proto.Notify_LayoutPlayParticle)
        {
            var particle = notify as Notify_LayoutPlayParticle;
            var layout = new Layout.LayoutElements.ParticleLayout
            {
                path = particle.Path,
                destoryTime = particle.DestoryTime,
                destoryType = (Layout.LayoutElements.ParticleDestoryType)particle.DestoryType,
                fromBoneName = particle.FromBoneName,
                fromTarget = (Layout.TargetType)particle.FromTarget,
                toBoneName = particle.ToBoneName,
                toTarget = (Layout.TargetType)particle.ToTarget,
                Bind = particle.Bind
            };
            var releaser = views[particle.ReleaseIndex] as UMagicReleaserView;
            UPerceptionView.Singleton.CreateParticlePlayer(releaser, layout);
        }
        else if (notify is Notify_LookAtCharacter)
        {
            var look = notify as Notify_LookAtCharacter;
            var owner = views[look.Own] as UCharacterView;
            var target = views[look.Target]as UCharacterView;
            owner.LookAt(target.Transform);
        }
        else if (notify is Proto.Notify_CharacterPosition)
        {
            var position = notify as Notify_CharacterPosition;
            var view = views[position.Index] as UCharacterView;
            var totalTime = (position.LastPosition.ToVer3() - position.TargetPosition.ToVer3()).magnitude / position.Speed;
            var speed = (view.transform.position - position.TargetPosition.ToVer3()).magnitude /
                        Mathf.Max(0.01f, totalTime);
            view.SetSpeed(speed);
            view.MoveTo(position.TargetPosition.ToGVer3());
        }
        else if (notify is Proto.Notify_LayoutPlayMotion)
        {
            var motion = notify as Notify_LayoutPlayMotion;
            var view = views[motion.Index] as UCharacterView;
            view.PlayMotion(motion.Motion);
        }
        else if (notify is Proto.Notify_HPChange)
        {
            var change = notify as Proto.Notify_HPChange;
            var view = views[change.Index] as UCharacterView;
            view.ShowHPChange(change.HP, change.TargetHP, change.Max);
            if (change.TargetHP == 0)
            {
                if (OnDeath != null)
                {
                    OnDeath(view);
                }
                view.Death();
            }
        }
        else if (notify is Proto.Notify_ElementExitState)
        {
            var exit = notify as Notify_ElementExitState;
            var view = views[exit.Index];
            views.Remove(exit.Index);
            GameObject.Destroy(view.gameObject);
        }
        else if (notify is Proto.Notify_ElementJoinState)
        {
           
        }
        else if (notify is Proto.Notify_DamageResult)
        {

            /*var damage = notify as Proto.Notify_DamageResult;
            var view = views[damage.Index];
            var character = view as UCharacterView;
            character.NotifyDamage(damage);*/
        }
        else if (notify is Proto.Notify_MPChange)
        {
            //do nothing
        }
        else if (notify is Proto.Notify_PropertyValue)
        {
            
        }
        else if (notify is Notify_PlayerJoinState)
        {
            //package
            var package = notify as Notify_PlayerJoinState;
            gold = package.Gold;
            Items = package.Package.Items;
        }
        else if (notify is Proto.Notify_Drop)
        {
            var drop = notify as Notify_Drop;
            if (drop.Gold > 0)
            {
                gold += drop.Gold;
                UUITipDrawer.Singleton.ShowNotify("Gold +" + drop.Gold);
            }

            foreach (var i in drop.Items)
            {
                var item = ExcelToJSONConfigManager.Current.GetConfigByID<ItemData>(i.ItemID);
                UUITipDrawer.Singleton.ShowNotify(string.Format("{0}+{1}",item.Name,i.Num));
                bool found = false;
                foreach(var t in Items)
                {
                    if(t.ItemID == i.ItemID){
                        t.Num += i.Num;
                        found = true;
                        break;
                    }
                }
                if (!found)
                    Items.Add(i);

            }
        }
        else
        {
            Debug.LogError("NO Handle:" + notify.GetType());
        }
    }

}


