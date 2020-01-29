
using Proto;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
namespace Proto.PServices.BattleServerService
{

    /// <summary>
    /// 10001
    /// </summary>    
    [API(10001)]
    public class ExitBattle:APIBase<C2B_ExitBattle, B2C_ExitBattle> 
    {
        private ExitBattle() : base() { }
        public  static ExitBattle CreateQuery(){ return new ExitBattle();}
    }

    /// <summary>
    /// 10001
    /// </summary>    
    [API(10001)]
    public partial class ExitBattleHandler:APIHandler<C2B_ExitBattle,B2C_ExitBattle>
    {
     
    }
    

    /// <summary>
    /// 10002
    /// </summary>    
    [API(10002)]
    public class ExitGame:APIBase<C2B_ExitGame, B2C_ExitGame> 
    {
        private ExitGame() : base() { }
        public  static ExitGame CreateQuery(){ return new ExitGame();}
    }

    /// <summary>
    /// 10002
    /// </summary>    
    [API(10002)]
    public partial class ExitGameHandler:APIHandler<C2B_ExitGame,B2C_ExitGame>
    {
     
    }
    

    /// <summary>
    /// 10003
    /// </summary>    
    [API(10003)]
    public class JoinBattle:APIBase<C2B_JoinBattle, B2C_JoinBattle> 
    {
        private JoinBattle() : base() { }
        public  static JoinBattle CreateQuery(){ return new JoinBattle();}
    }

    /// <summary>
    /// 10003
    /// </summary>    
    [API(10003)]
    public partial class JoinBattleHandler:APIHandler<C2B_JoinBattle,B2C_JoinBattle>
    {
     
    }
    

}