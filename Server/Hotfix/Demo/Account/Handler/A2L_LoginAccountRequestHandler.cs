using System;

namespace ET
{
    [ActorMessageHandler]
    public class A2L_LoginAccountRequestHandler : AMActorRpcHandler<Scene, A2L_LoginAccountRequest, L2A_LoginAccountResponse>
    {
        protected override async ETTask Run(Scene scene, A2L_LoginAccountRequest request, L2A_LoginAccountResponse response, Action reply)
        {
            long accountId = request.AccountId;

            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LogoutCenterLock, accountId.GetHashCode()))
            {
                if (!scene.GetComponent<LoginInfoRecordComponent>().IsExit(accountId))
                {
                    reply();
                    return;
                }

                int zone = scene.GetComponent<LoginInfoRecordComponent>().Get(accountId);//获取区服ID
                StartSceneConfig gateConfig = RealmGateAddressHelper.GetGate(zone, accountId);//获取区服信息

                //为啥使用MessageHelper.CallActor而不用ActorMessageSenderComponent.Instance.Call
                //主要原因:好看
                var g2LDisconnectGateUnitResponse = (G2L_DisconnectGateUnitResponse)await MessageHelper.CallActor(gateConfig.InstanceId, new L2G_DisconnectGateUnitRequest()
                {
                    AccountId = accountId,
                });

                response.Error = g2LDisconnectGateUnitResponse.Error;
                reply();
            }
        }
    }
}
