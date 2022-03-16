using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BAIA.Models
{
    public class ServiceDetail
    {
        public int ServiceDetailID { get; set; }

        public string ServiceDetailString { get; set; }

        public bool ServiceDetailVerified { get; set; }

        public virtual Service Service { get; set; }
    }
}
