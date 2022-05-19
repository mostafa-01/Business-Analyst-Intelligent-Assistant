using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BAIA.Models
{
    public class ServiceDetail
    {
        public int ServiceDetailID { get; set; }

        [Required]
        public string ServiceDetailString { get; set; }
        public int Timestamp { get; set; }

        [DefaultValue(false)]
        public bool ServiceDetailVerified { get; set; }

        public virtual Service Service { get; set; }
    }
}
