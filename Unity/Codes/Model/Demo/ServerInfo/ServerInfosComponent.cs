using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    [ChildType]
    /// <summary> 服务器信息列表 </summary>
    public class ServerInfosComponent : Entity, IAwake, IDestroy
    {
        /// <summary> 服务器列表 </summary>
        public List<ServerInfo> serverInfoList { get; set; } = new List<ServerInfo>();
        /// <summary> 需要进入服务器的ID </summary>
        public int CurrentServerId { get; set; } = 0;
    }
}
