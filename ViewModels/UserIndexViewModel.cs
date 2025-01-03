using TimeTable.Models;

namespace TimeTable.ViewModels
{
    public class UserIndexViewModel
    {
        public List<User> Users { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }
        public int TotalCount { get; set; }
        public string NameFilter { get; set; }
        public Role? RoleFilter { get; set; }
    }
}
