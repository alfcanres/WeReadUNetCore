namespace DataTransferObjects
{
    public class Response<ReadDTO> where ReadDTO : class
    {
        private ReadDTO _DTO;

        public Response()
        {
        }

        public Response(ReadDTO DTO)
        {
            this.Data = DTO;

        }

        public ReadDTO Data { get => _DTO; set => _DTO = value; }
    }
}
