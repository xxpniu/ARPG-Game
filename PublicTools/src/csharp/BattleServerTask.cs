
using Proto;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Proto.PServices;
namespace Proto.BattleServerTask
{

    /// <summary>
    /// 10004
    /// </summary>    
    [API(10004)]
    public class TaskJoinBattle:APIBase<Void, Task_G2C_JoinBattle> 
    {
        private TaskJoinBattle() : base() { }
        public  static TaskJoinBattle CreateQuery(){ return new TaskJoinBattle();}
    }
    

    public interface IBattleServerTask
    {
        [API(10004)]Task_G2C_JoinBattle TaskJoinBattle(Void req);

    }
   

}