using FlagX0.Web.Business.UserInfo;
using FlagX0.Web.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FlagX0.Web.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IFlagUserDetails flagUserDetails) 
        : IdentityDbContext(options)
    {
		public DbSet<FlagEntity> Flags { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FlagEntity>()
                .HasQueryFilter(a => !a.IsDeleted 
                    && a.UserId == flagUserDetails.UserId);

			base.OnModelCreating(modelBuilder);
		}
    }
}
