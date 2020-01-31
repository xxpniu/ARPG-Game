
using Proto;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Proto.PServices;
namespace Proto.BattleServerService
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
    /// 10002
    /// </summary>    
    [API(10002)]
    public class ExitGame:APIBase<C2B_ExitGame, B2C_ExitGame> 
    {
        private ExitGame() : base() { }
        public  static ExitGame CreateQuery(){ return new ExitGame();}
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
    

    public interface IBattleServerService
    {
        [API(10003)]B2C_JoinBattle JoinBattle(C2B_JoinBattle req);
        [API(10002)]B2C_ExitGame ExitGame(C2B_ExitGame req);
        [API(10001)]B2C_ExitBattle ExitBattle(C2B_ExitBattle req);

    }
   

}