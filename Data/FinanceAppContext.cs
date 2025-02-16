using FinanceApp.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Data
{
    public class FinanceAppContext : DbContext
    {
        public FinanceAppContext(DbContextOptions<FinanceAppContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Agreement> Agreements { get; set; }
        public DbSet<AgreementKeys> AgreementKeywords { get; set; }
        public DbSet<Partners> BusinessPartners { get; set; }
        public DbSet<Jobs> Jobs { get; set; }
        public DbSet<RiskAnalysis> RiskAnalyses { get; set; }
        public DbSet<Company> Companies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Agreement>().Property(a => a.TenantId).IsRequired();
            modelBuilder.Entity<AgreementKeys>().Property(a => a.TenantId).IsRequired();
            modelBuilder.Entity<Jobs>().Property(j => j.TenantId).IsRequired();
            modelBuilder.Entity<RiskAnalysis>().Property(r => r.TenantId).IsRequired();
            modelBuilder.Entity<Partners>().Property(p => p.TenantId).IsRequired();
            modelBuilder.Entity<Company>().Property(x => x.Id).IsRequired();

            modelBuilder.Entity<Agreement>()
                .HasMany(a => a.Keywords)
                .WithOne(k => k.Agreement)
                .HasForeignKey(k => k.AgreementId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Agreement>()
                .HasMany(a => a.Jobs)
                .WithOne(j => j.Agreement)
                .HasForeignKey(j => j.AgreementId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Partners>()
                .HasMany(bp => bp.Jobs)
                .WithOne(j => j.BusinessPartner)
                .HasForeignKey(j => j.BusinessPartnerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Agreement>()
                .HasMany(a => a.RiskAnalyses)
                .WithOne(ra => ra.Agreement)
                .HasForeignKey(ra => ra.AgreementId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Jobs>()
                .HasOne(j => j.RiskAnalysis)
                .WithOne(ra => ra.Job)
                .HasForeignKey<RiskAnalysis>(ra => ra.JobId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Company>().HasData(
                new Company { Id = 1, UserName = "Caner", Password = Company.HashPassword("123") },
                new Company { Id = 2, UserName = "HDI", Password = Company.HashPassword("123") }
            );

            // Tenant 1 ve Tenant 2 için rastgele Agreements
            modelBuilder.Entity<Agreement>().HasData(
                new Agreement { Id = 1, Name = "Agreement A1", Description = "Agreement for Tenant 1", StartDate = DateTime.UtcNow, IsActive = true, TenantId = 1 },
                new Agreement { Id = 2, Name = "Agreement A2", Description = "Agreement for Tenant 1", StartDate = DateTime.UtcNow, IsActive = true, TenantId = 1 },
                new Agreement { Id = 3, Name = "Agreement A3", Description = "Agreement for Tenant 1", StartDate = DateTime.UtcNow, IsActive = true, TenantId = 1 },
                new Agreement { Id = 4, Name = "Agreement A4", Description = "Agreement for Tenant 1", StartDate = DateTime.UtcNow, IsActive = true, TenantId = 1 },
                new Agreement { Id = 5, Name = "Agreement A5", Description = "Agreement for Tenant 1", StartDate = DateTime.UtcNow, IsActive = true, TenantId = 1 },

                new Agreement { Id = 6, Name = "Agreement B1", Description = "Agreement for Tenant 2", StartDate = DateTime.UtcNow, IsActive = true, TenantId = 2 },
                new Agreement { Id = 7, Name = "Agreement B2", Description = "Agreement for Tenant 2", StartDate = DateTime.UtcNow, IsActive = true, TenantId = 2 },
                new Agreement { Id = 8, Name = "Agreement B3", Description = "Agreement for Tenant 2", StartDate = DateTime.UtcNow, IsActive = true, TenantId = 2 },
                new Agreement { Id = 9, Name = "Agreement B4", Description = "Agreement for Tenant 2", StartDate = DateTime.UtcNow, IsActive = true, TenantId = 2 },
                new Agreement { Id = 10, Name = "Agreement B5", Description = "Agreement for Tenant 2", StartDate = DateTime.UtcNow, IsActive = true, TenantId = 2 }
            );

            // Tenant 1 ve Tenant 2 için rastgele Business Partners
            modelBuilder.Entity<Partners>().HasData(
                new Partners { Id = 1, Name = "Partner A1", ContactEmail = "a1@tenant1.com", PhoneNumber = "123456789", Address = "Address A1", TenantId = 1 },
                new Partners { Id = 2, Name = "Partner A2", ContactEmail = "a2@tenant1.com", PhoneNumber = "123456789", Address = "Address A2", TenantId = 1 },
                new Partners { Id = 3, Name = "Partner A3", ContactEmail = "a3@tenant1.com", PhoneNumber = "123456789", Address = "Address A3", TenantId = 1 },
                new Partners { Id = 4, Name = "Partner A4", ContactEmail = "a4@tenant1.com", PhoneNumber = "123456789", Address = "Address A4", TenantId = 1 },
                new Partners { Id = 5, Name = "Partner A5", ContactEmail = "a5@tenant1.com", PhoneNumber = "123456789", Address = "Address A5", TenantId = 1 },

                new Partners { Id = 6, Name = "Partner B1", ContactEmail = "b1@tenant2.com", PhoneNumber = "123456789", Address = "Address B1", TenantId = 2 },
                new Partners { Id = 7, Name = "Partner B2", ContactEmail = "b2@tenant2.com", PhoneNumber = "123456789", Address = "Address B2", TenantId = 2 },
                new Partners { Id = 8, Name = "Partner B3", ContactEmail = "b3@tenant2.com", PhoneNumber = "123456789", Address = "Address B3", TenantId = 2 },
                new Partners { Id = 9, Name = "Partner B4", ContactEmail = "b4@tenant2.com", PhoneNumber = "123456789", Address = "Address B4", TenantId = 2 },
                new Partners { Id = 10, Name = "Partner B5", ContactEmail = "b5@tenant2.com", PhoneNumber = "123456789", Address = "Address B5", TenantId = 2 }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
