using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public enum AccountType
    {
        /// <summary> 普通账号 </summary>
        General = 0,
        /// <summary> 黑名单 </summary>
        BlackList=1,
    }
    /// <summary> 账号结构 </summary>
    public class Account : Entity, IAwake
    {
        /// <summary> 账号名 </summary>
        public string AccountName { get; set; }

        /// <summary> 密码 </summary>
        public string Password { get; set; }

        /// <summary> 账号创建时间 </summary>
        public long CreateTimer { get; set; }

        /// <summary> 账号类型 </summary>
        public int AccountType { get; set; }
    }
}
