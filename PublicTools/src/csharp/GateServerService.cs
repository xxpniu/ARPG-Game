
using Proto;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Proto.PServices;
namespace Proto.GateServerService
{

    /// <summary>
    /// 10016
    /// </summary>    
    [API(10016)]
    public class Login:APIBase<C2G_Login, G2C_Login> 
    {
        private Login() : base() { }
        public  static Login CreateQuery(){ return new Login();}
    }
    

    /// <summary>
    /// 10017
    /// </summary>    
    [API(10017)]
    public class CreateHero:APIBase<C2G_CreateHero, G2C_CreateHero> 
    {
        private CreateHero() : base() { }
        public  static CreateHero CreateQuery(){ return new CreateHero();}
    }
    

    /// <summary>
    /// 10018
    /// </summary>    
    [API(10018)]
    public class BeginGame:APIBase<C2G_BeginGame, G2C_BeginGame> 
    {
        private BeginGame() : base() { }
        public  static BeginGame CreateQuery(){ return new BeginGame();}
    }
    

    /// <summary>
    /// 10019
    /// </summary>    
    [API(10019)]
    public class GetLastBattle:APIBase<C2G_GetLastBattle, G2C_GetLastBattle> 
    {
        private GetLastBattle() : base() { }
        public  static GetLastBattle CreateQuery(){ return new GetLastBattle();}
    }
    

    /// <summary>
    /// 10020
    /// </summary>    
    [API(10020)]
    public class OperatorEquip:APIBase<C2G_OperatorEquip, G2C_OperatorEquip> 
    {
        private OperatorEquip() : base() { }
        public  static OperatorEquip CreateQuery(){ return new OperatorEquip();}
    }
    

    /// <summary>
    /// 10021
    /// </summary>    
    [API(10021)]
    public class SaleItem:APIBase<C2G_SaleItem, G2C_SaleItem> 
    {
        private SaleItem() : base() { }
        public  static SaleItem CreateQuery(){ return new SaleItem();}
    }
    

    /// <summary>
    /// 10022
    /// </summary>    
    [API(10022)]
    public class EquipmentLevelUp:APIBase<C2G_EquipmentLevelUp, G2C_EquipmentLevelUp> 
    {
        private EquipmentLevelUp() : base() { }
        public  static EquipmentLevelUp CreateQuery(){ return new EquipmentLevelUp();}
    }
    

    /// <summary>
    /// 10023
    /// </summary>    
    [API(10023)]
    public class GMTool:APIBase<C2G_GMTool, G2C_GMTool> 
    {
        private GMTool() : base() { }
        public  static GMTool CreateQuery(){ return new GMTool();}
    }
    

    public interface IGateServerService
    {
        [API(10023)]G2C_GMTool GMTool(C2G_GMTool req);
        [API(10022)]G2C_EquipmentLevelUp EquipmentLevelUp(C2G_EquipmentLevelUp req);
        [API(10021)]G2C_SaleItem SaleItem(C2G_SaleItem req);
        [API(10020)]G2C_OperatorEquip OperatorEquip(C2G_OperatorEquip req);
        [API(10019)]G2C_GetLastBattle GetLastBattle(C2G_GetLastBattle req);
        [API(10018)]G2C_BeginGame BeginGame(C2G_BeginGame req);
        [API(10017)]G2C_CreateHero CreateHero(C2G_CreateHero req);
        [API(10016)]G2C_Login Login(C2G_Login req);

    }
   

}