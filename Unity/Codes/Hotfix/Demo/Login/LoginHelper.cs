using System;


namespace ET
{
    public static class LoginHelper
    {
        public static async ETTask<int> Login(Scene zoneScene, string address, string account, string password)
        {
            A2C_LoginAccount a2C_LoginAccount = null;
            Session accountSession = null;

            try
            {
                accountSession = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(address));
                password = MD5Helper.StringMD5(password);//密码加密
                a2C_LoginAccount = (A2C_LoginAccount)await accountSession.Call(new C2A_LoginAccount()
                {
                    Account = account,
                    Password = password
                });
            }
            catch (Exception e)
            {
                accountSession?.Dispose();
                Log.Error("(PS:服务器可能没开?)连接错误:" + e);
                return ErrorCode.ERR_NetWorkError;//网络错误
            }

            //为什么写这个代码 因为这边是逻辑层  不能显示编写UI界面显示代码 所以返回int型,在外面编写显示登录错误的UI界面
            if (a2C_LoginAccount.Error != ErrorCode.ERR_Success)
            {
                accountSession?.Dispose();
                return a2C_LoginAccount.Error;
            }

            zoneScene.AddComponent<SessionComponent>().Session = accountSession;//保存Session链接
            zoneScene.GetComponent<SessionComponent>().Session.AddComponent<PingComponent>();//心跳包检测

            //保存令牌等
            zoneScene.GetComponent<AccountInfoComponent>().Token = a2C_LoginAccount.Token;
            zoneScene.GetComponent<AccountInfoComponent>().AccountId = a2C_LoginAccount.AccountId;

            return ErrorCode.ERR_Success;
        }

        /// <summary> 获取服务器列表 </summary>
        public static async ETTask<int> GetServerInfos(Scene zoneScene)
        {
            A2C_GetServerInfos a2C_GetServerInfos = null;

            try
            {
                a2C_GetServerInfos = (A2C_GetServerInfos)await zoneScene.GetComponent<SessionComponent>().Session.Call(new C2A_GetServerInfos()
                {
                    AccountId = zoneScene.GetComponent<AccountInfoComponent>().AccountId,
                    Token = zoneScene.GetComponent<AccountInfoComponent>().Token,
                });
            }
            catch (Exception e)
            {
                Log.Debug(e.ToString());
                return ErrorCode.ERR_NetWorkError;
            }

            if (a2C_GetServerInfos.Error != ErrorCode.ERR_Success)
            {
                return a2C_GetServerInfos.Error;
            }
            //存放游戏服务器信息
            foreach (var serverInfoProto in a2C_GetServerInfos.ServerInfoList)
            {
                ServerInfo serverInfo = zoneScene.GetComponent<ServerInfosComponent>().AddChild<ServerInfo>();
                serverInfo.FromMessage(serverInfoProto);
                zoneScene.GetComponent<ServerInfosComponent>().Add(serverInfo);
            }

            return ErrorCode.ERR_Success;
        }

        /// <summary> 创建角色 </summary>
        /// <param name="zoneScene"></param>
        /// <param name="name">角色名称</param>
        /// <returns></returns>
        public static async ETTask<int> CreatRole(Scene zoneScene, string name)
        {
            A2C_CreatRole a2C_CreatRole = null;
            try
            {
                a2C_CreatRole = (A2C_CreatRole)await zoneScene.GetComponent<SessionComponent>().Session.Call(new C2A_CreatRole()
                {
                    AccountId = zoneScene.GetComponent<AccountInfoComponent>().AccountId,
                    Token = zoneScene.GetComponent<AccountInfoComponent>().Token,
                    Name = name,
                    ServerId = zoneScene.GetComponent<ServerInfosComponent>().CurrentServerId,
                });
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return ErrorCode.ERR_NetWorkError;
            }

            if (a2C_CreatRole.Error != ErrorCode.ERR_Success)
            {
                Log.Error(a2C_CreatRole.Error.ToString());
                return a2C_CreatRole.Error;
            }

            //消息类转实体类
            Log.Debug("进入到了实体转化类");
            RoleInfo newRoleInfo = zoneScene.GetComponent<RoleInfosComponent>().AddChild<RoleInfo>();
            newRoleInfo.FromMessage(a2C_CreatRole.RloeInfo);
            zoneScene.GetComponent<RoleInfosComponent>().roleInfosList.Add(newRoleInfo);

            return ErrorCode.ERR_Success;
        }

        /// <summary> 获取角色信息 </summary>
        public static async ETTask<int> GetRoles(Scene zoneScene)
        {
            A2C_GetRoles a2C_GetRoles = null;
            try
            {
                a2C_GetRoles = (A2C_GetRoles)await zoneScene.GetComponent<SessionComponent>().Session.Call(new C2A_GetRoles()
                {
                    AccountId = zoneScene.GetComponent<AccountInfoComponent>().AccountId,
                    Token = zoneScene.GetComponent<AccountInfoComponent>().Token,
                    ServerId = zoneScene.GetComponent<ServerInfosComponent>().CurrentServerId,
                });
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return ErrorCode.ERR_NetWorkError;
            }

            if (a2C_GetRoles.Error != ErrorCode.ERR_Success)
            {
                Log.Error(a2C_GetRoles.Error.ToString());
                return a2C_GetRoles.Error;
            }

            zoneScene.GetComponent<RoleInfosComponent>().roleInfosList.Clear();
            foreach (var roleInfoProto in a2C_GetRoles.RloeInfo)
            {
                RoleInfo roleInfo = zoneScene.GetComponent<RoleInfosComponent>().AddChild<RoleInfo>();
                roleInfo.FromMessage(roleInfoProto);
                zoneScene.GetComponent<RoleInfosComponent>().roleInfosList.Add(roleInfo);
            }

            return ErrorCode.ERR_Success;
        }

