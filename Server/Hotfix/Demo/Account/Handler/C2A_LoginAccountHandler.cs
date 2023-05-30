using System;
using System.Text.RegularExpressions;

namespace ET
{

    [MessageHandler]
    /// <summary> 后端游戏账号登录 </summary>
    public class C2A_LoginAccountHandler : AMRpcHandler<C2A_LoginAccount, A2C_LoginAccount>
    {
        protected override async ETTask Run(Session session, C2A_LoginAccount request, A2C_LoginAccount response, Action reply)
        {
            if (session.DomainScene().SceneType != SceneType.Account)
            {
                Log.Error($"请求的Scene错误,当前的Scene为:{session.DomainScene().SceneType}");
                session.Dispose();
                return;
            }

            session.RemoveComponent<SessionAcceptTimeoutComponent>();

            //防止多次点击登录
            if (session.GetComponent<SessionLockingComponent>() != null)
            {
                response.Error = ErrorCode.ERR_RequestRepeatedlyError;
                reply();//用于发送response这条消息
                session.Disconnect().Coroutine();
                return;
            }

            if (string.IsNullOrEmpty(request.Account) || string.IsNullOrEmpty(request.Password))
            {
                response.Error = ErrorCode.ERR_LoginInfoError;
                reply();//用于发送response这条消息
                session.Disconnect().Coroutine();
                return;
            }

            //验证账号
            if (!Regex.IsMatch(request.Account.Trim(), @"^(?=.*[0-9].*)(?=.*[A-Z].*)(?=.*[a-z].*).{6,15}$"))
            {
                response.Error = ErrorCode.ERR_AccountNamFormError;
                reply();//用于发送response这条消息
                session.Disconnect().Coroutine();
                return;
            }
            //验证密码
            if (!Regex.IsMatch(request.Password.Trim(), @"^[A-Za-z0-9]+$"))//@" ^ (?=.*[0-9].*)(?=.*[A-Z].*)(?=.*[a-z].*).{6,15}$"
            {
                response.Error = ErrorCode.ERR_LoginPasswordError;
                reply();//用于发送response这条消息
                session.Disconnect().Coroutine();
                return;
            }

            //using包裹语句块里面的代码逻辑执行完毕 才会释放SessionLockingComponent 防止多次点击登录
            using (session.AddComponent<SessionLockingComponent>())
            {
                using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginAccount, request.Account.Trim().GetHashCode()))//锁,防止用户同时点击注册
                {
                    Log.Debug("连接数据库的是:  "+ session.DomainZone().ToString());
                    //查询数据库
                    var accountInfoList = await DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).Query<Account>(d =>
                            d.AccountName.Equals(request.Account.Trim()));
                    Account account = null;
                    if (accountInfoList != null && accountInfoList.Count > 0)
                    {
                        account = accountInfoList[0];
                        session.AddChild(account);
                        if (account.AccountType == (int)AccountType.BlackList)
                        {
                            response.Error = ErrorCode.ERR_AccountInBlackListError;
                            reply();//用于发送response这条消息
                            session.Disconnect().Coroutine();
                            account?.Dispose();
                            return;
                        }

                        //判断账号的登录密码
                        if (!account.Password.Equals(request.Password))
                        {
                            response.Error = ErrorCode.ERR_LoginPasswordError;
                            reply();//用于发送response这条消息
                            session.Disconnect().Coroutine();
                            account?.Dispose();
                            return;
                        }

                    }
                    else
                    {
                        account = session.AddChild<Account>();
                        account.AccountName = request.Account.Trim();
                        account.Password = request.Password;
                        account.CreateTimer = TimeHelper.ServerNow();
                        account.AccountType = (int)AccountType.General;

                        //DomainZone() 代表区服 比如1服 2服 3服等
                        //GetZoneDB() 获得1服数据库 2服数据库 等
                        await DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).Save(account);
                    }

                    //账号中心服务器
                    StartSceneConfig startSceneConfig = StartSceneConfigCategory.Instance.GetBySceneName(session.DomainZone(), "LoginCenter");
                    long loginCenterInstanceId = startSceneConfig.InstanceId;
                    L2A_LoginAccountResponse l2A_LoginAccountResponse = (L2A_LoginAccountResponse)await ActorMessageSenderComponent.Instance.Call(loginCenterInstanceId,new A2L_LoginAccountRequest() 
                    {
                        AccountId = account.Id,
                    });
                    if (l2A_LoginAccountResponse.Error!=ErrorCode.ERR_Success)
                    {
                        response.Error = l2A_LoginAccountResponse.Error;
                        reply();
                        session?.Disconnect().Coroutine();
                        account?.Dispose();
                        return;
                    }

                    //顶号操作
                    long accountSessionInstanceId = session.DomainScene().GetComponent<AccountSessionsComponent>().Get(account.Id);
                    Session otherSession = Game.EventSystem.Get(accountSessionInstanceId) as Session;
                    //如果otherSession不为空,则说明有人已经在用这个账号了
                    otherSession?.Send(new A2C_Disconnect()
                    {
                        Error = 0,
                    });
                    otherSession?.Disconnect().Coroutine();
                    session.DomainScene().GetComponent<AccountSessionsComponent>().Add(account.Id, session.InstanceId);
                    session.AddComponent<AccountCheckOutTimerComponent, long>(account.Id);

                    // 记录每个登录的令牌令牌
                    string Token = TimeHelper.ServerNow().ToString() + RandomHelper.RandomNumber(int.MinValue, int.MaxValue).ToString();
                    session.DomainScene().GetComponent<TokenComponent>().Remove(account.Id);
                    session.DomainScene().GetComponent<TokenComponent>().Add(account.Id, Token);

                    //返回给服务器的
                    response.AccountId = account.Id;
                    response.Token = Token;
                    reply();
                    account?.Dispose();
                }
            }
        }
    }
}
