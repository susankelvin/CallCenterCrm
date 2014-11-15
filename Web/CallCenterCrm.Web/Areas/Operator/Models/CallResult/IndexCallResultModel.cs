namespace CallCenterCrm.Web.Areas.Operator.Models.CallResult
{
    using CallCenterCrm.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;


    public class IndexCallResultModel
    {
        [Required]
        [HiddenInput(DisplayValue=false)]
        public int CallResultId { get; set; }

        [Required]
        public string CampaignDescription { get; set; }

        [Required]
        public string OperatorUserName { get; set; }

        [Required]
        public Status StatusDescription { get; set; }

        [Required]
        public DateTime CallDate { get; set; }

        [Required]
        public int Duration { get; set; }

        public string Notes { get; set; }

        [Required]
        public string Customer { get; set; }
    }
}