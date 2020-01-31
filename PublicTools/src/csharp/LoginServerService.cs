
using Proto;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Proto.PServices;
namespace Proto.LoginServerService
{

    /// <summary>
    /// 10025
    /// </summary>    
    [API(10025)]
    public class Login:APIBase<C2L_Login, L2C_Login> 
    {
        private Login() : base() { }
        public  static Login CreateQuery(){ return new Login();}
    }
    

    /// <summary>
    /// 10026
    /// </summary>    
    [API(10026)]
    public class Reg:APIBase<C2L_Reg, L2C_Reg> 
    {
        private Reg() : base() { }
        public  static Reg CreateQuery(){ return new Reg();}
    }
    

    public interface ILoginServerService
    {
        [API(10026)]L2C_Reg Reg(C2L_Reg req);
        [API(10025)]L2C_Login Login(C2L_Login req);

    }
   

}