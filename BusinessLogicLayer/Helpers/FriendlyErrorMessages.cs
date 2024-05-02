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

    internal static class ValidationAccountErrorMessages
    {
        internal static readonly string OnInsertEmailAlreadyInUse = "There's already an user with that email. Please use another email";
        internal static readonly string OnInsertUserNameAlreadyInUse = "There's already an user with that user name. Please use another user name";
        internal static readonly string OnLoginIncorrectUserNameOrPassword = "Incorrect username or password.";
        internal static readonly string OnUpdatePasswordWrongPassword = "Incorrect password.";
        internal static readonly string OnUpdatePasswordIncorretConfirmPassword = "Your new password is different than confirm new password. Please verify";


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
        internal static readonly string ErrorChangingVoteForThisPost = "Somethig went wrong, when we find the developer who make this we will punish him";
        internal static readonly string AlreadyVotedForThisPost = "You already voted for this post";

    }

}
