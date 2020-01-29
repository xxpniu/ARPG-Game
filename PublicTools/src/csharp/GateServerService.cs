
using Proto;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
namespace Proto.PServices.GateServerService
{

    /// <summary>
    /// 10005
    /// </summary>    
    [API(10005)]
    public class Login:APIBase<C2G_Login, G2C_Login> 
    {
        private Login() : base() { }
        public  static Login CreateQuery(){ return new Login();}
    }

    /// <summary>
    /// 10005
    /// </summary>    
    [API(10005)]
    public partial class LoginHandler:APIHandler<C2G_Login,G2C_Login>
    {
     
    }
    

    /// <summary>
    /// 10006
    /// </summary>    
    [API(10006)]
    public class CreateHero:APIBase<C2G_CreateHero, G2C_CreateHero> 
    {
        private CreateHero() : base() { }
        public  static CreateHero CreateQuery(){ return new CreateHero();}
    }

    /// <summary>
    /// 10006
    /// </summary>    
    [API(10006)]
    public partial class CreateHeroHandler:APIHandler<C2G_CreateHero,G2C_CreateHero>
    {
     
    }
    

    /// <summary>
    /// 10007
    /// </summary>    
    [API(10007)]
    public class BeginGame:APIBase<C2G_BeginGame, G2C_BeginGame> 
    {
        private BeginGame() : base() { }
        public  static BeginGame CreateQuery(){ return new BeginGame();}
    }

    /// <summary>
    /// 10007
    /// </summary>    
    [API(10007)]
    public partial class BeginGameHandler:APIHandler<C2G_BeginGame,G2C_BeginGame>
    {
     
    }
    

    /// <summary>
    /// 10008
    /// </summary>    
    [API(10008)]
    public class GetLastBattle:APIBase<C2G_GetLastBattle, G2C_GetLastBattle> 
    {
        private GetLastBattle() : base() { }
        public  static GetLastBattle CreateQuery(){ return new GetLastBattle();}
    }

    /// <summary>
    /// 10008
    /// </summary>    
    [API(10008)]
    public partial class GetLastBattleHandler:APIHandler<C2G_GetLastBattle,G2C_GetLastBattle>
    {
     
    }
    

    /// <summary>
    /// 10009
    /// </summary>    
    [API(10009)]
    public class OperatorEquip:APIBase<C2G_OperatorEquip, G2C_OperatorEquip> 
    {
        private OperatorEquip() : base() { }
        public  static OperatorEquip CreateQuery(){ return new OperatorEquip();}
    }

    /// <summary>
    /// 10009
    /// </summary>    
    [API(10009)]
    public partial class OperatorEquipHandler:APIHandler<C2G_OperatorEquip,G2C_OperatorEquip>
    {
     
    }
    

    /// <summary>
    /// 10010
    /// </summary>    
    [API(10010)]
    public class SaleItem:APIBase<C2G_SaleItem, G2C_SaleItem> 
    {
        private SaleItem() : base() { }
        public  static SaleItem CreateQuery(){ return new SaleItem();}
    }

    /// <summary>
    /// 10010
    /// </summary>    
    [API(10010)]
    public partial class SaleItemHandler:APIHandler<C2G_SaleItem,G2C_SaleItem>
    {
     
    }
    

    /// <summary>
    /// 10011
    /// </summary>    
    [API(10011)]
    public class EquipmentLevelUp:APIBase<C2G_EquipmentLevelUp, G2C_EquipmentLevelUp> 
    {
        private EquipmentLevelUp() : base() { }
        public  static EquipmentLevelUp CreateQuery(){ return new EquipmentLevelUp();}
    }

    /// <summary>
    /// 10011
    /// </summary>    
    [API(10011)]
    public partial class EquipmentLevelUpHandler:APIHandler<C2G_EquipmentLevelUp,G2C_EquipmentLevelUp>
    {
     
    }
    

    /// <summary>
    /// 10012
    /// </summary>    
    [API(10012)]
    public class GMTool:APIBase<C2G_GMTool, G2C_GMTool> 
    {
        private GMTool() : base() { }
        public  static GMTool CreateQuery(){ return new GMTool();}
    }

    /// <summary>
    /// 10012
    /// </summary>    
    [API(10012)]
    public partial class GMToolHandler:APIHandler<C2G_GMTool,G2C_GMTool>
    {
     
    }
    

}