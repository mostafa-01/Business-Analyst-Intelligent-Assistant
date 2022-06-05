using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BAIA.Models
{
    public class UserStoryResponseModel
    {
        public List<string> stories { get; set; }

        public List<string> preconditions { get; set; }

        public List<string> acceptanceCriteria { get; set; }

    }
}
