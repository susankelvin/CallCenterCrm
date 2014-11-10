namespace CallCenterCrm.Web.Areas.Administration.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class EditOfficeViewModel : NewOfficeViewModel
    {
        [Required]
        public int OfficeId { get; set; }
    }
}