using System;
namespace ServerUtility
{

    [AttributeUsage(AttributeTargets.Class)]
    public class TaskHandlerAttribute : Attribute
    {
        public TaskHandlerAttribute(Type type)
        {
            this.RType = type;
        }

        public Type RType { set; get; }
    }

    public class TaskHandler
    {
    }
}
