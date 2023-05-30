using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    管理Session 用于顶号操作

-----------------------*/

namespace ET
{
    [ComponentOf(typeof(Scene))]
    public class AccountSessionsComponent :Entity,IAwake ,IDestroy
    {
        public Dictionary<long, long> AccountSessionDic { get; set; } = new Dictionary<long, long>(); 
    }
}
