using System;

namespace ET
{
    public class G2M_RequestExitGameHandler : AMActorLocationRpcHandler<Unit, G2M_RequestExitGame, M2G_RequestExitGame>
    {
        protected override async ETTask Run(Unit unit, G2M_RequestExitGame request, M2G_RequestExitGame response, Action reply)
        {
            //TODD 保存玩家数据到数据库，执行相关下线操作
            Log.Debug("开始下线保存玩家数据");

            reply();

            //正式释放Unit
            await unit.RemoveLocation();//通知定位游戏服务器将unit位置移除
            UnitComponent unitComponent = unit.DomainScene().GetComponent<UnitComponent>();
            unitComponent.Remove(unit.Id);

            await ETTask.CompletedTask;
        }
    }
}