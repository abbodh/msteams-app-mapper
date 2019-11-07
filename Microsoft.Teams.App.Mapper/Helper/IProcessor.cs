using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Teams.App.Mapper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Teams.App.Mapper.Helper
{
    public interface IProcessor
    {
        Task<EventPayload> NotifySubscribersAsync(EventPayload eventPayload);

        Task<bool> WriteToStoreAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken);
    }
}
