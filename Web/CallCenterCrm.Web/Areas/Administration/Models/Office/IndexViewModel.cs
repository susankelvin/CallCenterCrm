namespace CallCenterCrm.Web.Areas.Administration.Models.Office
{
    using System.Collections.Generic;

    public class IndexViewModel
    {
        public int ActivePage { get; set; }

        public int  PageCount { get; set; }

        public IEnumerable<IndexOfficeViewModel> Offices { get; set; }
    }
}
