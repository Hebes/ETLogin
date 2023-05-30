namespace ET
{
    [FriendClass(typeof(ET.TokenComponent))]
    public static class TokenComponentSystem
    {
        /// <summary> 令牌添加 </summary>
        public static void Add(this TokenComponent self, long key, string token)
        {
            self.TokenDictionary.Add(key, token);
            self.TimeOutRemoveKey(key, token).Coroutine();//启动一个协程
        }
        /// <summary> 令牌获取 </summary>
        public static string Get(this TokenComponent self, long key)
        {
            string value = null;
            self.TokenDictionary.TryGetValue(key, out value);
            foreach (var item in self.TokenDictionary.Values)
                Log.Debug("token字典中有: " + item);
            return value;
        }
        /// <summary> 令牌移除 </summary>
        public static void Remove(this TokenComponent self, long key)
        {
            if (self.TokenDictionary.ContainsKey(key))
                self.TokenDictionary.Remove(key);
        }
        /// <summary> 令牌过期移除 </summary>
        private static async ETTask TimeOutRemoveKey(this TokenComponent self, long key, string tokenKey)
        {
            //等待10分钟
            await TimerComponent.Instance.WaitAsync(600000);
            //获取令牌
            string onlineToken = self.Get(key);
            //令牌不能为空并且和以前的令牌保持一致
            if (!string.IsNullOrEmpty(onlineToken) && onlineToken == tokenKey)
                self.Remove(key);
        }
    }
}
