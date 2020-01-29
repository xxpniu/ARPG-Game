
using Proto;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
namespace Proto.PServices.GateBattleServerService
{

    /// <summary>
    /// 10015
    /// </summary>    
    [API(10015)]
    public class GetPlayerInfo:APIBase<B2G_GetPlayerInfo, G2B_GetPlayerInfo> 
    {
        private GetPlayerInfo() : base() { }
        public  static GetPlayerInfo CreateQuery(){ return new GetPlayerInfo();}
    }

    /// <summary>
    /// 10015
    /// </summary>    
    [API(10015)]
    public partial class GetPlayerInfoHandler:APIHandler<B2G_GetPlayerInfo,G2B_GetPlayerInfo>
    {
     
    }
    

    /// <summary>
    /// 10016
    /// </summary>    
    [API(10016)]
    public class BattleReward:APIBase<B2G_BattleReward, G2B_BattleReward> 
    {
        private BattleReward() : base() { }
        public  static BattleReward CreateQuery(){ return new BattleReward();}
    }

    /// <summary>
    /// 10016
    /// </summary>    
    [API(10016)]
    public partial class BattleRewardHandler:APIHandler<B2G_BattleReward,G2B_BattleReward>
    {
     
    }
    

}