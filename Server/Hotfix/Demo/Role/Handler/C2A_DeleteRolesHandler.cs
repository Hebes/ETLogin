using System;

namespace ET
{
    /// <summary> 客户端法网服务端的消息的处理 </summary>
    public class C2A_DeleteRolesHandler : AMRpcHandler<C2A_DeleteRoles, A2C_DeleteRoles>
    {
        protected override async ETTask Run(Session session, C2A_DeleteRoles request, A2C_DeleteRoles response, Action reply)
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
                //创建和删除都是异步的过程,所以都用CreatRoleLock这个锁住.保证逻辑顺序的正确性
                using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.CreatRoleLock, request.AccountId))
                {
                    var roleInfos = await DBManagerComponent.Instance.GetZoneDB(request.ServerId).
                        Query<RoleInfo>(d => d.Id == request.RoleInfoId && d.ServerId == request.ServerId);

                    //有角色了
                    if (roleInfos == null && roleInfos.Count > 0)
                    {
                        response.Error = ErrorCode.ERR_RoleNotExistError;
                        reply();
                        return;
                    }

                    var roleInfo = roleInfos[0];
                    session.AddChild(roleInfo);
                    roleInfo.State = (int)RoleInfoState.Freeze;//不能直接删除,用冻结状态
                    await DBManagerComponent.Instance.GetZoneDB(request.ServerId).Save<RoleInfo>(roleInfo);
                    response.DeleteRloeInfoId = roleInfo.Id;
                    roleInfo?.Dispose();//销毁
                    reply();
                }
            }
        }
    }
}
