using System;

namespace ET
{
    public class L2G_DisconnectGateUnitRequestHandler : AMActorRpcHandler<Scene, L2G_DisconnectGateUnitRequest, G2L_DisconnectGateUnitResponse>
    {
        protected override async ETTask Run(Scene scene, L2G_DisconnectGateUnitRequest request, G2L_DisconnectGateUnitResponse response, Action reply)
        {
            long accountId = request.AccountId;

            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.GateLoginLock, accountId.GetHashCode()))
            {
                PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
                Player gateUnit = playerComponent.Get(accountId);
                if (gateUnit == null)
                {
                    reply();
                    return;
                }
                playerComponent.Remove(accountId);

                //tudo 临时处理
                gateUnit.Dispose();
            }
            reply();
        }
    }
}
