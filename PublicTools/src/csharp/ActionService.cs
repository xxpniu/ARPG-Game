
using Proto;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Proto.PServices;
namespace Proto.ActionService
{

    /// <summary>
    /// 10018
    /// </summary>    
    [API(10018)]
    public class ClickMapGround:APIBase<Action_ClickMapGround, Action_ClickMapGround> 
    {
        private ClickMapGround() : base() { }
        public  static ClickMapGround CreateQuery(){ return new ClickMapGround();}
    }
    

    /// <summary>
    /// 10019
    /// </summary>    
    [API(10019)]
    public class ClickSkillIndex:APIBase<Action_ClickSkillIndex, Action_ClickSkillIndex> 
    {
        private ClickSkillIndex() : base() { }
        public  static ClickSkillIndex CreateQuery(){ return new ClickSkillIndex();}
    }
    

    /// <summary>
    /// 10020
    /// </summary>    
    [API(10020)]
    public class AutoFindTarget:APIBase<Action_AutoFindTarget, Action_AutoFindTarget> 
    {
        private AutoFindTarget() : base() { }
        public  static AutoFindTarget CreateQuery(){ return new AutoFindTarget();}
    }
    

    /// <summary>
    /// 10021
    /// </summary>    
    [API(10021)]
    public class MoveDir:APIBase<Action_MoveDir, Action_MoveDir> 
    {
        private MoveDir() : base() { }
        public  static MoveDir CreateQuery(){ return new MoveDir();}
    }
    

    public interface IActionService
    {
        [API(10021)]Action_MoveDir MoveDir(Action_MoveDir req);
        [API(10020)]Action_AutoFindTarget AutoFindTarget(Action_AutoFindTarget req);
        [API(10019)]Action_ClickSkillIndex ClickSkillIndex(Action_ClickSkillIndex req);
        [API(10018)]Action_ClickMapGround ClickMapGround(Action_ClickMapGround req);

    }
   

}