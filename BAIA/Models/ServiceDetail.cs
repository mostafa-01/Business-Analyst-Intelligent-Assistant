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

        [Required(ErrorMessage = "Service Detail is required")]
        public string ServiceDetailString { get; set; }
        public string Timestamp { get; set; }
        public virtual Service Service { get; set; }
    }
}
