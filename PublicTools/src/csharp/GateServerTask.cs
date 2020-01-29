
using Proto;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
namespace Proto.PServices.GateServerTask
{

    /// <summary>
    /// 10013
    /// </summary>    
    [API(10013)]
    public class SyncPackage:APIBase<Void, Task_G2C_SyncPackage> 
    {
        private SyncPackage() : base() { }
        public  static SyncPackage CreateQuery(){ return new SyncPackage();}
    }

    /// <summary>
    /// 10013
    /// </summary>    
    [API(10013)]
    public partial class SyncPackageHandler:APIHandler<Void,Task_G2C_SyncPackage>
    {
     
    }
    

    /// <summary>
    /// 10014
    /// </summary>    
    [API(10014)]
    public class SyncHero:APIBase<Void, Task_G2C_SyncHero> 
    {
        private SyncHero() : base() { }
        public  static SyncHero CreateQuery(){ return new SyncHero();}
    }

    /// <summary>
    /// 10014
    /// </summary>    
    [API(10014)]
    public partial class SyncHeroHandler:APIHandler<Void,Task_G2C_SyncHero>
    {
     
    }
    

}