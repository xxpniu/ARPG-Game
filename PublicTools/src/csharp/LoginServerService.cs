
using Proto;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
namespace Proto.PServices.LoginServerService
{

    /// <summary>
    /// 10017
    /// </summary>    
    [API(10017)]
    public class Login:APIBase<C2L_Login, L2C_Login> 
    {
        private Login() : base() { }
        public  static Login CreateQuery(){ return new Login();}
    }

    /// <summary>
    /// 10017
    /// </summary>    
    [API(10017)]
    public partial class LoginHandler:APIHandler<C2L_Login,L2C_Login>
    {
     
    }
    

    /// <summary>
    /// 10018
    /// </summary>    
    [API(10018)]
    public class Reg:APIBase<C2L_Reg, L2C_Reg> 
    {
        private Reg() : base() { }
        public  static Reg CreateQuery(){ return new Reg();}
    }

    /// <summary>
    /// 10018
    /// </summary>    
    [API(10018)]
    public partial class RegHandler:APIHandler<C2L_Reg,L2C_Reg>
    {
     
    }
    

}