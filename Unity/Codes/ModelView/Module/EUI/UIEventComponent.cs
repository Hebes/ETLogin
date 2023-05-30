using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    public class UIEventComponent : Entity,IAwake,IDestroy
    {
        public static UIEventComponent Instance { get; set; }
        public readonly Dictionary<WindowID, IAUIEventHandler> UIEventHandlers = new Dictionary<WindowID, IAUIEventHandler>();
        //IsClicked这个没有的话就添加
        public bool IsClicked { get; set; }
    }
}