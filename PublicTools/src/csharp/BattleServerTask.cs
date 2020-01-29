
using Proto;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
namespace Proto.PServices.BattleServerTask
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

    /// <summary>
    /// 10004
    /// </summary>    
    [API(10004)]
    public partial class TaskJoinBattleHandler:APIHandler<Void,Task_G2C_JoinBattle>
    {
     
    }
    

}