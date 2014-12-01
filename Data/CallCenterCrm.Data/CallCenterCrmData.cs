namespace CallCenterCrm.Data
{
    using System;
    using System.Collections.Generic;
    using CallCenterCrm.Data.Common.Repository;
    using CallCenterCrm.Data.Models;

    public class CallCenterCrmData : ICallCenterCrmData
    {
        private readonly ApplicationDbContext context;

        private readonly Dictionary<Type, object> repositories;

        public CallCenterCrmData(ApplicationDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context", "Context cannot be null");
            }

            this.context = context;
            this.repositories = new Dictionary<Type, object>();
        }

        public ApplicationDbContext Context
        {
            get
            {
                return this.context;
            }
        }

        public IDeletableEntityRepository<ApplicationUser> Users
        {
            get
            {
                return this.GetDeletableEntityRepository<ApplicationUser>();
            }
        }

        public IDeletableEntityRepository<Office> Offices
        {
            get
            {
                return this.GetDeletableEntityRepository<Office>();
            }
        }

        public IDeletableEntityRepository<Campaign> Campaigns
        {
            get
            {
                return this.GetDeletableEntityRepository<Campaign>();
            }
        }

        public IDeletableEntityRepository<CallResult> CallResults
        {
            get
            {
                return this.GetDeletableEntityRepository<CallResult>();
            }
        }

        public IDeletableEntityRepository<Status> Statuses
        {
            get
            {
                return this.GetDeletableEntityRepository<Status>();
            }
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.context != null)
                {
                    this.context.Dispose();
                }
            }
        }

        private IDeletableEntityRepository<T> GetDeletableEntityRepository<T>() where T : class, IDeletableEntity
        {
            if (!this.repositories.ContainsKey(typeof(T)))
            {
                var type = typeof(DeletableEntityRepository<T>);
                this.repositories.Add(typeof(T), Activator.CreateInstance(type, this.context));
            }

            return (IDeletableEntityRepository<T>)this.repositories[typeof(T)];
        }
    }
}
