namespace ET
{
    /// <summary> 帮助类 </summary>
    public static class DisconnectHelper
    {
        /// <summary> 防止消息没有发送完成就关闭了链接 </summary>
        public static async ETTask Disconnect(this Session self)
        {
            if (self == null || self.IsDisposed) return;

            long instanceId = self.InstanceId;

            await TimerComponent.Instance.WaitAsync(1000);

            if (self.InstanceId != instanceId) return;

            self.Dispose();
        }

        /// <summary> 玩家下线 </summary>
        /// <param name="player"></param>
        /// <param name="isExcption">是否是正常下线</param>
        /// <returns></returns>
        public static async ETTask KickPlayer(Player player, bool isExcption = false)
        {
            if (player == null || player.IsDisposed) return;

            long instanceId = player.InstanceId;
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginGate, player.AccountId.GetHashCode()))
            {
                if (player.IsDisposed || instanceId != player.InstanceId) return;

                if (isExcption == false)
                {

                    switch (player.PlayerState)
                    {
                        case PlayerState.Disconnect:
                            break;
                        case PlayerState.Gate:
                            break;
                        case PlayerState.Game:
                            //通知游戏逻辑服下线Unit角色逻辑，并将数据存入数据库
                            var m2GRequestExitGame = (M2G_RequestExitGame)await MessageHelper.CallLocationActor(player.UnitId, new G2M_RequestExitGame());

                            //通知移除账号角色登录信息
                            long LoginCenterConfigSceneId = StartSceneConfigCategory.Instance.loginCenterConfig.InstanceId;
                            var L2G_RemoveLoginRecord = (L2G_RemoveLoginRecord)await MessageHelper.CallActor(LoginCenterConfigSceneId, new G2L_RemoveLoginRecord()
                            {
                                AccountId = player.AccountId,
                                ServerId = player.DomainZone()
                            });
                            break;
                        default:
                            break;
                    }
                }

                player.PlayerState = PlayerState.Disconnect;
                player.DomainScene().GetComponent<PlayerComponent>()?.Remove(player.AccountId);
                player?.Dispose();
                await TimerComponent.Instance.WaitAsync(300);
            }
        }
    }
}
