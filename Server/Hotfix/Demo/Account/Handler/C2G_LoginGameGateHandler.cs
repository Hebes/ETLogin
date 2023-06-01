using System;

namespace ET
{
    [FriendClass(typeof(SessionPlayerComponent))]
    public class C2G_LoginGameGateHandler : AMRpcHandler<C2G_LoginGameGate, G2C_LoginGameGate>
    {
        protected override async ETTask Run(Session session, C2G_LoginGameGate request, G2C_LoginGameGate response, Action reply)
        {
            if (session.DomainScene().SceneType != SceneType.Gate)
            {
                Log.Error($"请求的Scene错误,当前的Scene为:{session.DomainScene().SceneType}");
                session.Dispose();
                return;
            }

            //为保持长时间连接必须移除SessionAcceptTimeoutComponent组件
            session.RemoveComponent<SessionAcceptTimeoutComponent>();

            //防止多次点击登录
            if (session.GetComponent<SessionLockingComponent>() != null)
            {
                response.Error = ErrorCode.ERR_RequestRepeatedlyError;
                reply();//用于发送response这条消息
                return;
            }

            Scene scene = session.DomainScene();
            string tokenKey = scene.GetComponent<GateSessionKeyComponent>().Get(request.Account);
            if (tokenKey == null || !tokenKey.Equals(request.Key))
            {
                response.Error = ErrorCode.ERR_ConnectGateKeyError;
                response.Message = "Gate Key验证失败!";
                reply();
                session?.Disconnect().Coroutine();
                return;
            }

            scene.GetComponent<GateSessionKeyComponent>().Remove(request.Account);
            long instanceId = session.InstanceId;
            //防止频繁创建游戏角色
            using (session.AddComponent<SessionLockingComponent>())
            {
                using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginGate, request.Account.GetHashCode()))
                {

                    if (instanceId != session.InstanceId) return;

                    //通知登录中心服,记录本次登录的服务器Zone
                    StartSceneConfig loginCenterConfig = StartSceneConfigCategory.Instance.loginCenterConfig;
                    L2G_AddLoginRecord l2ARoleLogin = (L2G_AddLoginRecord)await MessageHelper.CallActor(loginCenterConfig.InstanceId, new G2L_AddLoginRecord()
                    {
                        AccountId = request.Account,
                        ServerId = scene.Zone,
                    });


                    if (l2ARoleLogin.Error != ErrorCode.ERR_Success)
                    {
                        response.Error = l2ARoleLogin.Error;
                        reply();
                        session?.Disconnect().Coroutine();
                        return;
                    }

                    SessionStateComponent sessionStateComponent = session.GetComponent<SessionStateComponent>();
                    if (sessionStateComponent == null)
                    {
                        sessionStateComponent = session.AddComponent<SessionStateComponent>();
                    }
                    sessionStateComponent.State = SessionState.Normal;

                    Player player = scene.GetComponent<PlayerComponent>().Get(request.Account);

                    if (player == null)
                    {
                        //添加一个新的GateUnit
                        player = scene.GetComponent<PlayerComponent>().AddChildWithId<Player, long, long>(request.RoleId, request.Account, request.RoleId);
                        player.PlayerState = PlayerState.Gate;
                        scene.GetComponent<PlayerComponent>().Add(player);
                        //添加MailBoxComponent组件后session这个实体就有处理Actor消息的能力
                        session.AddComponent<MailBoxComponent, MailboxType>(MailboxType.GateSession);
                    }
                    else
                    {
                        //TODO角色下线
                        player.RemoveComponent<PlayerOfflineOutTimeComponent>();
                    }

                    session.AddComponent<SessionPlayerComponent>().PlayerId = player.Id;
                    session.GetComponent<SessionPlayerComponent>().PlayerInstanceId = player.InstanceId;
                    session.GetComponent<SessionPlayerComponent>().AccountId = request.Account;
                    player.SessionInstanceId = session.InstanceId;
                }
                reply();
            }
        }
    }
}
