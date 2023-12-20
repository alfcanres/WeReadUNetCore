using DataAccessLayer.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Enabling lazy loading by proxies requires three steps:
            //add package Microsoft.EntityFrameworkCore.Proxies
            //optionsBuilder.UseLazyLoadingProxies();

            base.OnConfiguring(optionsBuilder); 

        }


        public DbSet<Post> Posts => Set<Post>();
        public DbSet<PostType> PostTypes => Set<PostType>();
        public DbSet<PostVote> PostVotes => Set<PostVote>();



    }
}
