
using Proto;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
namespace Proto.PServices.L2GService
{

    /// <summary>
    /// 1
    /// </summary>    
    [API(1)]
    public class BeginBattle:APIBase<L2G_BeginBattle, G2L_BeginBattle> 
    {
        private BeginBattle() : base() { }
        public  static BeginBattle CreateQuery(){ return new BeginBattle();}
    }

    /// <summary>
    /// 1
    /// </summary>    
    [API(1)]
    partial class BeginBattleHandler:APIHandler<L2G_BeginBattle,G2L_BeginBattle>
    {
     
    }
    

}