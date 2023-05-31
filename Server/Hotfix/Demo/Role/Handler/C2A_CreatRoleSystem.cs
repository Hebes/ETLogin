using System;

namespace ET
{
    public class C2A_CreatRoleSystem : AMRpcHandler<C2A_CreatRole, A2C_CreatRole>
    {
        protected override async ETTask Run(Session session, C2A_CreatRole request, A2C_CreatRole response, Action reply)
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

            //TODO 游戏角色名称的敏感词判定、长度的判定
            if (string.IsNullOrEmpty(request.Name))
            {
                response.Error = ErrorCode.ERR_RoleNameIsNullError;
                reply();
                return;
            }

            //防止频繁创建游戏角色
            using (session.AddComponent<SessionLockingComponent>())
            {
                using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.CreatRoleLock, request.AccountId))
                {
                    var roleInfos = await DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).
                        Query<RoleInfo>(d => d.Name == request.Name 
                        && d.ServerId == request.ServerId);

                    //有角色
                    if (roleInfos == null || roleInfos.Count > 0)
                    {
                        response.Error = ErrorCode.ERR_RoleNameSameError;
                        reply();
                        return;
                    }

                    //没有角色的话,就创建出来一个
                    RoleInfo newRoleInfo = session.AddChildWithId<RoleInfo>(IdGenerater.Instance.GenerateUnitId(request.ServerId));
                    newRoleInfo.Name = request.Name;
                    newRoleInfo.State = (int)RoleInfoState.Normal;
                    newRoleInfo.ServerId = request.ServerId;
                    newRoleInfo.AccountId = request.AccountId;
                    newRoleInfo.CreatTiemr = TimeHelper.ServerNow();
                    newRoleInfo.LastLoginTime = 0;

                    await DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).Save<RoleInfo>(newRoleInfo);

                    response.RloeInfo = newRoleInfo.ToMessage();
                    reply();

                    newRoleInfo?.Dispose();//销毁
                }
            }
        }
    }
}
