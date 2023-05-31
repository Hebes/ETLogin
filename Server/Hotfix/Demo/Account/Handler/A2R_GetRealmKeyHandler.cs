using System;

namespace ET
{
    public class A2R_GetRealmKeyHandler : AMActorRpcHandler<Scene, A2R_GetRealmKey, R2A_GetRealmKey>
    {
        //Scene 代表A2R_GetRealmKey消息交给Scene实体处理
        protected override async ETTask Run(Scene scene, A2R_GetRealmKey request, R2A_GetRealmKey response, Action reply)
        {
            if (scene.SceneType != SceneType.Realm)
            {
                Log.Error($"请求的Sceen错误,当前Scene为:{scene.SceneType}");
                response.Error = ErrorCode.ERR_RequestSceneTypeError;
                reply();
                return;
            }

            //scene的类型是Realm
            string key = TimeHelper.ServerNow().ToString() + RandomHelper.RandInt64().ToString();
            scene.GetComponent<TokenComponent>().Remove(request.AccountId);
            scene.GetComponent<TokenComponent>().Add(request.AccountId, key);
            response.RealmKey = key.ToString();
            reply();
            await ETTask.CompletedTask;
        }
    }
}
