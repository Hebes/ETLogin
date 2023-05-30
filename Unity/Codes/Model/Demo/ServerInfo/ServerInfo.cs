namespace ET
{

    public enum ServerStatus
    {
        NorMal = 0,
        Stop = 1,
    }

    public class ServerInfo : Entity, IAwake
    {
        public int Status;
        public string ServerName { get; set; }
    }
}
