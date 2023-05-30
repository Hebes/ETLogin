namespace ET
{
    /// <summary> 游戏角色 </summary>
    public class RoleInfo : Entity, IAwake
    {
        /// <summary> 角色名称 </summary>
        public string Name { get; set; }
        /// <summary> 区服 </summary>
        public int ServerId { get; set; }
        /// <summary> 角色当前的状态 </summary>
        public int State { get; set; }
        /// <summary> 角色所属的账号ID </summary>
        public long AccountId { get; set; }
        /// <summary> 最后的登录时间 </summary>
        public long LastLoginTime { get; set; }
        /// <summary> 创建的时间 </summary>
        public long CreatTiemr { get; set; }
    }
}
