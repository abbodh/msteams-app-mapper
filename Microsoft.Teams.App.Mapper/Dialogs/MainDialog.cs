using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Teams.App.Mapper.Cards;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Teams.App.Mapper.Dialogs
{
    public class MainDialog : LogoutDialog
    {
        protected readonly ILogger Logger;

        private static readonly MemoryStorage memoryStore = new MemoryStorage();

        public MainDialog(IConfiguration configuration, ILogger<MainDialog> logger)
            : base(nameof(MainDialog), configuration["ConnectionName"])
        {
            Logger = logger;

            //TODO :: add auth later
            AddDialog(new OAuthPrompt(
                nameof(OAuthPrompt),
                new OAuthPromptSettings
                {
                    ConnectionName = ConnectionName,
                    Text = "Please Sign In",
                    Title = "Sign In",
                    Timeout = 300000, // User has 5 minutes to login (1000 * 60 * 5)
                }));

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
               // PromptStepAsync
               ProcessCommandAsync
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> PromptStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            stepContext.Values["command"] = stepContext.Context.Activity.Text;
            if (stepContext.Context.Activity.Text == null && stepContext.Context.Activity.Value != null && stepContext.Context.Activity.Type == "message")
            {
                stepContext.Values["command"] = JToken.Parse(stepContext.Context.Activity.Value.ToString()).SelectToken("text").ToString();
            }

            return await stepContext.BeginDialogAsync(nameof(OAuthPrompt), null, cancellationToken);
        }

        private async Task<DialogTurnResult> ProcessCommandAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var attachments = new List<Attachment>();

            // Reply to the activity we received with an activity.
            var reply = MessageFactory.Attachment(attachments);
            var command = !string.IsNullOrEmpty(stepContext.Context.Activity.Text) ? stepContext.Context.Activity.Text.Trim().ToLower() : string.Empty;

            switch (command)
            {
                case "help":
                    reply.Attachments.Add(Cards.Cards.GetHeroCard().ToAttachment());
                    await stepContext.Context.SendActivityAsync(reply);
                    break;
                case "subscribe":
                    reply.Attachments.Add(Cards.Cards.GetSubscribeCard());
                    await stepContext.Context.SendActivityAsync(reply);
                    break;
                case "eventsubmit":
                    // save info for later notifications
                    var events = new Dictionary<string, object>();
                    {
                        events.Add(((JObject)stepContext.Context.Activity.Value).GetValue("eventname").ToString(), stepContext.Context.Activity);
                    };
                    await memoryStore.WriteAsync(events, cancellationToken);
                    await stepContext.Context.SendActivityAsync($"{((JObject)stepContext.Context.Activity.Value).GetValue("eventname").ToString()} is regisetered");
                    break;
                default:
                    await stepContext.Context.SendActivityAsync($"command not recognized. Type 'Help' to see supported commands.");
                    break;
            }

            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }
    }
}
