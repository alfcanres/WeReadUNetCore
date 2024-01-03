using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Helpers
{
    internal static class FriendlyErrorMessages
    {
        internal static readonly string ErrorOnInsertOpeation = "Ops!, we were unable to register the new data. Please try again later";
        internal static readonly string ErrorOnUpdateOpeation = "Ops!, we were unable to update the current data. Please try again later";
        internal static readonly string ErrorOnReadOpeation = "Ops!, we were unable to bring you back the requested information. Please try again later";
        internal static readonly string ErrorOnDeleteOperation = "Ops!, we were unable to delete that. Please try again later";
        internal static readonly string ErrorGeneric = "Ops!. Something went wrong please try again later.";
    }
}
