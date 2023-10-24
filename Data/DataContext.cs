using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Api_leaning.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Skill>().HasMany(e => e.Characters).WithMany(e => e.Skills);
            modelBuilder.Entity<MyUser>().Property(user => user.Role).HasDefaultValue("Player");



            // modelBuilder.Entity<Post>()
            //     .HasMany(e => e.Tags)
            //     .WithMany(e => e.Posts);
        }
        public DbSet<Character> Characters { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<MyUser> MyUser { get; set; }
        public DbSet<Weapon> Weapons { get; set; }

        public DbSet<Skill> Skills { get; set; }


    }
}