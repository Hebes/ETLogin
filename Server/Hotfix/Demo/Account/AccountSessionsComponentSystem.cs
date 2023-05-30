namespace ET
{
    public class AccountSessionsComponentAwakeSystem : AwakeSystem<AccountSessionsComponent>
    {
        public override void Awake(AccountSessionsComponent self)
        {
        }
    }
    public class AccountSessionsComponentDestroySystem : DestroySystem<AccountSessionsComponent>
    {
        public override void Destroy(AccountSessionsComponent self)
        {
            self.AccountSessionDic.Clear();
        }
    }

    public static class AccountSessionsComponentSystem
    {
        public static long Get(this AccountSessionsComponent self,long accountId)
        {
            if (!self.AccountSessionDic.TryGetValue(accountId,out long instanceId)) return 0;
            return instanceId;
        }
        public static void  Add(this AccountSessionsComponent self, long accountId, long sessionInstanceId)
        {
            if (self.AccountSessionDic.ContainsKey(accountId))
            {
                self.AccountSessionDic[accountId] = sessionInstanceId;
                return;
            }
            self.AccountSessionDic.Add(accountId, sessionInstanceId);
        }

        public static void Remove(this AccountSessionsComponent self,long accountId)
        {
            if (self.AccountSessionDic.ContainsKey(accountId))
                self.AccountSessionDic.Remove(accountId);
        }
    }
}
