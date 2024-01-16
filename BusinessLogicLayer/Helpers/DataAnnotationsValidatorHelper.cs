using BusinessLogicLayer.Interfaces;
using DataTransferObjects.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.Helpers
{
    public class DataAnnotationsValidatorHelper : IDataAnnotationsValidator
    {
        public void ValidateModel(object model, IValidate validate)
        {
            if (model != null)
            {
                var context = new ValidationContext(model, serviceProvider: null, items: null);
                var results = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(model, context, results);
                if (!isValid)
                {
                    foreach (var item in results)
                    {
                        validate.AddError(item.ErrorMessage);
                    }
                }
            }
            else
            {
                validate.AddError("Model must not be null");
            }
        }

    }
}
