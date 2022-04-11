using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BAIA.Models
{
    public class Meeting
    {
        public int MeetingID { get; set; }

        [Required]
        public string MeetingTitle { get; set; }

        [Required]
        public string MeetingDescription { get; set; }

        [Required]
        public string MeetingPersonnel { get; set; }

        [Required]
        public string AudioReference { get; set; }

        [Required]
        public string ASR_Text { get; set; }

        public virtual Project Project { get; set; }

        public virtual IList<Service> Services { get; set; }

        public virtual IList<UserStory> UserStories { get; set; }
    }
}
