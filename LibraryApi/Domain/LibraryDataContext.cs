using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Domain
{
    public class LibraryDataContext : DbContext
    {
        public LibraryDataContext(DbContextOptions<LibraryDataContext> options) : base(options)
        {

        }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().Property(p => p.Title).HasMaxLength(200);
            modelBuilder.Entity<Book>().Property(p => p.Author).HasMaxLength(200);
            
        }

        public virtual IQueryable<Book> BooksInInventory()
        {
            return Books.Where(b => b.IsInInventory == true);
        }
    }
}