        /// <summary> 删除角色 </summary>
        public static async ETTask<int> DeleteRole(Scene zoneScene)
        {
            A2C_DeleteRoles a2C_DeleteRoles = null;

            try
            {
                a2C_DeleteRoles = (A2C_DeleteRoles)await zoneScene.GetComponent<SessionComponent>().Session.Call(new C2A_DeleteRoles()
                {
                    AccountId = zoneScene.GetComponent<AccountInfoComponent>().AccountId,
                    Token = zoneScene.GetComponent<AccountInfoComponent>().Token,
                    RoleInfoId = zoneScene.GetComponent<RoleInfosComponent>().CurrentRoleId,
                    ServerId = zoneScene.GetComponent<ServerInfosComponent>().CurrentServerId,
                });
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return ErrorCode.ERR_NetWorkError;
            }

            if (a2C_DeleteRoles.Error != ErrorCode.ERR_Success)
            {
                Log.Error(a2C_DeleteRoles.Error.ToString());
                return a2C_DeleteRoles.Error;
            }

            //删除本地角色信息
            int index = zoneScene.GetComponent<RoleInfosComponent>().roleInfosList.FindIndex((index) =>
             {
                 return index.Id == a2C_DeleteRoles.DeleteRloeInfoId;
             });
            zoneScene.GetComponent<RoleInfosComponent>().roleInfosList.RemoveAt(index);

            return ErrorCode.ERR_Success;
        }

        /// <summary> 获取网关服务器连接的令牌等 </summary>
        public static async ETTask<int> GetRelamKey(Scene zoneScene)
        {
            A2C_GetRealmKey a2C_GetRealmKey = null;

            try
            {
                a2C_GetRealmKey = (A2C_GetRealmKey)await zoneScene.GetComponent<SessionComponent>().Session.Call(new C2A_GetRealmKey()
                {
                    Token = zoneScene.GetComponent<AccountInfoComponent>().Token,
                    AccountId = zoneScene.GetComponent<AccountInfoComponent>().AccountId,
                    ServerId = zoneScene.GetComponent<ServerInfosComponent>().CurrentServerId,
                });
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return ErrorCode.ERR_NetWorkError;
            }

            if (a2C_GetRealmKey.Error != ErrorCode.ERR_Success)
            {
                Log.Error(a2C_GetRealmKey.Error.ToString());
                return a2C_GetRealmKey.Error;
            }

            //保存网关服务器的信息
            zoneScene.GetComponent<AccountInfoComponent>().RealmKey = a2C_GetRealmKey.RealmKey;
            zoneScene.GetComponent<AccountInfoComponent>().RealmAddress = a2C_GetRealmKey.RealmAddress;
            zoneScene.GetComponent<SessionComponent>().Session.Dispose();

            return ErrorCode.ERR_Success;
        }

        /// <summary> 登录游戏 </summary>
        public static async ETTask<int> EnterGame(Scene zoneScene)
        {
            string realmAddress = zoneScene.GetComponent<AccountInfoComponent>().RealmAddress;

            //1. 连接Realm,获取分配的Gate
            R2C_LoginRealm r2C_LoginRealm;

            Session session = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(realmAddress));
            try
            {
                r2C_LoginRealm = (R2C_LoginRealm)await session.Call(new C2R_LoginRealm()
                {
                    AccountId = zoneScene.GetComponent<AccountInfoComponent>().AccountId,
                    RealmTokenKey = zoneScene.GetComponent<AccountInfoComponent>().RealmKey,
                });
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                session?.Dispose();
                return ErrorCode.ERR_NetWorkError;
            }
            session?.Dispose();

            if (r2C_LoginRealm.Error != ErrorCode.ERR_Success)
            {
                Log.Error(r2C_LoginRealm.Error.ToString());
                return r2C_LoginRealm.Error;
            }

            Log.Warning($"GateAddress : {r2C_LoginRealm.GateAddress}");
            Session gateSession = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(r2C_LoginRealm.GateAddress));
            gateSession.AddComponent<PingComponent>();
            zoneScene.GetComponent<SessionComponent>().Session = gateSession;

            //2. 开始连接Gate
            G2C_LoginGameGate g2C_LoginGameGate = null;

            try
            {
                g2C_LoginGameGate = (G2C_LoginGameGate)await gateSession.Call(new C2G_LoginGameGate()
                {
                    Key = r2C_LoginRealm.GateSessionKey,
                    Account = zoneScene.GetComponent<AccountInfoComponent>().AccountId,
                    RoleId = zoneScene.GetComponent<RoleInfosComponent>().CurrentRoleId,
                });
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                zoneScene.GetComponent<SessionComponent>().Session?.Dispose();
                return ErrorCode.ERR_NetWorkError;
            }

            if (g2C_LoginGameGate.Error != ErrorCode.ERR_Success)
            {
                zoneScene.GetComponent<SessionComponent>().Session?.Dispose();
                return g2C_LoginGameGate.Error;
            }

            Log.Debug("登录Gate成功!");

            //3. 角色正式请求进入游戏逻辑服
            G2C_EnterGame g2C_EnterGame = null;
            try
            {
                g2C_EnterGame = (G2C_EnterGame)await gateSession.Call(new C2G_EnterGame() { });
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                zoneScene.GetComponent<SessionComponent>().Session?.Dispose();
                return ErrorCode.ERR_NetWorkError;
            }

            if (g2C_EnterGame.Error != ErrorCode.ERR_Success)
            {
                Log.Error(g2C_EnterGame.Error.ToString());
                return g2C_EnterGame.Error;
            }
            Log.Debug("角色进入游戏成功!!");

            return ErrorCode.ERR_Success;
        }
    }
}