using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Teams.App.Mapper.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Teams.App.Mapper.Helper
{
    public class Processor : IProcessor
    {
        private static readonly MemoryStorage memoryStore = new MemoryStorage();
        private HttpClient httpClient = new HttpClient();

        public async Task<EventPayload> NotifySubscribersAsync(EventPayload eventPayload)
        {
            eventPayload.Response = await httpClient.GetStringAsync(eventPayload.ServiceUrl);

            // read memory storage

            return eventPayload;
        }

        public async Task<bool> WriteToStoreAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            try
            {
                var events = new Dictionary<string, object>();
                {
                    events.Add(((JObject)stepContext.Context.Activity.Value).GetValue("eventname").ToString(), stepContext.Context.Activity);
                };

                await memoryStore.WriteAsync(events, cancellationToken);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
