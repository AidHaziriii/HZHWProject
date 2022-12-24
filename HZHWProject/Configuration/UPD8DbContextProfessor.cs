using HZHWProject.Models;
using Microsoft.EntityFrameworkCore;

namespace HZHWProject.Configuration
{
    public class UPD8DbContext : DbContext
    {
        public UPD8DbContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Professor> Professor { get; set; }
    }
}