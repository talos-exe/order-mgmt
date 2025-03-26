namespace OrderMgmtRevision.Models
{
    public class UserLog
    {
       public int Id { get; set; }
        public string UserId {  get; set; }
        public string UserName { get; set; }

        public string Action { get; set; }
        public string IpAddress {  get; set; }
        public DateTime Timestamp { get; set; }

        public User User { get; set; }
    }
}
