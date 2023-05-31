namespace ET
{
    [ComponentOf(typeof(Scene))]
    /// <summary> 保存登录令牌等 </summary>
    public class AccountInfoComponent : Entity, IAwake, IDestroy
    {
        public string Token { get; set; }
        public long AccountId { get; set; }

        /// <summary> Realm网关负载均衡服务器令牌 </summary>
        public string RealmKey { get; set; }
        /// <summary> Realm网关负载均衡服务地址 </summary>
        public string RealmAddress { get; set; }
    }
}
