using AdaptiveCards;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Teams.App.Mapper.Cards
{
    public static class Cards
    {
        public static Attachment GetSubscribeCard()
        {
            AdaptiveCard card = new AdaptiveCard("1.0");
            var container = new AdaptiveContainer()
            {
                Height = AdaptiveHeight.Auto,
                Items = new List<AdaptiveElement>
                    {
                        new AdaptiveTextBlock
                        {
                            Size = AdaptiveTextSize.Medium,
                            Wrap = true,
                            Text = "Event name",
                        },
                        new AdaptiveTextInput
                        {
                            Style = AdaptiveTextInputStyle.Text,
                            Id = "eventname",
                            MaxLength = 300,
                        },
                        new AdaptiveTextBlock
                        {
                            Size = AdaptiveTextSize.Medium,
                            Wrap = true,
                            Text = "Action type",
                        },
                        new AdaptiveChoiceSetInput
                        {
                          Id = "Action type",
                          Choices = new List<AdaptiveChoice>()
                          {
                              new AdaptiveChoice() { Title = "Open url", Value = "Open URL" },
                              new AdaptiveChoice() { Title = "Submit", Value = "Submit" },
                              new AdaptiveChoice() {Title = "Show card", Value = "Show card"}
                          }
                        }
                    },
            };
            card.Body.Add(container);
            var adaptiveCardAttachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card,
            };

            return adaptiveCardAttachment;
        }

        public static HeroCard GetHeroCard()
        {
            var heroCard = new HeroCard
            {
                Title = "BotFramework Hero Card",
                Subtitle = "Microsoft Bot Framework",
                Text = "Build and connect intelligent bots to interact with your users naturally wherever they are," +
                       " from text/sms to Skype, Slack, Office 365 mail and other popular services.",
                Images = new List<CardImage> { new CardImage("https://sec.ch9.ms/ch9/7ff5/e07cfef0-aa3b-40bb-9baa-7c9ef8ff7ff5/buildreactionbotframework_960.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl, "Get Started", value: "https://docs.microsoft.com/bot-framework") },
            };

            return heroCard;
        }



        public static ThumbnailCard GetThumbnailCard()
        {
            var heroCard = new ThumbnailCard
            {
                Title = "BotFramework Thumbnail Card",
                Subtitle = "Microsoft Bot Framework",
                Text = "Build and connect intelligent bots to interact with your users naturally wherever they are," +
                       " from text/sms to Skype, Slack, Office 365 mail and other popular services.",
                Images = new List<CardImage> { new CardImage("https://sec.ch9.ms/ch9/7ff5/e07cfef0-aa3b-40bb-9baa-7c9ef8ff7ff5/buildreactionbotframework_960.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl, "Get Started", value: "https://docs.microsoft.com/bot-framework") },
            };

            return heroCard;
        }
    }
}
