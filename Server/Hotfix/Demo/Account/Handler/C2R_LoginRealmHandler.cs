using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class C2R_LoginRealmHandler : AMRpcHandler<C2R_LoginRealm, R2C_LoginRealm>
    {
        protected override async ETTask Run(Session session, C2R_LoginRealm request, R2C_LoginRealm response, Action reply)
        {
            if (session.DomainScene().SceneType != SceneType.Realm)
            {
                Log.Error($"请求的Scene错误,当前的Scene为:{session.DomainScene().SceneType}");
                session.Dispose();
                return;
            }
            Scene domainScene = session.DomainScene();

            //防止多次点击登录
            if (session.GetComponent<SessionLockingComponent>() != null)
            {
                response.Error = ErrorCode.ERR_RequestRepeatedlyError;
                reply();//用于发送response这条消息
                session?.Disconnect().Coroutine();
                return;
            }

            string Token = session.DomainScene().GetComponent<TokenComponent>().Get(request.AccountId);

            if (Token == null || Token != request.RealmTokenKey)
            {
                response.Error = ErrorCode.ERR_TokenError;
                reply();
                session?.Disconnect().Coroutine();
                return;
            }

            domainScene.GetComponent<TokenComponent>().Remove(request.AccountId);//之后不会在连接Realm服务器

            //防止频繁创建游戏角色
            using (session.AddComponent<SessionLockingComponent>())
            {
                using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginRealm, request.AccountId))
                {
                    //取模固定分配一个Gate
                    //domainScene.Zone 一服 看配置表都是1
                    StartSceneConfig config = RealmGateAddressHelper.GetGate(domainScene.Zone, request.AccountId);

                    //进程之间消息发送可以用MessageHelper
                    //同Gate请求一个key,客户端可以拿着这个Key请求gate
                    G2R_GetLoginGateKey g2R_GetLoginGateKey = (G2R_GetLoginGateKey)await MessageHelper.CallActor(config.InstanceId,
                        new R2G_GetLoginGateKey()
                        {
                            AccountId = request.AccountId,
                        });

                    //失败了
                    if (g2R_GetLoginGateKey.Error != ErrorCode.ERR_Success)
                    {
                        response.Error = g2R_GetLoginGateKey.Error;
                        reply();
                        return;
                    }
                    //成功 给游戏客户端返回消息
                    response.GateSessionKey = g2R_GetLoginGateKey.GateSessionKey;
                    response.GateAddress = config.OuterIPPort.ToString();
                    reply();
                    session?.Disconnect().Coroutine();
                }
            }
        }
    }
}
