using System;
using Proto;
using UGameTools;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using ExcelConfig;
using GameLogic.Game.Perceptions;
using GameLogic.Game.Elements;

/// <summary>
/// 游戏中的通知播放者
/// </summary>
public class NotifyPlayer
{
    public NotifyPlayer()
    {
        
    }
        
    private  Dictionary<int,IBattleElement> views = new Dictionary<int, IBattleElement>();

    #region Events
    public Action<IBattleCharacter> OnCreateUser;
    public Action<IBattleCharacter> OnDeath;
    public Action<Notify_PlayerJoinState> OnJoined;
    public Action<Notify_Drop> OnDrop;
    #endregion

    /// <summary>
    /// 处理网络包的解析
    /// </summary>
    /// <param name="notify">Notify.</param>
    public void Process(ISerializerable notify)
    {
        var per = UPerceptionView.S as IBattlePerception;
        if (notify is Notify_CreateBattleCharacter)
        {
            var createcharacter = notify as Notify_CreateBattleCharacter;
            var resources = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigByID<ExcelConfig.CharacterData>(createcharacter.ConfigID);
            var view = per.CreateBattleCharacterView(
                           resources.ResourcesPath,
                           createcharacter.Position.ToGVer3(), 
                           createcharacter.Forward.ToGVer3());
            var character = view as UCharacterView;
            character.index = createcharacter.Index;
            character.UserID = createcharacter.UserID;
            view.SetSpeed(createcharacter.Speed*1.1f);

            foreach (var i in createcharacter.Magics)
            {
                view.AttachMaigc(i.MagicID, i.CDTime);
            }
            views.Add(view.Index, view);
            if (OnCreateUser != null)
            {
                OnCreateUser(view);
            }
    
        }
        else if (notify is Notify_CreateReleaser)
        {
            var creater = notify as Notify_CreateReleaser;
            var releaer = views[creater.ReleaserIndex] as UCharacterView;
            var target = views[creater.TargetIndex] as UCharacterView;
            var viewer = per.CreateReleaserView(releaer, target, null);
            var releaser = viewer as UMagicReleaserView;
            releaser.SetCharacter(releaer, target);
            releaser.index = creater.Index;
            views.Add(viewer.Index, viewer);
        }
        else if (notify is Notify_CreateMissile)
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
            var view = per.CreateMissile(releaser, layout);
            var missile = view as UBattleMissileView;
            missile.index = create.Index;
            views.Add(view.Index, view);
        }
        else if (notify is Notify_LayoutPlayParticle)
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
            per.CreateParticlePlayer(releaser, layout);
        }
        else if (notify is Notify_LookAtCharacter)
        {
            var look = notify as Notify_LookAtCharacter;
            var owner = views[look.Own] as IBattleCharacter;
            var target = views[look.Target]as IBattleCharacter;
            owner.LookAtTarget(target);
        }
        else if (notify is Notify_CharacterPosition)
        {
            var position = notify as Notify_CharacterPosition;
            var view = views[position.Index] as IBattleCharacter;
            view.MoveTo(position.TargetPosition.ToGVer3());
        }
        else if (notify is Notify_LayoutPlayMotion)
        {
            var motion = notify as Notify_LayoutPlayMotion;
            var view = views[motion.Index] as IBattleCharacter;
            view.PlayMotion(motion.Motion);
        }
        else if (notify is Notify_HPChange)
        {
            var change = notify as Proto.Notify_HPChange;
            var view = views[change.Index] as IBattleCharacter;
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
        else if (notify is Notify_ElementExitState)
        {
            var exit = notify as Notify_ElementExitState;
            var view = views[exit.Index];
            views.Remove(exit.Index);

            //GameObject.Destroy(view.gameObject);
        }
        else if (notify is Notify_ElementJoinState)
        {
            var joinState = notify as Proto.Notify_ElementJoinState;
            var view = views[joinState.Index];
            //view.Joined();
        }
        else if (notify is Notify_DamageResult)
        {
            /*var damage = notify as Proto.Notify_DamageResult;
            var view = views[damage.Index];
            var character = view as UCharacterView;
            character.NotifyDamage(damage);*/
        }
        else if (notify is Notify_MPChange)
        {
            var mpChanged = notify as Notify_MPChange;
            var view = views[mpChanged.Index] as IBattleCharacter;
            view.ShowMPChange(mpChanged.MP, mpChanged.TargetMP, mpChanged.Max);
        }
        else if (notify is Notify_PropertyValue)
        {
            var pV = notify as Notify_PropertyValue;
            var view = views[pV.Index] as IBattleCharacter;
            view.ProtertyChange(pV.Type, pV.FinallyValue);
        }
        else if (notify is Notify_PlayerJoinState)
        {
            //package
            var package = notify as Notify_PlayerJoinState;
            if (this.OnJoined != null)
                OnJoined(package);
        }
        else if (notify is Notify_Drop)
        {
            var drop = notify as Notify_Drop;
            if (OnDrop != null)
            {
                OnDrop(drop);
            }

            //var drop = notify as Notify_Drop;
            if (drop.Gold > 0)
            {
                //gold += drop.Gold;
                UAppliaction.S.ShowNotify("Gold +" + drop.Gold);
            }

            foreach (var i in drop.Items)
            {
                var item = ExcelToJSONConfigManager.Current.GetConfigByID<ItemData>(i.ItemID);
                UAppliaction.S.ShowNotify(string.Format("{0}+{1}",item.Name,i.Num));
            }
        }
        else if (notify is Notify_ReleaseMagic)
        {
            var release = notify as Notify_ReleaseMagic;
            var view = views[release.Index] as IBattleCharacter;
            view.AttachMaigc(release.MagicID, release.CdCompletedTime);
        }
        else
        {
            Debug.LogError("NO Handle:" + notify.GetType());
        }
    }

}


