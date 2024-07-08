using DataTransferObjects.Interfaces;
using DataTransferObjects;


namespace WebAPI.Client.ViewModels
{

    public class ResponseViewModel<T>
    {
        public ResponseViewModel()
        {
            Validate = new ValidateDTO() { IsValid = true, MessageList = new List<string>() };
        }

        public T Content { set; get; }
        public ValidateDTO Validate { set; get; }

    }

}
