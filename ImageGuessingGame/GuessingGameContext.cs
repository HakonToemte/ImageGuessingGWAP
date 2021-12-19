using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Reflection;    
using System; 
using System.IO;
using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using BC = BCrypt.Net.BCrypt;
using ImageGuessingGame.GameContext;


namespace ImageGuessingGame
{
    // This class should inherit from the EntityFramework DbContext
    public class GuessingGameContext : IdentityDbContext
    {    
        public string DbPath { get; private set; }
        public GuessingGameContext(DbContextOptions<GuessingGameContext> options)
            :base(options)
        {
            DbPath = "sqlitedb1";
        }
        public DbSet<LoginUser> LoginUsers {get; set; }
        public DbSet<Game> Games {get; set; }
        public DbSet<Oracle> Oracles{get;set;}
        public DbSet<Suggestion> Suggestions{get;set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LoginUser>().ToTable("loginusers");
            modelBuilder.Entity<IdentityUser>(entity => {
                    entity.HasIndex(u=>u.UserName).IsUnique();
            });
            modelBuilder.Entity<ImageGuessingGame.GameContext.Index>()
                .HasOne(i => i.Oracle)
                .WithMany(o => o.PartialIndex)
                .HasForeignKey(i=>i.OracleId);
            base.OnModelCreating(modelBuilder);
        }
    }
}
