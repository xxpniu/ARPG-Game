
using Proto;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Proto.PServices;
using System.Threading.Tasks;
namespace Proto.GateServerService
{

    /// <summary>
    /// 10035
    /// </summary>    
    [API(10035)]
    public class Login:APIBase<C2G_Login, G2C_Login> 
    {
        private Login() : base() { }
        public  static Login CreateQuery(){ return new Login();}
    }
    

    /// <summary>
    /// 10036
    /// </summary>    
    [API(10036)]
    public class CreateHero:APIBase<C2G_CreateHero, G2C_CreateHero> 
    {
        private CreateHero() : base() { }
        public  static CreateHero CreateQuery(){ return new CreateHero();}
    }
    

    /// <summary>
    /// 10037
    /// </summary>    
    [API(10037)]
    public class BeginGame:APIBase<C2G_BeginGame, G2C_BeginGame> 
    {
        private BeginGame() : base() { }
        public  static BeginGame CreateQuery(){ return new BeginGame();}
    }
    

    /// <summary>
    /// 10038
    /// </summary>    
    [API(10038)]
    public class GetLastBattle:APIBase<C2G_GetLastBattle, G2C_GetLastBattle> 
    {
        private GetLastBattle() : base() { }
        public  static GetLastBattle CreateQuery(){ return new GetLastBattle();}
    }
    

    /// <summary>
    /// 10039
    /// </summary>    
    [API(10039)]
    public class OperatorEquip:APIBase<C2G_OperatorEquip, G2C_OperatorEquip> 
    {
        private OperatorEquip() : base() { }
        public  static OperatorEquip CreateQuery(){ return new OperatorEquip();}
    }
    

    /// <summary>
    /// 10040
    /// </summary>    
    [API(10040)]
    public class SaleItem:APIBase<C2G_SaleItem, G2C_SaleItem> 
    {
        private SaleItem() : base() { }
        public  static SaleItem CreateQuery(){ return new SaleItem();}
    }
    

    /// <summary>
    /// 10041
    /// </summary>    
    [API(10041)]
    public class EquipmentLevelUp:APIBase<C2G_EquipmentLevelUp, G2C_EquipmentLevelUp> 
    {
        private EquipmentLevelUp() : base() { }
        public  static EquipmentLevelUp CreateQuery(){ return new EquipmentLevelUp();}
    }
    

    /// <summary>
    /// 10042
    /// </summary>    
    [API(10042)]
    public class GMTool:APIBase<C2G_GMTool, G2C_GMTool> 
    {
        private GMTool() : base() { }
        public  static GMTool CreateQuery(){ return new GMTool();}
    }
    

    public interface IGateServerService
    {
        [API(10042)]G2C_GMTool GMTool(C2G_GMTool req);
        [API(10041)]G2C_EquipmentLevelUp EquipmentLevelUp(C2G_EquipmentLevelUp req);
        [API(10040)]G2C_SaleItem SaleItem(C2G_SaleItem req);
        [API(10039)]G2C_OperatorEquip OperatorEquip(C2G_OperatorEquip req);
        [API(10038)]G2C_GetLastBattle GetLastBattle(C2G_GetLastBattle req);
        [API(10037)]G2C_BeginGame BeginGame(C2G_BeginGame req);
        [API(10036)]G2C_CreateHero CreateHero(C2G_CreateHero req);
        [API(10035)]G2C_Login Login(C2G_Login req);

    }
   

    public abstract class GateServerService
    {
        [API(10042)]public abstract Task<G2C_GMTool> GMTool(C2G_GMTool request);
        [API(10041)]public abstract Task<G2C_EquipmentLevelUp> EquipmentLevelUp(C2G_EquipmentLevelUp request);
        [API(10040)]public abstract Task<G2C_SaleItem> SaleItem(C2G_SaleItem request);
        [API(10039)]public abstract Task<G2C_OperatorEquip> OperatorEquip(C2G_OperatorEquip request);
        [API(10038)]public abstract Task<G2C_GetLastBattle> GetLastBattle(C2G_GetLastBattle request);
        [API(10037)]public abstract Task<G2C_BeginGame> BeginGame(C2G_BeginGame request);
        [API(10036)]public abstract Task<G2C_CreateHero> CreateHero(C2G_CreateHero request);
        [API(10035)]public abstract Task<G2C_Login> Login(C2G_Login request);

    }

}