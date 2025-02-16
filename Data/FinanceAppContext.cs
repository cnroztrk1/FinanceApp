﻿using FinanceApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace FinanceApp.Data
{
    public class FinanceAppContext : DbContext
    {
        public FinanceAppContext(DbContextOptions<FinanceAppContext> options)
            : base(options)
        {
            // Veritabanı yoksa oluşturur, varsa devam eder.
            Database.EnsureCreated();
        }

        // Tablolar
        public DbSet<Agreement> Agreements { get; set; }
        public DbSet<AgreementKeys> AgreementKeywords { get; set; }
        public DbSet<Partners> BusinessPartners { get; set; }
        public DbSet<Jobs> Jobs { get; set; }
        public DbSet<RiskAnalysis> RiskAnalyses { get; set; }

        public DbSet<Company> Companies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // TenantId eklenen modeller
            modelBuilder.Entity<Agreement>().Property(a => a.TenantId).IsRequired();
            modelBuilder.Entity<AgreementKeys>().Property(a => a.TenantId).IsRequired();
            modelBuilder.Entity<Jobs>().Property(j => j.TenantId).IsRequired();
            modelBuilder.Entity<RiskAnalysis>().Property(r => r.TenantId).IsRequired();
            modelBuilder.Entity<Partners>().Property(p => p.TenantId).IsRequired();
            modelBuilder.Entity<Company>().Property(x=>x.Id).IsRequired();

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

            base.OnModelCreating(modelBuilder);
        }
    }
}
