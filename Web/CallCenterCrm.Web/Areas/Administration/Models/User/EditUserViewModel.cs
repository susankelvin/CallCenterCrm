namespace CallCenterCrm.Web.Areas.Administration.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;

    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Username")]
        public string UserName { get; set; }

        public string Email { get; set; }

        [Required]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Office")]
        public int OfficeId { get; set; }

        [Display(Name="Office")]
        public IEnumerable<SelectListItem> Offices { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string RoleId { get; set; }

        [Display(Name = "Roles")]
        public IEnumerable<SelectListItem> Roles { get; set; }
    }
}
