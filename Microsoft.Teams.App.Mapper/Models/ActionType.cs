using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Teams.App.Mapper.Models
{
    public enum ActionType
    {
        /// <summary>
        /// Recent tickets
        /// </summary>
        OpenUrl,

        /// <summary>
        /// Open tickets
        /// </summary>
        Submit,

        /// <summary>
        /// Tickets assigned to a subject-matter expert
        /// </summary>
        ShowCard
    }
}
