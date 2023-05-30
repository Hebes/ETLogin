namespace ET
{
    /// <summary> 帮助类 </summary>
    public static class DisconnectHelper
    {
        /// <summary> 防止消息没有发送完成就关闭了链接 </summary>
        public static async ETTask Disconnect(this Session self )
        {
            if (self == null || self.IsDisposed) return;

            long instanceId=self.InstanceId;

            await TimerComponent.Instance.WaitAsync(1000);

            if (self.InstanceId != instanceId) return;

            self.Dispose();
        }
    }
}
