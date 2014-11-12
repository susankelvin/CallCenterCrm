namespace CallCenterCrm.Web.Areas.Administration.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web;
    using CallCenterCrm.Data.Models;

    public class UserViewModel
    {
        public static Expression<Func<ApplicationUser, UserViewModel>> FromUser
        {
            get
            {
                return u => new UserViewModel()
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Office = u.Office.Name,
                    Role = u.Roles.FirstOrDefault().RoleId
                };
            }
        }

        public string Id { get; set; }

        [Display(Name="Username")]
        public string UserName { get; set; }

        public string Email { get; set; }

        [Display(Name="Phone number")]
        public string PhoneNumber { get; set; }

        public string Office { get; set; }

        public string Role { get; set; }
    }
}