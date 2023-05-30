using System.Collections.Generic;

namespace ET
{
    /// <summary> 存储游戏服务器创建出来的角色信息 </summary>
    [ComponentOf(typeof(Scene))]
    [ChildType]
    public class RoleInfosComponent : Entity, IAwake, IDestroy
    {
        /// <summary> 角色信息列表 </summary>
        public List<RoleInfo> roleInfosList { get; set; } = new List<RoleInfo>();
        /// <summary> 当前的角色ID </summary>
        public long CurrentRoleId { get; set; } = 0;
    }
}
