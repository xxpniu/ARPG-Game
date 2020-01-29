
using Proto;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
namespace Proto.PServices.LoginGateServerService
{

    /// <summary>
    /// 10019
    /// </summary>    
    [API(10019)]
    public class RegServer:APIBase<G2L_Reg, L2G_Reg> 
    {
        private RegServer() : base() { }
        public  static RegServer CreateQuery(){ return new RegServer();}
    }

    /// <summary>
    /// 10019
    /// </summary>    
    [API(10019)]
    public partial class RegServerHandler:APIHandler<G2L_Reg,L2G_Reg>
    {
     
    }
    

    /// <summary>
    /// 10020
    /// </summary>    
    [API(10020)]
    public class CheckUserSession:APIBase<G2L_CheckUserSession, L2G_CheckUserSession> 
    {
        private CheckUserSession() : base() { }
        public  static CheckUserSession CreateQuery(){ return new CheckUserSession();}
    }

    /// <summary>
    /// 10020
    /// </summary>    
    [API(10020)]
    public partial class CheckUserSessionHandler:APIHandler<G2L_CheckUserSession,L2G_CheckUserSession>
    {
     
    }
    

    /// <summary>
    /// 10021
    /// </summary>    
    [API(10021)]
    public class BeginBattle:APIBase<G2L_BeginBattle, L2G_BeginBattle> 
    {
        private BeginBattle() : base() { }
        public  static BeginBattle CreateQuery(){ return new BeginBattle();}
    }

    /// <summary>
    /// 10021
    /// </summary>    
    [API(10021)]
    public partial class BeginBattleHandler:APIHandler<G2L_BeginBattle,L2G_BeginBattle>
    {
     
    }
    

    /// <summary>
    /// 10022
    /// </summary>    
    [API(10022)]
    public class GetLastBattle:APIBase<G2L_GetLastBattle, L2G_GetLastBattle> 
    {
        private GetLastBattle() : base() { }
        public  static GetLastBattle CreateQuery(){ return new GetLastBattle();}
    }

    /// <summary>
    /// 10022
    /// </summary>    
    [API(10022)]
    public partial class GetLastBattleHandler:APIHandler<G2L_GetLastBattle,L2G_GetLastBattle>
    {
     
    }
    

}