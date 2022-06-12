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

        [Required(ErrorMessage = "Project Title is required")]
        public string ProjectTitle { get; set; }

        [Required(ErrorMessage = "Project Description is required")]
        public string ProjectDescription { get; set; }

        [Required(ErrorMessage = "Project Domain is required")]
        public string Domain { get; set; }

        [Required(ErrorMessage = "Organization Name is required")]
        public string OrganizationName { get; set; }

        [Required(ErrorMessage = "System Actors are required")]
        public string SystemActors { get; set; }

        public virtual IList<Meeting> Meetings { get; set; }

        public virtual IList<UserStory> UserStories { get; set; }

        public virtual User User { get; set; }
    }
}
