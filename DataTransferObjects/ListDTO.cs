using DataTransferObjects.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class ListDTO<T> : IListDTO<T>
    {
        public IEnumerable<T> List => _list;
        public int RecordCount => _list.Count();
        public IValidate Validate => _validate;

        private readonly IEnumerable<T> _list;
        private readonly IValidate _validate;

        public ListDTO(IEnumerable<T> list, IValidate validate)
        {
            this._list = list;
            _validate = validate;
        }
    }
}
