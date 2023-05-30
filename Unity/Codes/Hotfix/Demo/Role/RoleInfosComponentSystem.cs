namespace ET
{
    public class RoleInfosComponentSystemAwakeSystem : AwakeSystem<RoleInfosComponent>
    {
        public override void Awake(RoleInfosComponent self)
        {
        }
    }

    public class RoleInfosComponentSystemDestroySystem : DestroySystem<RoleInfosComponent>
    {
        public override void Destroy(RoleInfosComponent self)
        {
            foreach (var roleInfo in self.roleInfosList)
                roleInfo?.Dispose();
            self.roleInfosList.Clear();
            self.CurrentRoleId = 0;
        }
    }

    /// <summary> 角色信息系统 </summary>
    public static  class RoleInfosComponentSystem
    {
    }
}
