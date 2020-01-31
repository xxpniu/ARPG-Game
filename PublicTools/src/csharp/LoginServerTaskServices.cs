
using Proto;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Proto.PServices;
namespace Proto.LoginServerTaskServices
{

    /// <summary>
    /// 10014
    /// </summary>    
    [API(10014)]
    public class StartBattle:APIBase<Task_L2B_StartBattle, Task_L2B_StartBattle> 
    {
        private StartBattle() : base() { }
        public  static StartBattle CreateQuery(){ return new StartBattle();}
    }
    

    /// <summary>
    /// 10015
    /// </summary>    
    [API(10015)]
    public class ExitUser:APIBase<Task_L2B_ExitUser, Task_L2B_ExitUser> 
    {
        private ExitUser() : base() { }
        public  static ExitUser CreateQuery(){ return new ExitUser();}
    }
    

    public interface ILoginServerTaskServices
    {
        [API(10015)]Task_L2B_ExitUser ExitUser(Task_L2B_ExitUser req);
        [API(10014)]Task_L2B_StartBattle StartBattle(Task_L2B_StartBattle req);

    }
   

}