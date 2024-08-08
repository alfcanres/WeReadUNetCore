using DataTransferObjects.DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Client.Repository.PostVote;
using WebAPI.Client.ViewModels;

namespace WebAPI.Client.Repository.PostVote
{
    public class PostVoteRepository : IPostVoteRepository
    {
        public PostVoteRepository(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<PostVoteRepository> logger
            ) 
        {

        }

        public Task<ResponseViewModel<bool>> CreateAsync(PostVoteCreateDTO createModel)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseViewModel<bool>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void SetBearerToken(string bearerToken)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseViewModel<bool>> UpdateAsync(PostVoteUpdateDTO updateModel)
        {
            throw new NotImplementedException();
        }
    }
}
