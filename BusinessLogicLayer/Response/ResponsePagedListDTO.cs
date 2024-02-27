using DataTransferObjects;
using DataTransferObjects.Interfaces;
using System.Collections.Generic;


namespace BusinessLogicLayer.Response
{
    public class ResponsePagedListDTO<T> : IResponsePagedListDTO<T>
    {
        public IEnumerable<T> List => _list;
        public int RecordCount => _recordCount;
        public int CurrentPage => _currentPage;
        public int PageCount => _pageCount;
        public IValidate Validate => _validate;

        private readonly IEnumerable<T> _list;
        private readonly int _recordCount;
        private readonly int _currentPage;
        private readonly int _pageCount;
        private readonly IValidate _validate;

        public ResponsePagedListDTO(IEnumerable<T> list, int recordCount, IPagerDTO pagerDTO, IValidate validate)
        {
            this._list = list;
            this._recordCount = recordCount;
            this._currentPage = pagerDTO.CurrentPage;
            double pageCountDoub = Math.Ceiling(Convert.ToDouble((recordCount / Convert.ToDouble(pagerDTO.RecordsPerPage))));
            this._pageCount = Convert.ToInt32(pageCountDoub);
            _validate = validate;
        }

        public ResponsePagedListDTO(IValidate validate)
        {
            this._list = new List<T>();
            this._recordCount = 0;
            this._currentPage = 0;
            this._pageCount = 0;
            _validate = validate;
        }

        public static async Task<IResponsePagedListDTO<ReadDTO>> GetResponseFromQueryAsync<ReadDTO>(QueryStrategyBase<ReadDTO> queryStrategy, IPagerDTO pager, IValidate validate) where ReadDTO : class
        {
            var list = await queryStrategy.GetResultsAsync();
            var recordCount = await queryStrategy.CountResultsAsync();
            var currentPage = pager.CurrentPage;
            double pageCountDoub = Math.Ceiling(Convert.ToDouble((recordCount / Convert.ToDouble(pager.RecordsPerPage))));
            var pageCount = Convert.ToInt32(pageCountDoub);

            IResponsePagedListDTO<ReadDTO> newList = new ResponsePagedListDTO<ReadDTO>(list, recordCount, pager, validate);

            return newList;

        }
    }
}
