namespace CallCenterCrm.Web.Areas.Operator.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using AutoMapper.QueryableExtensions;
    using CallCenterCrm.Data;
    using CallCenterCrm.Data.Models;
    using CallCenterCrm.Web.Areas.Operator.Models.ActiveCampaigns;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using Microsoft.AspNet.Identity;

    public class ActiveCampaignsController : Controller
    {
        private readonly ICallCenterCrmData data;

        public ActiveCampaignsController(ICallCenterCrmData data)
        {
            this.data = data;
        }

        // GET: Operator/ActiveCampaigns
        public ActionResult Index()
        {
            return View();
        }

        // AJAX update
        [HttpPost]
        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            string operatorId = this.User.Identity.GetUserId();
            ApplicationUser user = this.data.Users.Find(operatorId);
            int officeId = user.OfficeId ?? 0;
            var campaigns = this.data.Campaigns.All()
                                  .Where(c => c.OfficeId == officeId)
                                  .Project()
                                  .To<IndexActiveCampaignsModel>()
                                  .ToList();
            return Json(campaigns.ToDataSourceResult(request));
        } 
    }
}
