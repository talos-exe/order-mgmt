using X.PagedList;

namespace OrderMgmtRevision.Models
{
    public class UserManagementViewModel
    {
        public IPagedList<UserViewModel> Users {  get; set; }
        public IPagedList<UserLog> UserLogs { get; set; }
    }
}
