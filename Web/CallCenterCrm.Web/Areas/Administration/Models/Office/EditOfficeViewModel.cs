namespace CallCenterCrm.Web.Areas.Administration.Models.Office
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class EditOfficeViewModel : OfficeViewModel
    {
        [Required]
        [Display(Name = "Manager")]
        public string ManagerId { get; set; }

        public IEnumerable<SelectListItem> Managers { get; set; }
    }
}
