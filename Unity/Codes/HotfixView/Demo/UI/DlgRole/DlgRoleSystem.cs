using System;
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
            self.View.E_StartGameButton.AddListener(self.OnStartGameButton);
        }

        public static void ShowWindow(this DlgRole self, Entity contextData = null)
        {
            self.OnClearChild();
            self.OnShowChild();
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
                self.OnShowChild();
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

                //删除角色
                for (int i = 0; i < self.View.EContentImage.transform.childCount; i++)
                {

                    bool isEqual = self.View.EContentImage.transform.GetChild(i).Find("Name").GetComponent<Text>().text == self.roleInfo.Name;
                    if (isEqual)
                        GameObject.Destroy(self.View.EContentImage.transform.GetChild(i).gameObject);
                }
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
        public static void OnStartGameButton(this DlgRole self)
        {

        }
    }
}
