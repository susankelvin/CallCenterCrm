namespace CallCenterCrm.Web.Areas.Administration.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using CallCenterCrm.Data.Models;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Username")]
        public string UserName { get; set; }

        public string Email { get; set; }

        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        public int OfficeId { get; set; }

        [Display(Name="Office")]
        public IEnumerable<SelectListItem> Offices { get; set; }

        public string RoleId { get; set; }

        [Display(Name = "Role")]
        public IEnumerable<SelectListItem> Roles { get; set; }
    }
}
