namespace CallCenterCrm.Web.Infrastructure
{
    using AutoMapper;
    using CallCenterCrm.Data.Models;
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
        }
    }
}
