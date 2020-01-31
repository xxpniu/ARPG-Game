
using Proto;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Proto.PServices;
namespace Proto.GateServerTask
{

    /// <summary>
    /// 10024
    /// </summary>    
    [API(10024)]
    public class SyncPackage:APIBase<Task_G2C_SyncPackage, Task_G2C_SyncPackage> 
    {
        private SyncPackage() : base() { }
        public  static SyncPackage CreateQuery(){ return new SyncPackage();}
    }
    

    /// <summary>
    /// 10025
    /// </summary>    
    [API(10025)]
    public class SyncHero:APIBase<Task_G2C_SyncHero, Task_G2C_SyncHero> 
    {
        private SyncHero() : base() { }
        public  static SyncHero CreateQuery(){ return new SyncHero();}
    }
    

    public interface IGateServerTask
    {
        [API(10025)]Task_G2C_SyncHero SyncHero(Task_G2C_SyncHero req);
        [API(10024)]Task_G2C_SyncPackage SyncPackage(Task_G2C_SyncPackage req);

    }
   

}