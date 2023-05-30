using System;

namespace ET
{
    public class C2A_GetRolesHandler : AMRpcHandler<C2A_GetRoles, A2C_GetRoles>
    {
        protected override async ETTask Run(Session session, C2A_GetRoles request, A2C_GetRoles response, Action reply)
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
                //协程锁一样的CreatRoleLock  保证不会进行角色查询
                using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.CreatRoleLock, request.AccountId))
                {
                    var roleInfos = await DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).
                        Query<RoleInfo>(d => d.AccountId == request.AccountId //账号ID
                        && d.State == (int)RoleInfoState.Normal//角色的不是删除或者冻结的
                        && d.ServerId == request.ServerId);//游戏的区服

                    //没有角色
                    if (roleInfos == null || roleInfos.Count == 0) 
                    {
                        reply();
                        return;
                    }

                    foreach (var roleInfo in roleInfos)
                    {
                        response.RloeInfo.Add(roleInfo.ToMessage());
                        roleInfo?.Dispose();//释放,可以不释放,这个不释放系统会自动释放
                    }

                    roleInfos.Clear();
                    reply();
                }
            }
        }
    }
}
