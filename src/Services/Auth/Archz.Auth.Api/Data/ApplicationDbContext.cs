using Microsoft.EntityFrameworkCore;

namespace Archz.Auth.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options)
        : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder) { }
    }
}
