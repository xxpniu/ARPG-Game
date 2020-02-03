
using Proto;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Proto.PServices;
using System.Threading.Tasks;
namespace Proto.GateBattleServerService
{

    /// <summary>
    /// 10046
    /// </summary>    
    [API(10046)]
    public class GetPlayerInfo:APIBase<B2G_GetPlayerInfo, G2B_GetPlayerInfo> 
    {
        private GetPlayerInfo() : base() { }
        public  static GetPlayerInfo CreateQuery(){ return new GetPlayerInfo();}
    }
    

    /// <summary>
    /// 10047
    /// </summary>    
    [API(10047)]
    public class BattleReward:APIBase<B2G_BattleReward, G2B_BattleReward> 
    {
        private BattleReward() : base() { }
        public  static BattleReward CreateQuery(){ return new BattleReward();}
    }
    

    public interface IGateBattleServerService
    {
        [API(10047)]G2B_BattleReward BattleReward(B2G_BattleReward req);
        [API(10046)]G2B_GetPlayerInfo GetPlayerInfo(B2G_GetPlayerInfo req);

    }
   

    public abstract class GateBattleServerService
    {
        [API(10047)]public abstract Task<G2B_BattleReward> BattleReward(B2G_BattleReward request);
        [API(10046)]public abstract Task<G2B_GetPlayerInfo> GetPlayerInfo(B2G_GetPlayerInfo request);

    }

}