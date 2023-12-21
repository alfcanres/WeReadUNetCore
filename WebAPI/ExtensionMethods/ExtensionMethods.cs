using DataTransferObjects;
using DataTransferObjects.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Runtime.CompilerServices;

namespace WebAPI.ExtensionMethods
{
    public static class ExtensionMethods
    {
        public static IValidate ToValidate(this ModelStateDictionary modelState)
        {
            IValidate validate = new ValidateDTO(); 
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
