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

        [Required(ErrorMessage = "email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "pass is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "cn is required")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "pn is required")]
        public string PhoneNumber { get; set; }


        public virtual IList<Project> Projects { get; set; }
    }
}
