
using Proto;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
namespace Proto.PServices.C2GService
{

    /// <summary>
    /// 1
    /// </summary>    
    [API(1)]
    public class Login:APIBase<C2G_Login, G2C_Login> 
    {
        private Login() : base() { }
        public  static Login CreateQuery(){ return new Login();}
    }

    /// <summary>
    /// 1
    /// </summary>    
    [API(1)]
    partial class LoginHandler:APIHandler<C2G_Login,G2C_Login>
    {
     
    }
    

    /// <summary>
    /// 2
    /// </summary>    
    [API(2)]
    public class CreateHero:APIBase<C2G_CreateHero, G2C_CreateHero> 
    {
        private CreateHero() : base() { }
        public  static CreateHero CreateQuery(){ return new CreateHero();}
    }

    /// <summary>
    /// 2
    /// </summary>    
    [API(2)]
    partial class CreateHeroHandler:APIHandler<C2G_CreateHero,G2C_CreateHero>
    {
     
    }
    

    /// <summary>
    /// 3
    /// </summary>    
    [API(3)]
    public class BeginGame:APIBase<C2G_BeginGame, G2C_BeginGame> 
    {
        private BeginGame() : base() { }
        public  static BeginGame CreateQuery(){ return new BeginGame();}
    }

    /// <summary>
    /// 3
    /// </summary>    
    [API(3)]
    partial class BeginGameHandler:APIHandler<C2G_BeginGame,G2C_BeginGame>
    {
     
    }
    

}