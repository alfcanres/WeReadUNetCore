﻿using DataAccessLayer.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace DataAccessLayer
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
           

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
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
        public DbSet<PostComment> PostComments  => Set<PostComment>();

    }
}
