using System;

namespace ET
{
    [FriendClass(typeof(DlgServer))]
    public static class DlgServerSystem
    {

        public static void RegisterUIEvent(this DlgServer self)
        {
            //添加的测试监听
            self.View.E_EnterMapButton.AddListener(self.OnEnterMapCilckHandler);
            self.OnGetServerInfo();
        }

        public static void ShowWindow(this DlgServer self, Entity contextData = null)
        {

        }

        /// <summary> 获取服务器信息 </summary>
        public static void OnGetServerInfo(this DlgServer self)
        {
            ServerInfosComponent serverInfosComponent = self.ZoneScene().GetComponent<ServerInfosComponent>();
            foreach (var item in serverInfosComponent.serverInfoList)
            {
                Log.Debug("服务器有: " + item.ServerName);
            }

            self.View.E_EnterMap1Button.AddListener(() =>
            {
                serverInfosComponent.CurrentServerId = 1;
                Log.Debug("当前服务器ID是: " + serverInfosComponent.CurrentServerId);
            });
            self.View.E_EnterMap2Button.AddListener(() =>
            {
                serverInfosComponent.CurrentServerId = 2;
                Log.Debug("当前服务器ID是: " + serverInfosComponent.CurrentServerId);
            });
        }

        /// <summary> 点击按钮事件 </summary> 
        public static async void OnEnterMapCilckHandler(this DlgServer self)
        {
            if (self.ZoneScene().GetComponent<ServerInfosComponent>().CurrentServerId == 0)
            {
                Log.Error("请选择服务器");
                return;
            }

            try
            {
                //获取角色列表
                int errorCode = await LoginHelper.GetRoles(self.ZoneScene());
                if (errorCode != ErrorCode.ERR_Success)
                {
                    Log.Error(errorCode.ToString());
                    return;
                }

                //跳转场景
                //关闭选择服务器界面
                self.DomainScene().GetComponent<UIComponent>().HideWindow(WindowID.WindowID_Server);
                //打开选择角色界面
                self.DomainScene().GetComponent<UIComponent>().ShowWindow(WindowID.WindowID_Role);
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }
        }

        public static void OnE_EnterMap1Button(this DlgServer self)
        {

        }
        public static void OnE_EnterMap2Button(this DlgServer self)
        {

        }
    }
}
