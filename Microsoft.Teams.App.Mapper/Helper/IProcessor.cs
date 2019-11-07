using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Teams.App.Mapper.Helper
{
    public interface IProcessor
    {
        Task ActivityProcessor(string activityPayload);
    }
}
