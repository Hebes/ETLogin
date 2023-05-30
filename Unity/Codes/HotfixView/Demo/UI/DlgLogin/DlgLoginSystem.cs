using System;
using UnityEngine.UI;

namespace ET
{
    public static class DlgLoginSystem
    {

        public static void RegisterUIEvent(this DlgLogin self)
        {
            self.View.E_LoginButton.AddListenerAsync(() => { return self.OnLoginClickHandler(); });
        }

        public static void ShowWindow(this DlgLogin self, Entity contextData = null)
        {

        }

        public static async ETTask OnLoginClickHandler(this DlgLogin self)
        {
            try
            {
                //登录
                int errorCode = await LoginHelper.Login(
                               self.DomainScene(),
                               ConstValue.LoginAddress,
                               self.View.E_AccountInputField.GetComponent<InputField>().text,
                               self.View.E_PasswordInputField.GetComponent<InputField>().text);

                if (errorCode != ErrorCode.ERR_Success)
                {
                    Log.Error(errorCode.ToString());
                    return;
                }
                Log.Debug("登录成功");

                //获取服务器列表
                errorCode = await LoginHelper.GetServerInfos(self.ZoneScene());
                if (errorCode != ErrorCode.ERR_Success)
                {
                    Log.Error(errorCode.ToString());
                    return;
                }

                //UI界面
                //TODO 显示登录之后的页面逻辑比如登录后的显示页面
                self.DomainScene().GetComponent<UIComponent>().HideWindow(WindowID.WindowID_Login);
                //显示服务器的界面
                self.DomainScene().GetComponent<UIComponent>().ShowWindow(WindowID.WindowID_Server);
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }
        }

        public static void HideWindow(this DlgLogin self)
        {

        }

    }
}
