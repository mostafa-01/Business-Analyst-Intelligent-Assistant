using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BAIA.Models
{
    [Serializable]
    public class UserStoryResponseModel
    {
        public List<string> stories { get; set; }

        public List<string> preconditions { get; set; }

        public List<string> acceptanceCriteria { get; set; }

    }
}
