using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Teams.App.Mapper.Models
{
    public class Event
    {
        [JsonProperty("name")]
        public string Name
        {
            get; set;
        }

        [JsonProperty("actionType")]
        public ActionType ActionType
        {
            get; set;
        }
    }
}
