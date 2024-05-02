using DataTransferObjects.Interfaces;

namespace BusinessLogicLayer.Interfaces
{
    public interface IDataAnnotationsValidator
    {
        void ValidateModel(object model, IValidate validate);
    }
}
