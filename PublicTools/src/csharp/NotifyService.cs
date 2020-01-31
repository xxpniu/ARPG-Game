
using Proto;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Proto.PServices;
namespace Proto.NotifyService
{

    /// <summary>
    /// 10028
    /// </summary>    
    [API(10028)]
    public class CharacterAlpha:APIBase<Void, Notify_CharacterAlpha> 
    {
        private CharacterAlpha() : base() { }
        public  static CharacterAlpha CreateQuery(){ return new CharacterAlpha();}
    }
    

    /// <summary>
    /// 10029
    /// </summary>    
    [API(10029)]
    public class CharacterPosition:APIBase<Void, Notify_CharacterPosition> 
    {
        private CharacterPosition() : base() { }
        public  static CharacterPosition CreateQuery(){ return new CharacterPosition();}
    }
    

    /// <summary>
    /// 10030
    /// </summary>    
    [API(10030)]
    public class CreateBattleCharacter:APIBase<Void, Notify_CreateBattleCharacter> 
    {
        private CreateBattleCharacter() : base() { }
        public  static CreateBattleCharacter CreateQuery(){ return new CreateBattleCharacter();}
    }
    

    /// <summary>
    /// 10031
    /// </summary>    
    [API(10031)]
    public class CreateMissile:APIBase<Void, Notify_CreateMissile> 
    {
        private CreateMissile() : base() { }
        public  static CreateMissile CreateQuery(){ return new CreateMissile();}
    }
    

    /// <summary>
    /// 10032
    /// </summary>    
    [API(10032)]
    public class CreateReleaser:APIBase<Void, Notify_CreateReleaser> 
    {
        private CreateReleaser() : base() { }
        public  static CreateReleaser CreateQuery(){ return new CreateReleaser();}
    }
    

    /// <summary>
    /// 10033
    /// </summary>    
    [API(10033)]
    public class DamageResult:APIBase<Void, Notify_DamageResult> 
    {
        private DamageResult() : base() { }
        public  static DamageResult CreateQuery(){ return new DamageResult();}
    }
    

    /// <summary>
    /// 10034
    /// </summary>    
    [API(10034)]
    public class Drop:APIBase<Void, Notify_Drop> 
    {
        private Drop() : base() { }
        public  static Drop CreateQuery(){ return new Drop();}
    }
    

    /// <summary>
    /// 10035
    /// </summary>    
    [API(10035)]
    public class ElementExitState:APIBase<Void, Notify_ElementExitState> 
    {
        private ElementExitState() : base() { }
        public  static ElementExitState CreateQuery(){ return new ElementExitState();}
    }
    

    /// <summary>
    /// 10036
    /// </summary>    
    [API(10036)]
    public class ElementJoinState:APIBase<Void, Notify_ElementJoinState> 
    {
        private ElementJoinState() : base() { }
        public  static ElementJoinState CreateQuery(){ return new ElementJoinState();}
    }
    

    /// <summary>
    /// 10037
    /// </summary>    
    [API(10037)]
    public class HPChange:APIBase<Void, Notify_HPChange> 
    {
        private HPChange() : base() { }
        public  static HPChange CreateQuery(){ return new HPChange();}
    }
    

    /// <summary>
    /// 10038
    /// </summary>    
    [API(10038)]
    public class LayoutPlayMotion:APIBase<Void, Notify_LayoutPlayMotion> 
    {
        private LayoutPlayMotion() : base() { }
        public  static LayoutPlayMotion CreateQuery(){ return new LayoutPlayMotion();}
    }
    

    /// <summary>
    /// 10039
    /// </summary>    
    [API(10039)]
    public class LayoutPlayParticle:APIBase<Void, Notify_LayoutPlayParticle> 
    {
        private LayoutPlayParticle() : base() { }
        public  static LayoutPlayParticle CreateQuery(){ return new LayoutPlayParticle();}
    }
    

    /// <summary>
    /// 10040
    /// </summary>    
    [API(10040)]
    public class LookAtCharacter:APIBase<Void, Notify_LookAtCharacter> 
    {
        private LookAtCharacter() : base() { }
        public  static LookAtCharacter CreateQuery(){ return new LookAtCharacter();}
    }
    

    /// <summary>
    /// 10041
    /// </summary>    
    [API(10041)]
    public class MPChange:APIBase<Void, Notify_MPChange> 
    {
        private MPChange() : base() { }
        public  static MPChange CreateQuery(){ return new MPChange();}
    }
    

    /// <summary>
    /// 10042
    /// </summary>    
    [API(10042)]
    public class PlayerJoinState:APIBase<Void, Notify_PlayerJoinState> 
    {
        private PlayerJoinState() : base() { }
        public  static PlayerJoinState CreateQuery(){ return new PlayerJoinState();}
    }
    

    /// <summary>
    /// 10043
    /// </summary>    
    [API(10043)]
    public class PropertyValue:APIBase<Void, Notify_PropertyValue> 
    {
        private PropertyValue() : base() { }
        public  static PropertyValue CreateQuery(){ return new PropertyValue();}
    }
    

    /// <summary>
    /// 10044
    /// </summary>    
    [API(10044)]
    public class ReleaseMagic:APIBase<Void, Notify_ReleaseMagic> 
    {
        private ReleaseMagic() : base() { }
        public  static ReleaseMagic CreateQuery(){ return new ReleaseMagic();}
    }
    

    public interface INotifyService
    {
        [API(10044)]Notify_ReleaseMagic ReleaseMagic(Void req);
        [API(10043)]Notify_PropertyValue PropertyValue(Void req);
        [API(10042)]Notify_PlayerJoinState PlayerJoinState(Void req);
        [API(10041)]Notify_MPChange MPChange(Void req);
        [API(10040)]Notify_LookAtCharacter LookAtCharacter(Void req);
        [API(10039)]Notify_LayoutPlayParticle LayoutPlayParticle(Void req);
        [API(10038)]Notify_LayoutPlayMotion LayoutPlayMotion(Void req);
        [API(10037)]Notify_HPChange HPChange(Void req);
        [API(10036)]Notify_ElementJoinState ElementJoinState(Void req);
        [API(10035)]Notify_ElementExitState ElementExitState(Void req);
        [API(10034)]Notify_Drop Drop(Void req);
        [API(10033)]Notify_DamageResult DamageResult(Void req);
        [API(10032)]Notify_CreateReleaser CreateReleaser(Void req);
        [API(10031)]Notify_CreateMissile CreateMissile(Void req);
        [API(10030)]Notify_CreateBattleCharacter CreateBattleCharacter(Void req);
        [API(10029)]Notify_CharacterPosition CharacterPosition(Void req);
        [API(10028)]Notify_CharacterAlpha CharacterAlpha(Void req);

    }
   

}