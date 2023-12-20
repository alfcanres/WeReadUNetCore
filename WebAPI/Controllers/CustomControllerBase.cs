using BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public abstract class CustomControllerBase<CreateDTO, ReadDTO, UpdateDTO>: ControllerBase
    {
        protected readonly IBLL<CreateDTO, ReadDTO, UpdateDTO> _BLL;

        protected CustomControllerBase(IBLL<CreateDTO, ReadDTO, UpdateDTO> BLL)
        {
            _BLL = BLL;
        }


        
    }
}
