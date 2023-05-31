using System;

namespace ET
{
    public class C2A_GetRealmKeyHandler : AMRpcHandler<C2A_GetRealmKey, A2C_GetRealmKey>
    {
        protected override async ETTask Run(Session session, C2A_GetRealmKey request, A2C_GetRealmKey response, Action reply)
        {
            if (session.DomainScene().SceneType != SceneType.Account)
            {
                Log.Error($"请求的Scene错误,当前的Scene为:{session.DomainScene().SceneType}");
                session.Dispose();
                return;
            }

            //防止多次点击登录
            if (session.GetComponent<SessionLockingComponent>() != null)
            {
                response.Error = ErrorCode.ERR_RequestRepeatedlyError;
                reply();//用于发送response这条消息
                session.Disconnect().Coroutine();
                return;
            }

            string Token = session.DomainScene().GetComponent<TokenComponent>().Get(request.AccountId);

            if (Token == null || Token != request.Token)
            {
                response.Error = ErrorCode.ERR_TokenError;
                reply();
                session.Disconnect().Coroutine();
                return;
            }

            //防止频繁创建游戏角色
            using (session.AddComponent<SessionLockingComponent>())
            {
                //在C2A_LoginAccountHandler这里也有用到
                //为什么用到LoginAccount:假设账号服务器正在和Ralem服务器发送消息中另外一个玩家登录这个账号把原先的玩家踢下线,
                //那么这个请求有效性就会收到质疑,帐号服务器虽然收到Realm服务器的回复消息但是玩家被踢下线,那么消息应该发给谁.
                using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginAccount, request.AccountId))
                {
                    StartSceneConfig realmStartSceneConfig = RealmGateAddressHelper.GetRealm(request.ServerId);

                    //进程之间消息发送可以用MessageHelper
                    //realmStartSceneConfig.InstanceId包含Realm服务器的地址
                    R2A_GetRealmKey r2A_GetRealmKey = (R2A_GetRealmKey)await MessageHelper.CallActor(realmStartSceneConfig.InstanceId,
                        new A2R_GetRealmKey()
                        {
                            AccountId = request.AccountId,
                        });

                    //失败了
                    if (r2A_GetRealmKey.Error!= ErrorCode.ERR_Success)
                    {
                        response.Error = r2A_GetRealmKey.Error;
                        reply();
                        session.Disconnect().Coroutine();
                    }
                    //成功 给游戏客户端返回消息
                    response.RealmKey = r2A_GetRealmKey.RealmKey;
                    response.RealmAddress = realmStartSceneConfig.OuterIPPort.ToString();
                    reply();
                    session.Disconnect().Coroutine();
                }
            }
        }
    }
}
