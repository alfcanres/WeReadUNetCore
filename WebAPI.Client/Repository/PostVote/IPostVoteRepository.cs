using DataTransferObjects.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Client.ViewModels;

namespace WebAPI.Client.Repository.PostVote
{
    public interface IPostVoteRepository : IBaseRepository
    {
        Task<ResponseViewModel<bool>> CreateAsync(PostVoteCreateDTO createModel);
        Task<ResponseViewModel<bool>> DeleteAsync(int id); 
        Task<ResponseViewModel<bool>> UpdateAsync(PostVoteUpdateDTO updateModel);

    }
}
