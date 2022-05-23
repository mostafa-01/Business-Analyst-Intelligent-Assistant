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

        [Required(ErrorMessage = "Meeting Title is required")]
        public string MeetingTitle { get; set; }

        public string MeetingDescription { get; set; }

        [Required(ErrorMessage = "You must select Meeting Personnel")]
        public string MeetingPersonnel { get; set; }

        [Required(ErrorMessage = "You must upload the Meeting Recording")]
        public string AudioReference { get; set; }

        public string ASR_Text { get; set; }

        public virtual IList<sentsTimestamp> sentsTimestamp { get; set; }

        public virtual Project Project { get; set; }

        public virtual IList<Service> Services { get; set; }

        public virtual IList<UserStory> UserStories { get; set; }
    }
}
