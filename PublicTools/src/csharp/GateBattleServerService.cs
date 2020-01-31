
using Proto;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Proto.PServices;
namespace Proto.GateBattleServerService
{

    /// <summary>
    /// 10026
    /// </summary>    
    [API(10026)]
    public class GetPlayerInfo:APIBase<B2G_GetPlayerInfo, G2B_GetPlayerInfo> 
    {
        private GetPlayerInfo() : base() { }
        public  static GetPlayerInfo CreateQuery(){ return new GetPlayerInfo();}
    }
    

    /// <summary>
    /// 10027
    /// </summary>    
    [API(10027)]
    public class BattleReward:APIBase<B2G_BattleReward, G2B_BattleReward> 
    {
        private BattleReward() : base() { }
        public  static BattleReward CreateQuery(){ return new BattleReward();}
    }
    

    public interface IGateBattleServerService
    {
        [API(10027)]G2B_BattleReward BattleReward(B2G_BattleReward req);
        [API(10026)]G2B_GetPlayerInfo GetPlayerInfo(B2G_GetPlayerInfo req);

    }
   

}