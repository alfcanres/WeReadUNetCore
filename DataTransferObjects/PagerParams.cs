
using System.ComponentModel.DataAnnotations;

namespace DataTransferObjects
{
    public class PagerParams 
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

        private int currentPage = 1;    


        private int recordsPerPage = 10;
        private readonly int maxRecordsPerPage = 50;

        public PagerParams()
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

        public int CurrentPage { get => currentPage; set => currentPage = value; }

        public string ToQueryString()
        {
            return $"?currentPage={CurrentPage}&recordsPerPage={RecordsPerPage}&searchKeyWord={SearchKeyWord}"; 
        }
    }
}
