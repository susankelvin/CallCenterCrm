namespace CallCenterCrm.Web.Infrastructure
{
    using AutoMapper;
    using CallCenterCrm.Data.Models;
    using CallCenterCrm.Web.Areas.Administration.Models.Office;
    using CallCenterCrm.Web.Areas.Management.Models.Campaign;
    using CallCenterCrm.Web.Areas.Operator.Models.ActiveCampaigns;
    using CallCenterCrm.Web.Areas.Operator.Models.CallResult;

    public static class AutoMapperConfig
    {
        public static void Execute()
        {
            // CallResult
            Mapper.CreateMap(typeof(CallResult), typeof(NewCallResultModel));
            Mapper.CreateMap(typeof(NewCallResultModel), typeof(CallResult));
            Mapper.CreateMap(typeof(IndexCallResultModel), typeof(CallResult));
            Mapper.CreateMap(typeof(CallResult), typeof(IndexCallResultModel));

            // Active campaigns
            Mapper.CreateMap(typeof(Campaign), typeof(IndexActiveCampaignsModel));
            Mapper.CreateMap(typeof(IndexActiveCampaignsModel), typeof(Campaign));

            // Offices
            Mapper.CreateMap(typeof(Office), typeof(IndexOfficeViewModel));
            Mapper.CreateMap(typeof(IndexOfficeViewModel), typeof(Office));
            Mapper.CreateMap(typeof(Office), typeof(NewOfficeViewModel));
            Mapper.CreateMap(typeof(NewOfficeViewModel), typeof(Office));
            Mapper.CreateMap(typeof(Office), typeof(EditOfficeViewModel));
            Mapper.CreateMap(typeof(EditOfficeViewModel), typeof(Office));

            // Campaigns management
            Mapper.CreateMap<Campaign, CallCenterCrm.Web.Areas.Management.Models.Campaign.IndexViewModel>();
            Mapper.CreateMap<CallCenterCrm.Web.Areas.Management.Models.Campaign.IndexViewModel, Campaign>();
            Mapper.CreateMap<NewCampaignViewModel, Campaign>();
        }
    }
}
