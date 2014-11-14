namespace CallCenterCrm.Data.Models.Base
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class BaseModel : IDeletableEntity
    {
        [Required]
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
