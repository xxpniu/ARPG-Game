
using Proto;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Proto.PServices;
namespace Proto.GateServerTask
{

    /// <summary>
    /// 10044
    /// </summary>    
    [API(10044)]
    public class SyncPackage:APIBase<Task_G2C_SyncPackage, Task_G2C_SyncPackage> 
    {
        private SyncPackage() : base() { }
        public  static SyncPackage CreateQuery(){ return new SyncPackage();}
    }
    

    /// <summary>
    /// 10045
    /// </summary>    
    [API(10045)]
    public class SyncHero:APIBase<Task_G2C_SyncHero, Task_G2C_SyncHero> 
    {
        private SyncHero() : base() { }
        public  static SyncHero CreateQuery(){ return new SyncHero();}
    }
    

    /// <summary>
    /// 10046
    /// </summary>    
    [API(10046)]
    public class JoinBattle:APIBase<Task_G2C_JoinBattle, Task_G2C_JoinBattle> 
    {
        private JoinBattle() : base() { }
        public  static JoinBattle CreateQuery(){ return new JoinBattle();}
    }
    

    public interface IGateServerTask
    {
        [API(10046)]Task_G2C_JoinBattle JoinBattle(Task_G2C_JoinBattle req);
        [API(10045)]Task_G2C_SyncHero SyncHero(Task_G2C_SyncHero req);
        [API(10044)]Task_G2C_SyncPackage SyncPackage(Task_G2C_SyncPackage req);

    }
   

}