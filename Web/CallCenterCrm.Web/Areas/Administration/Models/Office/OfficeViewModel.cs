namespace CallCenterCrm.Web.Areas.Administration.Models.Office
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class OfficeViewModel
    {
        [Required]
        [HiddenInput(DisplayValue=false)]
        public int OfficeId { get; set; }

        [Required]
        [Display(Name = "Office name")]
        public string Name { get; set; }

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