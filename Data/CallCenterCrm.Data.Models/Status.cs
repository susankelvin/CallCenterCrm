namespace CallCenterCrm.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Status
    {
        public int StatusId { get; set; }

        public string Description { get; set; }

        public ICollection<CallResult> CallResults { get; set; }

        public Status()
        {
            this.CallResults = new HashSet<CallResult>();
        }
    }
}
