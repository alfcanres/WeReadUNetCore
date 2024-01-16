using DataTransferObjects.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class PagedListDTO<T> : IPagedListDTO<T>
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

        public PagedListDTO(IEnumerable<T> list, int recordCount, IPagerDTO pagerDTO, IValidate validate)
        {
            this._list = list;
            this._recordCount = recordCount;
            this._currentPage = pagerDTO.CurrentPage;
            double pageCountDoub = Math.Ceiling(Convert.ToDouble((recordCount / Convert.ToDouble(pagerDTO.RecordsPerPage))));
            this._pageCount = Convert.ToInt32(pageCountDoub);
            _validate = validate;
        }

    }
}
