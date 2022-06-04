using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BAIA.Models
{
    public class UserStory
    {
        public int UserStoryID { get; set; }

        public string UserStoryTitle { get; set; }

        [Required]
        public string UserStoryDescription { get; set; }

        public string Preconditions { get; set; }

        public string AcceptanceCriteria { get; set; }

        public string BusinessLogicFlow { get; set; }

        [DefaultValue(false)]
        public bool Conflict { get; set; }

        [DefaultValue(false)]
        public bool UsetStoryVerified { get; set; }

        public virtual Meeting Meeting { get; set; }
    }
}
