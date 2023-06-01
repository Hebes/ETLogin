namespace ET
{
    public enum PlayerState
    {
        /// <summary> 断开状态 </summary>
        Disconnect,
        /// <summary> 连接Gate网关的装填 </summary>
        Gate,
        /// <summary> 进入游戏的状态 </summary>
        Game,
    }

    public sealed class Player : Entity, IAwake<string>, IAwake<long, long>,IDestroy
    {
        //public string Account { get; set; }

        public long UnitId { get; set; }

        public long AccountId { get; set; }

        public long SessionInstanceId { get; set; }

        public PlayerState PlayerState { get; set; }
    }
}