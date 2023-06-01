namespace ET
{
	[ComponentOf(typeof(Session))]
	public class SessionPlayerComponent : Entity, IAwake, IDestroy
	{
		public long PlayerId;
		public long PlayerInstanceId;

		public long AccountId;
		/// <summary> 是否是再次登录 </summary>
		public bool isLoginAgain = false;
    }
}