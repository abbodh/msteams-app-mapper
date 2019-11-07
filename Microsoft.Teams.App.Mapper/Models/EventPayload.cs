using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Teams.App.Mapper.Models
{
    public class EventPayload
    {
        [JsonProperty("eventname")]
        public string EventName { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("url")]
        public string ServiceUrl { get; set; }

        [JsonProperty("response")]
        public string Response { get; set; }
    }
}
