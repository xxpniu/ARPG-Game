
using Proto;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Proto.PServices;
namespace Proto.LoginBattleGameServerService
{

    /// <summary>
    /// 10027
    /// </summary>    
    [API(10027)]
    public class RegBattleServer:APIBase<B2L_RegBattleServer, L2B_RegBattleServer> 
    {
        private RegBattleServer() : base() { }
        public  static RegBattleServer CreateQuery(){ return new RegBattleServer();}
    }
    

    /// <summary>
    /// 10028
    /// </summary>    
    [API(10028)]
    public class EndBattle:APIBase<B2L_EndBattle, L2B_EndBattle> 
    {
        private EndBattle() : base() { }
        public  static EndBattle CreateQuery(){ return new EndBattle();}
    }
    

    /// <summary>
    /// 10029
    /// </summary>    
    [API(10029)]
    public class CheckSession:APIBase<B2L_CheckSession, L2B_CheckSession> 
    {
        private CheckSession() : base() { }
        public  static CheckSession CreateQuery(){ return new CheckSession();}
    }
    

    /// <summary>
    /// 10030
    /// </summary>    
    [API(10030)]
    public class RegServer:APIBase<G2L_Reg, L2G_Reg> 
    {
        private RegServer() : base() { }
        public  static RegServer CreateQuery(){ return new RegServer();}
    }
    

    /// <summary>
    /// 10031
    /// </summary>    
    [API(10031)]
    public class CheckUserSession:APIBase<G2L_CheckUserSession, L2G_CheckUserSession> 
    {
        private CheckUserSession() : base() { }
        public  static CheckUserSession CreateQuery(){ return new CheckUserSession();}
    }
    

    /// <summary>
    /// 10032
    /// </summary>    
    [API(10032)]
    public class BeginBattle:APIBase<G2L_BeginBattle, L2G_BeginBattle> 
    {
        private BeginBattle() : base() { }
        public  static BeginBattle CreateQuery(){ return new BeginBattle();}
    }
    

    /// <summary>
    /// 10033
    /// </summary>    
    [API(10033)]
    public class GetLastBattle:APIBase<G2L_GetLastBattle, L2G_GetLastBattle> 
    {
        private GetLastBattle() : base() { }
        public  static GetLastBattle CreateQuery(){ return new GetLastBattle();}
    }
    

    public interface ILoginBattleGameServerService
    {
        [API(10033)]L2G_GetLastBattle GetLastBattle(G2L_GetLastBattle req);
        [API(10032)]L2G_BeginBattle BeginBattle(G2L_BeginBattle req);
        [API(10031)]L2G_CheckUserSession CheckUserSession(G2L_CheckUserSession req);
        [API(10030)]L2G_Reg RegServer(G2L_Reg req);
        [API(10029)]L2B_CheckSession CheckSession(B2L_CheckSession req);
        [API(10028)]L2B_EndBattle EndBattle(B2L_EndBattle req);
        [API(10027)]L2B_RegBattleServer RegBattleServer(B2L_RegBattleServer req);

    }
   

}