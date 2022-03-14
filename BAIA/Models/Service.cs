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

        [Required]
        public string ServiceTitle { get; set; }

        [Required]
        public List<Tuple<string, bool>> ServiceDetails { get; set; }

        [DefaultValue(false)]
        public bool ServiceVerified { get; set; }


        public virtual Project Project { get; set; }
    }
}
