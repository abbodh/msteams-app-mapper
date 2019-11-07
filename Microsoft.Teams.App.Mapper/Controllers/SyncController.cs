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
    using Microsoft.Extensions.Configuration;

    [Route("api/[controller]")]
    [ApiController]
    public class SyncController : ControllerBase
    {
        private readonly IBotFrameworkHttpAdapter _adapter;
        private readonly string appId;
        private readonly ConcurrentDictionary<string, ConversationReference> _conversationReferences;

        public SyncController(IBotFrameworkHttpAdapter adapter, IConfiguration configuration, ConcurrentDictionary<string, ConversationReference> conversationReferences)
        {
            _adapter = adapter;
            _conversationReferences = conversationReferences;
            appId = configuration["MicrosoftAppId"];

            // If the channel is the Emulator, and authentication is not in use,
            // the AppId will be null.  We generate a random AppId for this case only.
            // This is not required for production, since the AppId will have a value.
            if (string.IsNullOrEmpty(appId))
            {
                appId = Guid.NewGuid().ToString(); //if no AppId, use a random Guid
            }
        }

        public async Task<IActionResult> Get()
        {
            List<string> statusUpdate = new List<string>();
            foreach (var conversationReference in _conversationReferences.Values)
            {
                await ((BotAdapter)_adapter).ContinueConversationAsync(appId, conversationReference, BotCallback, default(CancellationToken));
                statusUpdate.Add($"succeeded for {conversationReference.ChannelId}");
            }

            // Let the caller know proactive messages have been sent
            return new ContentResult()
            {
                Content = string.Join(",", statusUpdate.ToArray()),
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
            };
        }

        private async Task BotCallback(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            MicrosoftAppCredentials.TrustServiceUrl(turnContext.Activity.ServiceUrl);
            await turnContext.SendActivityAsync("proactive hello");
        }
    }
}