namespace CallCenterCrm.Web.Areas.Administration.Models.Office
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class IndexOfficeViewModel : OfficeViewModel
    {
        [Required]
        [Display(Name = "Manager")]
        public string ManagerUserName { get; set; }
    }
}