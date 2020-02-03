
using Proto;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Proto.PServices;
using System.Threading.Tasks;
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
    public class RegGateServer:APIBase<G2L_GateServerReg, L2G_GateServerReg> 
    {
        private RegGateServer() : base() { }
        public  static RegGateServer CreateQuery(){ return new RegGateServer();}
    }
    

    /// <summary>
    /// 10031
    /// </summary>    
    [API(10031)]
    public class GateServerSession:APIBase<G2L_GateCheckSession, L2G_GateCheckSession> 
    {
        private GateServerSession() : base() { }
        public  static GateServerSession CreateQuery(){ return new GateServerSession();}
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
        [API(10031)]L2G_GateCheckSession GateServerSession(G2L_GateCheckSession req);
        [API(10030)]L2G_GateServerReg RegGateServer(G2L_GateServerReg req);
        [API(10029)]L2B_CheckSession CheckSession(B2L_CheckSession req);
        [API(10028)]L2B_EndBattle EndBattle(B2L_EndBattle req);
        [API(10027)]L2B_RegBattleServer RegBattleServer(B2L_RegBattleServer req);

    }
   

    public abstract class LoginBattleGameServerService
    {
        [API(10033)]public abstract Task<L2G_GetLastBattle> GetLastBattle(G2L_GetLastBattle request);
        [API(10032)]public abstract Task<L2G_BeginBattle> BeginBattle(G2L_BeginBattle request);
        [API(10031)]public abstract Task<L2G_GateCheckSession> GateServerSession(G2L_GateCheckSession request);
        [API(10030)]public abstract Task<L2G_GateServerReg> RegGateServer(G2L_GateServerReg request);
        [API(10029)]public abstract Task<L2B_CheckSession> CheckSession(B2L_CheckSession request);
        [API(10028)]public abstract Task<L2B_EndBattle> EndBattle(B2L_EndBattle request);
        [API(10027)]public abstract Task<L2B_RegBattleServer> RegBattleServer(B2L_RegBattleServer request);

    }

}