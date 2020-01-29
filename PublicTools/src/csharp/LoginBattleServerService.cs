
using Proto;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
namespace Proto.PServices.LoginBattleServerService
{

    /// <summary>
    /// 10023
    /// </summary>    
    [API(10023)]
    public class StartBattle:APIBase<Void, Task_L2B_StartBattle> 
    {
        private StartBattle() : base() { }
        public  static StartBattle CreateQuery(){ return new StartBattle();}
    }

    /// <summary>
    /// 10023
    /// </summary>    
    [API(10023)]
    public partial class StartBattleHandler:APIHandler<Void,Task_L2B_StartBattle>
    {
     
    }
    

    /// <summary>
    /// 10024
    /// </summary>    
    [API(10024)]
    public class ExitUser:APIBase<Void, Task_L2B_ExitUser> 
    {
        private ExitUser() : base() { }
        public  static ExitUser CreateQuery(){ return new ExitUser();}
    }

    /// <summary>
    /// 10024
    /// </summary>    
    [API(10024)]
    public partial class ExitUserHandler:APIHandler<Void,Task_L2B_ExitUser>
    {
     
    }
    

    /// <summary>
    /// 10025
    /// </summary>    
    [API(10025)]
    public class RegBattleServer:APIBase<B2L_RegBattleServer, L2B_RegBattleServer> 
    {
        private RegBattleServer() : base() { }
        public  static RegBattleServer CreateQuery(){ return new RegBattleServer();}
    }

    /// <summary>
    /// 10025
    /// </summary>    
    [API(10025)]
    public partial class RegBattleServerHandler:APIHandler<B2L_RegBattleServer,L2B_RegBattleServer>
    {
     
    }
    

    /// <summary>
    /// 10026
    /// </summary>    
    [API(10026)]
    public class EndBattle:APIBase<B2L_EndBattle, L2B_EndBattle> 
    {
        private EndBattle() : base() { }
        public  static EndBattle CreateQuery(){ return new EndBattle();}
    }

    /// <summary>
    /// 10026
    /// </summary>    
    [API(10026)]
    public partial class EndBattleHandler:APIHandler<B2L_EndBattle,L2B_EndBattle>
    {
     
    }
    

    /// <summary>
    /// 10027
    /// </summary>    
    [API(10027)]
    public class CheckSession:APIBase<B2L_CheckSession, L2B_CheckSession> 
    {
        private CheckSession() : base() { }
        public  static CheckSession CreateQuery(){ return new CheckSession();}
    }

    /// <summary>
    /// 10027
    /// </summary>    
    [API(10027)]
    public partial class CheckSessionHandler:APIHandler<B2L_CheckSession,L2B_CheckSession>
    {
     
    }
    

}