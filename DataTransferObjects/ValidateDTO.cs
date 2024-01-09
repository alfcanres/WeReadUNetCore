using DataTransferObjects.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class ValidateDTO : IValidate
    {
        bool isValid = true;
        List<string> errorList = new List<string>();

        public ValidateDTO()
        {
        }

        public bool IsValid
        {
            get
            {
                return isValid;
            }
            set
            {
                isValid = value;
            }
        }

        public List<string> MessageList
        {
            get
            {
                return errorList;
            }
            set
            {
                errorList = value;
            }
        }

        public void AddError(string error)
        {
            errorList.Add(error);
            IsValid = false;
        }

        public void Clear()
        {
            MessageList.Clear();
            isValid = true;
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }

    }

}
