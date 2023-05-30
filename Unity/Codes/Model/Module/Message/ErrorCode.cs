namespace ET
{
    public static partial class ErrorCode
    {
        public const int ERR_Success = 0;

        // 1-11004 是SocketError请看SocketError定义
        //-----------------------------------
        // 100000-109999是Core层的错误

        // 110000以下的错误请看ErrorCore.cs

        // 这里配置逻辑层的错误码
        // 110000 - 200000是抛异常的错误
        // 200001以上不抛异常

        //自定义错误码

        /// <summary> 网络错误 </summary>
        public const int ERR_NetWorkError = 200002;
        /// <summary> 登录信息错误 </summary>
        public const int ERR_LoginInfoError = 200003;
        /// <summary> 账号格式错误 </summary>
        public const int ERR_AccountNamFormError = 200004;
        /// <summary> 登录密码格式错误 </summary>
        public const int ERR_PasswrodFromError = 200005;
        /// <summary> 账号处于黑名单 </summary>
        public const int ERR_AccountInBlackListError = 200006;
        /// <summary> 登录密码错误 </summary>
        public const int ERR_LoginPasswordError = 200007;
        /// <summary> 多次发送错误 </summary>
        public const int ERR_RequestRepeatedlyError = 200007;
        /// <summary> 令牌Token错误 </summary>
        public const int ERR_TokenError = 200008;
        /// <summary> 角色名称是空的 </summary>
        public const int ERR_RoleNameIsNullError = 200009;
        /// <summary> 角色名称重复 </summary>
        public const int ERR_RoleNameSameError = 200010;
        /// <summary> 游戏角色不存在 </summary>
        public const int ERR_RoleNotExistError = 200012;
    }
}