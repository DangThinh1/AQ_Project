using AQ_PGW.Core.Models.DBTables;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.ComponentModel.DataAnnotations.Schema;

namespace AQ_PGW.Infrastructure.DataContext
{
    public class AQ_PaymentGatewayDBContext : DbContext
    {     

        public AQ_PaymentGatewayDBContext(DbContextOptions options)
            : base(options)
        {
        }
        public DbSet<SystemLogs> SystemLogs { get; set; }
        public DbSet<Transactions> Transactions { get; set; }
        public DbSet<TransactionItems> TransactionItems { get; set; }
        public DbSet<UserAPI> UserAPI { get; set; }
        public DbSet<PaymentLogs> PaymentLogs { get; set; }
        public DbSet<LinksRelated> LinksRelated { get; set; }
        public DbSet<RelatedTransactionDetails> RelatedTransactionDetails { get; set; }
        public DbSet<RefundPaypal> RefundPaypal { get; set; }
        public DbSet<RefundStripe> RefundStripe { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseSqlServer(@"Server=172.16.10.137,1500;Database=AQ_PaymentGateway;User ID=vovms;Password=vovms2017;Trusted_Connection=false;MultipleActiveResultSets=true");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SystemLogs>().HasKey(x => x.ID);
            modelBuilder.Entity<SystemLogs>().Property(x => x.ID).UseSqlServerIdentityColumn();

            modelBuilder.Entity<PaymentLogs>().Property(x => x.ID).UseSqlServerIdentityColumn();

            modelBuilder.Entity<LinksRelated>().Property(x => x.ID).UseSqlServerIdentityColumn();
        }
    }
}
