using System;
using Proto;

namespace MapServer.GameViews
{
    /// <summary>
    /// Serializerable element.
    /// </summary>
    public interface ISerializerableElement
    {
        /// <summary>
        /// 获取初始化数据
        /// </summary>
        /// <returns>The init notify.</returns>
        ISerializerable GetInitNotify();
    }
}
