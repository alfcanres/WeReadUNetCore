using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Client.Repository
{
    public interface IBaseRepository
    {
        void SetBearerToken(string bearerToken);
    }
}
