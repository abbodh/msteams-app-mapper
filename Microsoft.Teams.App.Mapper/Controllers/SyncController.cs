namespace Microsoft.Teams.App.Mapper.Controllers
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Integration.AspNet.Core;
    using Microsoft.Bot.Connector.Authentication;
    using Microsoft.Bot.Schema;
    using Microsoft.Bot.Streaming.Payloads;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Teams.App.Mapper.Helper;
    using Microsoft.Teams.App.Mapper.Models;

    [Route("api/[controller]")]
    [ApiController]
    public class SyncController : ControllerBase
    {
        private readonly IBotFrameworkHttpAdapter _adapter;
        private readonly string appId;
        private readonly ConcurrentDictionary<string, ConversationReference> _conversationReferences;
        private readonly IProcessor processor;

        public SyncController(IBotFrameworkHttpAdapter adapter, IConfiguration configuration, ConcurrentDictionary<string, ConversationReference> conversationReferences, IProcessor proc)
        {
            _adapter = adapter;
            _conversationReferences = conversationReferences;
            appId = configuration["MicrosoftAppId"];
            processor = proc;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EventPayload payload)
        {
            List<string> statusUpdate = new List<string>();
            foreach (var conversationReference in _conversationReferences.Values)
            {
                // await ((BotAdapter)_adapter).ContinueConversationAsync(appId, conversationReference, BotCallback, default);
                await ((BotAdapter)_adapter).ContinueConversationAsync(appId, conversationReference, async (context, token) => {
                    MicrosoftAppCredentials.TrustServiceUrl(context.Activity.ServiceUrl);
                    payload = await processor.NotifySubscribersAsync(payload);
                    await context.SendActivityAsync($"{payload.EventName} requested for {payload.ServiceUrl} has response : {payload.Response}");
                }, default);
                statusUpdate.Add($"succeeded for {conversationReference.User.Name}");
            }

            // Let the caller know proactive messages have been sent
            return new ContentResult()
            {
                Content = string.Join(",", statusUpdate.ToArray()),
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
            };
        }
    }
}