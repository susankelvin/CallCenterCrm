namespace CallCenterCrm.Data
{
    using CallCenterCrm.Data.Common.Repository;
    using CallCenterCrm.Data.Models;

    public interface ICallCenterCrmData
    {
        ApplicationDbContext Context { get; }

        IDeletableEntityRepository<ApplicationUser> Users { get; }

        IDeletableEntityRepository<Office> Offices { get; }

        IDeletableEntityRepository<Campaign> Campaigns { get; }

        IDeletableEntityRepository<CallResult> CallResults { get; }

        IDeletableEntityRepository<Status> Statuses { get; }

        int SaveChanges();
    }
}