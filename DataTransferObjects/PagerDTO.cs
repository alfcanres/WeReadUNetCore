using DataTransferObjects.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class PagerDTO : IPagerDTO
    {
        private string searchKeyWord = "";
        public string SearchKeyWord
        {
            set
            {
                searchKeyWord = value;
            }
            get
            {
                return searchKeyWord;
            }
        }
        public int CurrentPage { set; get; }
        private int recordsPerPage = 10;
        private readonly int maxRecordsPerPage = 50;

        public PagerDTO()
        {
            RecordsPerPage = maxRecordsPerPage;
        }

        public int RecordsPerPage
        {
            get
            {
                return recordsPerPage;
            }
            set
            {
                recordsPerPage = (value > maxRecordsPerPage) ? maxRecordsPerPage : value;
            }
        }
    }
}
