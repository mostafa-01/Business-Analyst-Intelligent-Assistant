using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BAIA.Models
{
    public class Project
    {
        public int ProjectID { get; set; }
        
        [Required]
        public string ProjectTitle { get; set; }

        [Required]
        public string ProjectDescription { get; set; }

        [Required]
        public string Domain { get; set; }

        [Required]
        public string OrganizationName { get; set; }
        
        public virtual IList<Meeting> Meetings { get; set; }

        public virtual User User { get; set; }
    }
}
