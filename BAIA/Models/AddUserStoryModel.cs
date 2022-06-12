using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BAIA.Models
{
    public class AddUserStoryModel
    {
        public UserStory userStory { get; set; }
        public int projectID { get; set; }
    }
}
