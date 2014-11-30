namespace CallCenterCrm.Web.Areas.Administration.Models
{
    using System.Collections.Generic;

    public class IndexViewModel
    {
        public int ActivePage { get; set; }

        public int PageCount { get; set; }

        public IEnumerable<UserViewModel> Users { get; set; }
    }
}
