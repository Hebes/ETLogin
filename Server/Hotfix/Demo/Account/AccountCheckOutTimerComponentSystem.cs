namespace ET
{

    [Timer(TimerType.AccountSessionCheckOutTime)]
    public class AccountSessionCheckOutTimer : ATimer<AccountCheckOutTimerComponent>
    {
        public override void Run(AccountCheckOutTimerComponent self)
        {
            try
            {
                self.DelectSession();
            }
            catch (System.Exception e)
            {
                Log.Debug(e.ToString());
            }
        }
    }

    public class AccountCheckOutTimerComponentAwakeSystem : AwakeSystem<AccountCheckOutTimerComponent, long>
    {
        public override void Awake(AccountCheckOutTimerComponent self, long a)
        {
            self.AccountId = a;
            TimerComponent.Instance.Remove(ref self.Timer);
            self.Timer = TimerComponent.Instance.NewOnceTimer(TimeHelper.ServerNow() + 600000, TimerType.AccountSessionCheckOutTime, self);
        }
    }
    public class AccountCheckOutTimerComponentDestroySystem : DestroySystem<AccountCheckOutTimerComponent>
    {
        public override void Destroy(AccountCheckOutTimerComponent self)
        {
            self.AccountId = 0;
            TimerComponent.Instance.Remove(ref self.Timer);
        }
    }

    [FriendClass(typeof(AccountCheckOutTimerComponent))]
    public static class AccountCheckOutTimerComponentSystem
    {
        public static void DelectSession(this AccountCheckOutTimerComponent self)
        {
            Session session = self.GetParent<Session>();
            long sessionInstanceId = session.DomainScene().GetComponent<AccountSessionsComponent>().Get(self.AccountId);
            if (session.InstanceId == sessionInstanceId)
                session.DomainScene().GetComponent<AccountSessionsComponent>().Remove(self.AccountId);
            session?.Send(new A2C_Disconnect() 
            {
                Error =1,
            });
            session?.Disconnect().Coroutine();
        }
    }
}
