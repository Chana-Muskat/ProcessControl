using Microsoft.EntityFrameworkCore;
using ProcessControl.Server.Models.Ramdor;

namespace ProcessControl.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
      : base(options) { }
        public DbSet<StatusIv> StatusIv { get; set; }
        public DbSet<RamIvStages> RamIvStages { get; set; }
        public DbSet<RamInvoices> RamInvoices   { get; set; }   
        public DbSet<RamStagesPerIv> RamStagesPerIv { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RamInvoices>()
                .HasOne(i => i.StatusIvId)
                .WithMany(s => s.RamInvoice)
                .HasForeignKey(i => i.StatusIv);


            modelBuilder.Entity<RamStagesPerIv>()
                .HasOne(i => i.InvoiceId)
                .WithMany(s => s.RamStagePerIv)
                .HasForeignKey(i => i.InvoiceIdf)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RamStagesPerIv>()
             .HasOne(i => i.StageId)
             .WithMany(s => s.RamStagePerIv)
             .HasForeignKey(i => i.StageIdf)
             .OnDelete(DeleteBehavior.Restrict);
        }


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("YourConnectionStringHere");
        //}

    }
}
