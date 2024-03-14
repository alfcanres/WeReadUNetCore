using BusinessLogicLayer.Interfaces;
using DataAccessLayer;
using DataAccessLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _appDbContext;
        private readonly IRepository<Post> _post;
        private readonly IRepository<PostType> _postType;
        private readonly IRepository<PostVote> _postVote;
        private readonly IRepository<MoodType> _moodType;
        private readonly IRepository<ApplicationUserInfo> _applicationUserInfo;


        public UnitOfWork(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _post = new Repository<Post>(appDbContext);
            _postType = new Repository<PostType>(appDbContext);
            _postVote = new Repository<PostVote>(appDbContext);
            _moodType = new Repository<MoodType>(appDbContext);
            _applicationUserInfo = new Repository<ApplicationUserInfo>(appDbContext);
        }

        public IRepository<Post> Posts => _post;
        public IRepository<PostType> PostTypes => _postType;
        public IRepository<PostVote> PostVotes => _postVote;
        public IRepository<MoodType> MoodTypes => _moodType;
        public IRepository<ApplicationUserInfo> UsersInfo => _applicationUserInfo;

        public async Task SaveAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }
    }
}
