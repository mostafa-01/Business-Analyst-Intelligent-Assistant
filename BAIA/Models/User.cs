using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BAIA.Models
{
    public class User
    {
        public int UserID { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }


        public virtual IList<Project> Projects { get; set; }
    }
}
