using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BAIA.Models
{
    public class Service
    {
        public int ServiceID { get; set; }
        
        [Required(ErrorMessage = "Service Title is required")]
        public string ServiceTitle { get; set; }

        public virtual IList<ServiceDetail> ServiceDetails { get; set; }

        [DefaultValue(false)]
        public bool ServiceVerified { get; set; }

        public int ConflictServiceID { get; set; }

        public int ConflictMeetingID { get; set; }


        public virtual Meeting Meeting { get; set; }
    }
}
