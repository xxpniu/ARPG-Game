
using Proto;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Proto.PServices;
namespace Proto.NotifyService
{

    /// <summary>
    /// 10001
    /// </summary>    
    [API(10001)]
    public class CharacterAlpha:APIBase<Notify_CharacterAlpha, Notify_CharacterAlpha> 
    {
        private CharacterAlpha() : base() { }
        public  static CharacterAlpha CreateQuery(){ return new CharacterAlpha();}
    }
    

    /// <summary>
    /// 10002
    /// </summary>    
    [API(10002)]
    public class CharacterPosition:APIBase<Notify_CharacterPosition, Notify_CharacterPosition> 
    {
        private CharacterPosition() : base() { }
        public  static CharacterPosition CreateQuery(){ return new CharacterPosition();}
    }
    

    /// <summary>
    /// 10003
    /// </summary>    
    [API(10003)]
    public class CreateBattleCharacter:APIBase<Notify_CreateBattleCharacter, Notify_CreateBattleCharacter> 
    {
        private CreateBattleCharacter() : base() { }
        public  static CreateBattleCharacter CreateQuery(){ return new CreateBattleCharacter();}
    }
    

    /// <summary>
    /// 10004
    /// </summary>    
    [API(10004)]
    public class CreateMissile:APIBase<Notify_CreateMissile, Notify_CreateMissile> 
    {
        private CreateMissile() : base() { }
        public  static CreateMissile CreateQuery(){ return new CreateMissile();}
    }
    

    /// <summary>
    /// 10005
    /// </summary>    
    [API(10005)]
    public class CreateReleaser:APIBase<Notify_CreateReleaser, Notify_CreateReleaser> 
    {
        private CreateReleaser() : base() { }
        public  static CreateReleaser CreateQuery(){ return new CreateReleaser();}
    }
    

    /// <summary>
    /// 10006
    /// </summary>    
    [API(10006)]
    public class DamageResult:APIBase<Notify_DamageResult, Notify_DamageResult> 
    {
        private DamageResult() : base() { }
        public  static DamageResult CreateQuery(){ return new DamageResult();}
    }
    

    /// <summary>
    /// 10007
    /// </summary>    
    [API(10007)]
    public class Drop:APIBase<Notify_Drop, Notify_Drop> 
    {
        private Drop() : base() { }
        public  static Drop CreateQuery(){ return new Drop();}
    }
    

    /// <summary>
    /// 10008
    /// </summary>    
    [API(10008)]
    public class ElementExitState:APIBase<Notify_ElementExitState, Notify_ElementExitState> 
    {
        private ElementExitState() : base() { }
        public  static ElementExitState CreateQuery(){ return new ElementExitState();}
    }
    

    /// <summary>
    /// 10009
    /// </summary>    
    [API(10009)]
    public class ElementJoinState:APIBase<Notify_ElementJoinState, Notify_ElementJoinState> 
    {
        private ElementJoinState() : base() { }
        public  static ElementJoinState CreateQuery(){ return new ElementJoinState();}
    }
    

    /// <summary>
    /// 10010
    /// </summary>    
    [API(10010)]
    public class HPChange:APIBase<Notify_HPChange, Notify_HPChange> 
    {
        private HPChange() : base() { }
        public  static HPChange CreateQuery(){ return new HPChange();}
    }
    

    /// <summary>
    /// 10011
    /// </summary>    
    [API(10011)]
    public class LayoutPlayMotion:APIBase<Notify_LayoutPlayMotion, Notify_LayoutPlayMotion> 
    {
        private LayoutPlayMotion() : base() { }
        public  static LayoutPlayMotion CreateQuery(){ return new LayoutPlayMotion();}
    }
    

    /// <summary>
    /// 10012
    /// </summary>    
    [API(10012)]
    public class LayoutPlayParticle:APIBase<Notify_LayoutPlayParticle, Notify_LayoutPlayParticle> 
    {
        private LayoutPlayParticle() : base() { }
        public  static LayoutPlayParticle CreateQuery(){ return new LayoutPlayParticle();}
    }
    

    /// <summary>
    /// 10013
    /// </summary>    
    [API(10013)]
    public class LookAtCharacter:APIBase<Notify_LookAtCharacter, Notify_LookAtCharacter> 
    {
        private LookAtCharacter() : base() { }
        public  static LookAtCharacter CreateQuery(){ return new LookAtCharacter();}
    }
    

    /// <summary>
    /// 10014
    /// </summary>    
    [API(10014)]
    public class MPChange:APIBase<Notify_MPChange, Notify_MPChange> 
    {
        private MPChange() : base() { }
        public  static MPChange CreateQuery(){ return new MPChange();}
    }
    

    /// <summary>
    /// 10015
    /// </summary>    
    [API(10015)]
    public class PlayerJoinState:APIBase<Notify_PlayerJoinState, Notify_PlayerJoinState> 
    {
        private PlayerJoinState() : base() { }
        public  static PlayerJoinState CreateQuery(){ return new PlayerJoinState();}
    }
    

    /// <summary>
    /// 10016
    /// </summary>    
    [API(10016)]
    public class PropertyValue:APIBase<Notify_PropertyValue, Notify_PropertyValue> 
    {
        private PropertyValue() : base() { }
        public  static PropertyValue CreateQuery(){ return new PropertyValue();}
    }
    

    /// <summary>
    /// 10017
    /// </summary>    
    [API(10017)]
    public class ReleaseMagic:APIBase<Notify_ReleaseMagic, Notify_ReleaseMagic> 
    {
        private ReleaseMagic() : base() { }
        public  static ReleaseMagic CreateQuery(){ return new ReleaseMagic();}
    }
    

    public interface INotifyService
    {
        [API(10017)]Notify_ReleaseMagic ReleaseMagic(Notify_ReleaseMagic req);
        [API(10016)]Notify_PropertyValue PropertyValue(Notify_PropertyValue req);
        [API(10015)]Notify_PlayerJoinState PlayerJoinState(Notify_PlayerJoinState req);
        [API(10014)]Notify_MPChange MPChange(Notify_MPChange req);
        [API(10013)]Notify_LookAtCharacter LookAtCharacter(Notify_LookAtCharacter req);
        [API(10012)]Notify_LayoutPlayParticle LayoutPlayParticle(Notify_LayoutPlayParticle req);
        [API(10011)]Notify_LayoutPlayMotion LayoutPlayMotion(Notify_LayoutPlayMotion req);
        [API(10010)]Notify_HPChange HPChange(Notify_HPChange req);
        [API(10009)]Notify_ElementJoinState ElementJoinState(Notify_ElementJoinState req);
        [API(10008)]Notify_ElementExitState ElementExitState(Notify_ElementExitState req);
        [API(10007)]Notify_Drop Drop(Notify_Drop req);
        [API(10006)]Notify_DamageResult DamageResult(Notify_DamageResult req);
        [API(10005)]Notify_CreateReleaser CreateReleaser(Notify_CreateReleaser req);
        [API(10004)]Notify_CreateMissile CreateMissile(Notify_CreateMissile req);
        [API(10003)]Notify_CreateBattleCharacter CreateBattleCharacter(Notify_CreateBattleCharacter req);
        [API(10002)]Notify_CharacterPosition CharacterPosition(Notify_CharacterPosition req);
        [API(10001)]Notify_CharacterAlpha CharacterAlpha(Notify_CharacterAlpha req);

    }
   

}