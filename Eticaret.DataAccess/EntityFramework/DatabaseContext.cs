using Eticaret.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eticaret.DataAccess.EntityFramework
{
    public class DatabaseContext:DbContext
    {
        public DatabaseContext()
        {
            Database.SetInitializer(new MyInitializer());
        }
        public DbSet<TradeUser> TradeUsers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Liked> Likeds { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Note>()
                .HasMany(n => n.Comments)
                .WithRequired(c => c.Note)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Note>()
                .HasMany(n => n.Likes)
                .WithRequired(c => c.Note)
                .WillCascadeOnDelete(true);
        }

    }
}
