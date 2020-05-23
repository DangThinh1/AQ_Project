using System.Collections.Generic;

namespace Identity.Core.Models.Users
{
    public class UserSearchModel
    {
        public List<int> RoleIds { get; set; }
        public string SearchString { get; set; }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string SortColumn { get; set; }
        public string SortType { get; set; }
    }
}
