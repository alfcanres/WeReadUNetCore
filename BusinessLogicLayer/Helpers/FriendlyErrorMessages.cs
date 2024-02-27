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


    internal static class ValidationErrorMessages
    {
        internal static readonly string OnInsertAnItemAlreadyExists = "An item with the same description already exists, please type another";
        internal static readonly string OnDeleteNoRecordWasFound = "No record was found or was previously deleted";
        internal static readonly string OnUpdateNoRecordWasFound = "No record was found or was previously deleted";
    }

    internal static class ValidationPostErrorMessages
    {
        internal static readonly string OnTryPublishAlreadyPublished = "This post was already published";

    }

}
