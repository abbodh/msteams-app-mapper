using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Teams.App.Mapper.Models
{
    public class AdaptiveCardAction
    {
        /// <summary>
        /// Gets or sets Msteams object.
        /// </summary>
        [JsonProperty("msteams")]
        public CardAction Msteams { get; set; }

        [JsonProperty("event")]
        public Event Event { get; set; }
    }
}
