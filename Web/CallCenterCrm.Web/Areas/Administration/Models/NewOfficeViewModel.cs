namespace CallCenterCrm.Web.Areas.Administration.Models
{
    using CallCenterCrm.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    public class NewOfficeViewModel
    {
        [Required]
        [Display(Name = "Office name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Manager")]
        public string ManagerId { get; set; }

        public IEnumerable<SelectListItem> Managers { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
