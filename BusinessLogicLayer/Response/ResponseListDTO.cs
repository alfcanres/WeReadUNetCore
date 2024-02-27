using DataTransferObjects.Interfaces;
using System.Collections.Generic;

namespace BusinessLogicLayer.Response
{
    public class ResponseListDTO<ReadDTO> : IResponseListDTO<ReadDTO>
    {
        public IEnumerable<ReadDTO> List => _list;
        public int RecordCount => _recordCount;
        public IValidate Validate => _validate;

        private readonly IEnumerable<ReadDTO> _list;
        private readonly IValidate _validate;
        private readonly int _recordCount;
        private readonly IValidate validate;

        public ResponseListDTO(IEnumerable<ReadDTO> list, IValidate validate)
        {
            this._list = list;
            this._recordCount = list.Count();
            this._validate = validate;
        }

        public ResponseListDTO(IEnumerable<ReadDTO> list, int recordCount, IValidate validate)
        {
            this._list = list;
            this._recordCount = recordCount;
            this._validate = validate;
        }

        public ResponseListDTO(IValidate validate)
        {
            this._list = new List<ReadDTO>();
            this._recordCount = 0;
            this.validate = validate;
        }

        public static async Task<IResponseListDTO<ReadDTO>> GetResponseFromQueryAsync(QueryStrategyBase<ReadDTO> queryStrategyBase, IValidate validate)
        {
            var list = await queryStrategyBase.GetResultsAsync();
            var recordCount = await queryStrategyBase.CountResultsAsync();

            return new ResponseListDTO<ReadDTO>(list, recordCount, validate);

        }
    }
}
