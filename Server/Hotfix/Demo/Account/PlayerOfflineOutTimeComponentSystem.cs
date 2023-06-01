using System;
using System.Threading;

namespace ET
{
    [Timer(TimerType.PlayerOfflineOutTime)]
    public class PlayerOfflinwOutTime : ATimer<PlayerOfflineOutTimeComponent>
    {
        public override void Run(PlayerOfflineOutTimeComponent self)
        {
            try
            {
                self.KickPlayer();
            }
            catch (Exception e )
            {
                Log.Error($"玩家超时错误: {self.Id}\n{e}");
            }
        }
    }

    public class GateUnitDeleteComponentDestroySystem : DestroySystem<PlayerOfflineOutTimeComponent>
    {
        public override void Destroy(PlayerOfflineOutTimeComponent self)
        {
            //取消定时器的任务
            TimerComponent.Instance.Remove(ref self.Timer);
        }
    }

    public class GateUnitDeleteComponentAwakeSystem : AwakeSystem<PlayerOfflineOutTimeComponent>
    {
        public override void Awake(PlayerOfflineOutTimeComponent self)
        {
            //在TimeHelper.ServerNow() + 10000后启动定时器任务就是上方的PlayerOfflinwOutTime 10秒之后启动
            self.Timer = TimerComponent.Instance.NewOnceTimer(TimeHelper.ServerNow() + 10000, TimerType.PlayerOfflineOutTime, self);
        }
    }

    public static  class PlayerOfflineOutTimeComponentSystem
    {
        public static void KickPlayer(this PlayerOfflineOutTimeComponent self)
        {
            DisconnectHelper.KickPlayer(self.GetParent<Player>()).Coroutine();
        }
    }
}
