using System;

namespace ET
{
    [FriendClass(typeof(SessionPlayerComponent))]
    public class L2G_DisconnectGateUnitRequestHandler : AMActorRpcHandler<Scene, L2G_DisconnectGateUnitRequest, G2L_DisconnectGateUnitResponse>
    {
        protected override async ETTask Run(Scene scene, L2G_DisconnectGateUnitRequest request, G2L_DisconnectGateUnitResponse response, Action reply)
        {
            long accountId = request.AccountId;

            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginGate, accountId.GetHashCode()))
            {
                PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
                Player playr = playerComponent.Get(accountId);
                if (playr == null)
                {
                    reply();
                    return;
                }
                playerComponent.Remove(accountId);

                scene.GetComponent<GateSessionKeyComponent>().Remove(accountId);
                Session gateSession = Game.EventSystem.Get(playr.SessionInstanceId) as Session;
                if (gateSession != null && !gateSession.IsDisposed)
                {

                    if (gateSession.GetComponent<SessionPlayerComponent>() != null)
                    {
                        gateSession.GetComponent<SessionPlayerComponent>().isLoginAgain = true;
                    }

                    gateSession.Send(new A2C_Disconnect()
                    {
                        Error = ErrorCode.ERR_OtherAccountLoginError
                    });
                    gateSession?.Disconnect().Coroutine();
                }
                playr.SessionInstanceId = 0;
                playr.AddComponent<PlayerOfflineOutTimeComponent>();
            }
            reply();
        }
    }
}
