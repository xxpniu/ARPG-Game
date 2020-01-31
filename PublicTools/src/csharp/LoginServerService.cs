
using Proto;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Proto.PServices;
namespace Proto.LoginServerService
{

    /// <summary>
    /// 10005
    /// </summary>    
    [API(10005)]
    public class Login:APIBase<C2L_Login, L2C_Login> 
    {
        private Login() : base() { }
        public  static Login CreateQuery(){ return new Login();}
    }
    

    /// <summary>
    /// 10006
    /// </summary>    
    [API(10006)]
    public class Reg:APIBase<C2L_Reg, L2C_Reg> 
    {
        private Reg() : base() { }
        public  static Reg CreateQuery(){ return new Reg();}
    }
    

    public interface ILoginServerService
    {
        [API(10006)]L2C_Reg Reg(C2L_Reg req);
        [API(10005)]L2C_Login Login(C2L_Login req);

    }
   

}