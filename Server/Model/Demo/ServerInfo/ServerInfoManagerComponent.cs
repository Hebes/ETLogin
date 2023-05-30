using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    [ChildType]
    public class ServerInfoManagerComponent : Entity, IAwake,IDestroy,ILoad
    {
        public List<ServerInfo> ServerInfos { get; set; } = new List<ServerInfo>();
    }
}
