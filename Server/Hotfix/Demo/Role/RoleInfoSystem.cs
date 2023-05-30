namespace ET
{
    public static  class RoleInfoSystem
    {
        public static void FromMessage(this RoleInfo self,RoleInfoProto roleInfoProto)
        {
            self.Id = roleInfoProto.Id;
            self.Name = roleInfoProto.Name;
            self.State = roleInfoProto.State;
            self.ServerId = roleInfoProto.ServerId;
            self.CreatTiemr = roleInfoProto.CreateTime;
            self.AccountId = roleInfoProto.AccountId;
            self.LastLoginTime=roleInfoProto.LastLoginTime;
        }

        public static RoleInfoProto ToMessage(this RoleInfo self)
        {
            return new RoleInfoProto()
            {
                Id = self.Id,
                Name = self.Name,
                State = self.State,
                ServerId = self.ServerId,
                AccountId = self.AccountId,
                LastLoginTime = self.LastLoginTime,
                CreateTime = self.CreatTiemr,
            };
        }
    }
}
