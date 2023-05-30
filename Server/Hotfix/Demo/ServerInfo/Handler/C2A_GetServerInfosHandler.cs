using System;
using System.Diagnostics;

namespace ET
{
    /// <summary> 获取游戏服务器端下发游戏区服信息 </summary>
    public class C2A_GetServerInfosHandler : AMRpcHandler<C2A_GetServerInfos, A2C_GetServerInfos>
    {
        protected override async ETTask Run(Session session, C2A_GetServerInfos request, A2C_GetServerInfos response, Action reply)
        {
            if (session.DomainScene().SceneType != SceneType.Account)
            {
                Log.Error($"请求的Sceen错误,当前Sceen为:{session.DomainScene().SceneType}");
                session.Dispose();
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

            foreach (var sererInfo in session.DomainScene().GetComponent<ServerInfoManagerComponent>().ServerInfos)
                response.ServerInfoList.Add(sererInfo.ToMassage());

            reply();

            await ETTask.CompletedTask;
        }
    }
}
