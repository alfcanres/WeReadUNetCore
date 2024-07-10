using DataTransferObjects;


namespace WebAPI.Client.ViewModels
{

    public class ResponseViewModel<T>
    {
        public ResponseViewModel()
        {
            Validate = new ValidatorResponse() { IsValid = true, MessageList = new List<string>() };
        }

        public T Content { set; get; }
        public ValidatorResponse Validate { set; get; }

    }

}
