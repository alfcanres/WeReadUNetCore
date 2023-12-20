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
        public IEnumerable<T> List { get { return list; } set { list = value; } }
        public int RecordCount { get { return recordCount; } set { recordCount = value; } }
        public int CurrentPage { get { return currentPage; } set { currentPage = value; } }
        public int PageCount { get { return pageCount; } set { pageCount = value; } }

        IEnumerable<T> list;
        int recordCount;
        int currentPage;
        int pageCount;


        public PagedListDTO(List<T> list, int recordCount, IPagerDTO pagerDTO)
        {
            SetPagedListDTO(list, recordCount, pagerDTO.RecordsPerPage, pagerDTO.CurrentPage);
        }

        public PagedListDTO()
        {
        }

        public void SetPagedListDTO(List<T> list, int recordCount, int recordsPerPage, int currenPage)
        {
            this.list = list;
            this.recordCount = recordCount;
            this.currentPage = currenPage;
            double pageCount = Math.Ceiling(Convert.ToDouble((recordCount / Convert.ToDouble(recordsPerPage))));
            this.pageCount = Convert.ToInt32(pageCount);
        }

    }
}
