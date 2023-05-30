using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    /// <summary> 登录中心服的组件 </summary>
    public  class LoginInfoRecordComponent:Entity,IAwake ,IDestroy
    {
        public Dictionary<long, int> AccountLoginInfoDic { get; set; } = new Dictionary<long, int>();
    }
}
