using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    [FriendClass(typeof(DlgRole))]
    public static class DlgRoleSystem
    {

        public static void RegisterUIEvent(this DlgRole self)
        {
            self.View.E_CreatRoleButton.AddListenerAsync(self.OnCreatRoleButton);
            self.View.E_DelectRoleButton.AddListenerAsync(self.OnDelectRoleButton);
            self.View.E_StartGameButton.AddListenerAsync(self.OnStartGameButtonAsync);
            self.View.E_ReRoleButton.AddListenerAsync(self.OnReRoleButtonAsync);
        }

        public static void ShowWindow(this DlgRole self, Entity contextData = null)
        {
            self.OnShowRole();
        }

        /// <summary> 删除子物体 </summary>
        public static void OnClearChild(this DlgRole self)
        {
            for (int i = 0; i < self.View.EContentImage.transform.childCount; i++)
            {
                if (i == 0) continue;
                GameObject go = self.View.EContentImage.transform.GetChild(i).gameObject;
                GameObject.Destroy(go);
            }
        }
        /// <summary> 显示子物体 </summary>
        public static void OnShowChild(this DlgRole self)
        {
            self.View.ERoleNameText.text = string.Empty;
            Log.Debug("已经有的角色个数是: " + self.ZoneScene().GetComponent<RoleInfosComponent>().roleInfosList.Count);
            //显示角色
            foreach (RoleInfo item in self.ZoneScene().GetComponent<RoleInfosComponent>().roleInfosList)
            {
                //克隆
                GameObject roleItemGO = GameObject.Instantiate(self.View.ERoleItemImage.gameObject, self.View.EContentImage.transform);
                roleItemGO.SetActive(true);
                roleItemGO.transform.Find("Name").GetComponent<Text>().text = item.Name;
                roleItemGO.GetComponent<Button>().AddListener(() =>
                {
                    self.ZoneScene().GetComponent<RoleInfosComponent>().CurrentRoleId = item.Id;
                    self.roleInfo = item;
                    self.View.ERoleNameText.text = $"当前选择的角色名称是:\r\n{item.Name}";
                });
            }
        }
        /// <summary> 显示角色,发送请求 </summary>
        private static void OnShowRole(this DlgRole self)
        {
            self.OnClearChild();
            self.OnShowChild();
        }

        /// <summary> 创建角色 </summary>
        public static async ETTask OnCreatRoleButton(this DlgRole self)
        {
            //名字
            string InputName = self.View.EInputFieldNameInputField.text;
            if (string.IsNullOrEmpty(InputName))
            {
                Log.Error("名字是空的");
                return;
            }

            try
            {
                //创建角色
                int errorCode = await LoginHelper.CreatRole(self.ZoneScene(), InputName);
                if (errorCode != ErrorCode.ERR_Success)
                {
                    Log.Debug(errorCode.ToString());
                    return;
                }
                self.OnShowRole();
                self.View.EInputFieldNameInputField.text = string.Empty;
            }
            catch (Exception e)
            {
                Log.Debug(e.ToString());
            }
        }
        /// <summary> 删除角色 </summary>
        public static async ETTask OnDelectRoleButton(this DlgRole self)
        {
            if (self.ZoneScene().GetComponent<RoleInfosComponent>().CurrentRoleId == 0)
            {
                Log.Error("请选择需要删除的角色");
                return;
            }

            try
            {
                int errorCode = await LoginHelper.DeleteRole(self.ZoneScene());
                if (errorCode != ErrorCode.ERR_Success)
                {
                    Log.Debug(errorCode.ToString());
                    return;
                }

                self.OnShowRole();
                //清理不需要的赋值
                self.ZoneScene().GetComponent<RoleInfosComponent>().CurrentRoleId = 0;
                self.roleInfo = null;
                self.View.ERoleNameText.text = string.Empty;
            }
            catch (Exception e)
            {
                Log.Debug(e.ToString());
            }
        }
        /// <summary> 开始游戏 </summary>
        public static async ETTask OnStartGameButtonAsync(this DlgRole self)
        {
            if (self.ZoneScene().GetComponent<RoleInfosComponent>().CurrentRoleId == 0)
            {
                Log.Error("请选择需要删除的角色");
                return;
            }

            try
            {
                //请求网关地址
                int errorCode = await LoginHelper.GetRelamKey(self.ZoneScene());
                if (errorCode != ErrorCode.ERR_Success)
                {
                    Log.Debug(errorCode.ToString());
                    return;
                }

                //登录游戏
                errorCode = await LoginHelper.EnterGame(self.ZoneScene());
                if (errorCode != ErrorCode.ERR_Success)
                {
                    Log.Debug(errorCode.ToString());
                    return;
                }
            }
            catch (Exception e)
            {
                Log.Debug(e.ToString());
            }
        }
        /// <summary> 刷新游戏角色,发送请求 </summary>
        public static async ETTask OnReRoleButtonAsync(this DlgRole self)
        {
            //获取角色列表
            int errorCode = await LoginHelper.GetRoles(self.ZoneScene());
            if (errorCode != ErrorCode.ERR_Success)
            {
                Log.Error(errorCode.ToString());
                return;
            }
            self.OnShowRole();
        }
    }
}
