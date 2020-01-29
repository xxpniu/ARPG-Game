
using Proto;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
namespace Proto.PServices.NotifyService
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
    /// 10028
    /// </summary>    
    [API(10028)]
    public partial class CharacterAlphaHandler:APIHandler<Void,Notify_CharacterAlpha>
    {
     
    }
    

    /// <summary>
    /// 10029
    /// </summary>    
    [API(10029)]
    public class CharacterPosition:APIBase<Void , Notify_CharacterPosition> 
    {
        private CharacterPosition() : base() { }
        public  static CharacterPosition CreateQuery(){ return new CharacterPosition();}
    }

    /// <summary>
    /// 10029
    /// </summary>    
    [API(10029)]
    public partial class CharacterPositionHandler:APIHandler<Void ,Notify_CharacterPosition>
    {
     
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
    /// 10030
    /// </summary>    
    [API(10030)]
    public partial class CreateBattleCharacterHandler:APIHandler<Void,Notify_CreateBattleCharacter>
    {
     
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
    /// 10031
    /// </summary>    
    [API(10031)]
    public partial class CreateMissileHandler:APIHandler<Void,Notify_CreateMissile>
    {
     
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
    /// 10032
    /// </summary>    
    [API(10032)]
    public partial class CreateReleaserHandler:APIHandler<Void,Notify_CreateReleaser>
    {
     
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
    /// 10033
    /// </summary>    
    [API(10033)]
    public partial class DamageResultHandler:APIHandler<Void,Notify_DamageResult>
    {
     
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
    /// 10034
    /// </summary>    
    [API(10034)]
    public partial class DropHandler:APIHandler<Void,Notify_Drop>
    {
     
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
    /// 10035
    /// </summary>    
    [API(10035)]
    public partial class ElementExitStateHandler:APIHandler<Void,Notify_ElementExitState>
    {
     
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
    /// 10036
    /// </summary>    
    [API(10036)]
    public partial class ElementJoinStateHandler:APIHandler<Void,Notify_ElementJoinState>
    {
     
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
    /// 10037
    /// </summary>    
    [API(10037)]
    public partial class HPChangeHandler:APIHandler<Void,Notify_HPChange>
    {
     
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
    /// 10038
    /// </summary>    
    [API(10038)]
    public partial class LayoutPlayMotionHandler:APIHandler<Void,Notify_LayoutPlayMotion>
    {
     
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
    /// 10039
    /// </summary>    
    [API(10039)]
    public partial class LayoutPlayParticleHandler:APIHandler<Void,Notify_LayoutPlayParticle>
    {
     
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
    /// 10040
    /// </summary>    
    [API(10040)]
    public partial class LookAtCharacterHandler:APIHandler<Void,Notify_LookAtCharacter>
    {
     
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
    /// 10041
    /// </summary>    
    [API(10041)]
    public partial class MPChangeHandler:APIHandler<Void,Notify_MPChange>
    {
     
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
    /// 10042
    /// </summary>    
    [API(10042)]
    public partial class PlayerJoinStateHandler:APIHandler<Void,Notify_PlayerJoinState>
    {
     
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
    /// 10043
    /// </summary>    
    [API(10043)]
    public partial class PropertyValueHandler:APIHandler<Void,Notify_PropertyValue>
    {
     
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

    /// <summary>
    /// 10044
    /// </summary>    
    [API(10044)]
    public partial class ReleaseMagicHandler:APIHandler<Void,Notify_ReleaseMagic>
    {
     
    }
    

}