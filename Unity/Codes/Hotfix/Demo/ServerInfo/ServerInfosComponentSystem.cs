namespace ET
{

    public class ServerInfosComponentAwakeSystem : AwakeSystem<ServerInfosComponent>
    {
        public override void Awake(ServerInfosComponent self)
        {
            foreach (var serverInfo in self.serverInfoList)
            {
                serverInfo?.Dispose();
            }
            self.serverInfoList.Clear();
        }
    }

    public class ServerInfosComponentDestroySystem : DestroySystem<ServerInfosComponent>
    {
        public override void Destroy(ServerInfosComponent self)
        {
        }
    }

    [FriendClass(typeof(ServerInfosComponent))]
    public static class ServerInfosComponentSystem
    {
        public static void Add(this ServerInfosComponent self, ServerInfo serverInfo)
        {
            self.serverInfoList.Add(serverInfo);
        }
    }
}
