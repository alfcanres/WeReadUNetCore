using DataAccessLayer.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IUnitOfWork
    {
        public IRepository<Post> Posts { get; }
        public IRepository<PostType> PostTypes { get; }
        public IRepository<PostVote> PostVotes { get; }
        public IRepository<MoodType> MoodTypes { get; }
        Task SaveAsync();
    }
}
