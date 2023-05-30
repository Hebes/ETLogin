using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class AccountInfoComponentAwakeSystem : AwakeSystem<AccountInofComponent>
    {
        public override void Awake(AccountInofComponent self)
        {
            self.Token  =string.Empty;
            self.AccountId = 0;
        }
    }

    public class AccountInfoComponentDestroySystem : DestroySystem<AccountInofComponent>
    {
        public override void Destroy(AccountInofComponent self)
        {

        }
    }

    public static  class AccountInfoComponentSystem
    {
    }
}
