
using Proto;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Proto.PServices;
using System.Threading.Tasks;
namespace Proto.BattleServerService
{

    /// <summary>
    /// 10022
    /// </summary>    
    [API(10022)]
    public class ExitBattle:APIBase<C2B_ExitBattle, B2C_ExitBattle> 
    {
        private ExitBattle() : base() { }
        public  static ExitBattle CreateQuery(){ return new ExitBattle();}
    }
    

    /// <summary>
    /// 10023
    /// </summary>    
    [API(10023)]
    public class ExitGame:APIBase<C2B_ExitGame, B2C_ExitGame> 
    {
        private ExitGame() : base() { }
        public  static ExitGame CreateQuery(){ return new ExitGame();}
    }
    

    /// <summary>
    /// 10024
    /// </summary>    
    [API(10024)]
    public class JoinBattle:APIBase<C2B_JoinBattle, B2C_JoinBattle> 
    {
        private JoinBattle() : base() { }
        public  static JoinBattle CreateQuery(){ return new JoinBattle();}
    }
    

    public interface IBattleServerService
    {
        [API(10024)]B2C_JoinBattle JoinBattle(C2B_JoinBattle req);
        [API(10023)]B2C_ExitGame ExitGame(C2B_ExitGame req);
        [API(10022)]B2C_ExitBattle ExitBattle(C2B_ExitBattle req);

    }
   

    public abstract class BattleServerService
    {
        [API(10024)]public abstract Task<B2C_JoinBattle> JoinBattle(C2B_JoinBattle request);
        [API(10023)]public abstract Task<B2C_ExitGame> ExitGame(C2B_ExitGame request);
        [API(10022)]public abstract Task<B2C_ExitBattle> ExitBattle(C2B_ExitBattle request);

    }

}