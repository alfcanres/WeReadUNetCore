using DataTransferObjects;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebAPI.ExtensionMethods
{
    public static class ExtensionMethods
    {
        public static ValidatorResponse ToValidate(this ModelStateDictionary modelState)
        {
            ValidatorResponse validate = new ValidatorResponse(); 
            if (!modelState.IsValid)
            {
                validate.IsValid = false;
                foreach (var key in modelState.Keys)
                {
                    var _modelState = modelState[key];
                    foreach (var error in _modelState.Errors)
                    {
                        validate.MessageList.Add(error.ErrorMessage);
                    }
                }
            }

            return validate;

        }  
    }
}
