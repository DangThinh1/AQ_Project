﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AQBooking.Admin.Infrastructure.Databases.ConfigEntities
{
    public partial class AQConfigContext : DbContext
    {
        public AQConfigContext()
        {
        }

        public AQConfigContext(DbContextOptions<AQConfigContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AqmembershipDiscountPrivilegeDetails> AqmembershipDiscountPrivilegeDetails { get; set; }
        public virtual DbSet<AqmembershipDiscountPrivileges> AqmembershipDiscountPrivileges { get; set; }
        public virtual DbSet<AuthorizationFunctions> AuthorizationFunctions { get; set; }
        public virtual DbSet<AuthorizationModules> AuthorizationModules { get; set; }
        public virtual DbSet<AuthorizationPageFunctions> AuthorizationPageFunctions { get; set; }
        public virtual DbSet<AuthorizationPages> AuthorizationPages { get; set; }
        public virtual DbSet<AuthorizationRoles> AuthorizationRoles { get; set; }
        public virtual DbSet<Cities> Cities { get; set; }
        public virtual DbSet<CommonLanguages> CommonLanguages { get; set; }
        public virtual DbSet<CommonResources> CommonResources { get; set; }
        public virtual DbSet<CommonValues> CommonValues { get; set; }
        public virtual DbSet<Countries> Countries { get; set; }
        public virtual DbSet<Currencies> Currencies { get; set; }
        public virtual DbSet<DataProtectionKeys> DataProtectionKeys { get; set; }
        public virtual DbSet<PortalLanguageControls> PortalLanguageControls { get; set; }
        public virtual DbSet<PortalLocationControls> PortalLocationControls { get; set; }
        public virtual DbSet<ZoneDistricts> ZoneDistricts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AqmembershipDiscountPrivilegeDetails>(entity =>
            {
                entity.ToTable("AQMembershipDiscountPrivilegeDetails");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BookingDomainFid).HasColumnName("BookingDomainFID");

                entity.Property(e => e.BookingDomainName).HasMaxLength(456);

                entity.Property(e => e.BookingDomainUniqueId)
                    .IsRequired()
                    .HasColumnName("BookingDomainUniqueID")
                    .HasMaxLength(12);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.PrivilegeFid).HasColumnName("PrivilegeFID");

                entity.Property(e => e.Remark).HasMaxLength(2000);
            });

            modelBuilder.Entity<AqmembershipDiscountPrivileges>(entity =>
            {
                entity.ToTable("AQMembershipDiscountPrivileges");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveEndDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(456);

                entity.Property(e => e.Remark).HasMaxLength(2000);

                entity.Property(e => e.RoleFid).HasColumnName("RoleFID");

                entity.Property(e => e.RoleName).HasMaxLength(256);
            });

            modelBuilder.Entity<AuthorizationFunctions>(entity =>
            {
                entity.HasKey(e => e.FuntionId);

                entity.Property(e => e.FuntionId).HasColumnName("FuntionID");

                entity.Property(e => e.FunctionName).HasMaxLength(150);

                entity.Property(e => e.FunctionNameResKey).HasMaxLength(150);
            });

            modelBuilder.Entity<AuthorizationModules>(entity =>
            {
                entity.HasKey(e => e.ModuleId);

                entity.Property(e => e.ModuleId).HasColumnName("ModuleID");

                entity.Property(e => e.ActivatedDate).HasColumnType("datetime");

                entity.Property(e => e.ControllerName).HasMaxLength(150);

                entity.Property(e => e.DisplayName).HasMaxLength(150);

                entity.Property(e => e.DisplayNameResKey).HasMaxLength(150);

                entity.Property(e => e.Icon).HasMaxLength(50);
            });

            modelBuilder.Entity<AuthorizationPageFunctions>(entity =>
            {
                entity.HasKey(e => new { e.PageId, e.FunctionId })
                    .HasName("PK_PageFunction");

                entity.Property(e => e.PageId).HasColumnName("PageID");

                entity.Property(e => e.FunctionId).HasColumnName("FunctionID");
            });

            modelBuilder.Entity<AuthorizationPages>(entity =>
            {
                entity.HasKey(e => e.PageId);

                entity.Property(e => e.PageId).HasColumnName("PageID");

                entity.Property(e => e.Action).HasMaxLength(50);

                entity.Property(e => e.Controller).HasMaxLength(50);

                entity.Property(e => e.Icon).HasMaxLength(50);

                entity.Property(e => e.LinkVideo).HasMaxLength(250);

                entity.Property(e => e.ModuleFid).HasColumnName("ModuleFID");

                entity.Property(e => e.PageName).HasMaxLength(150);

                entity.Property(e => e.PageNameResKey).HasMaxLength(150);

                entity.Property(e => e.ParentFid).HasColumnName("ParentFID");

                entity.Property(e => e.Tooltip).HasMaxLength(150);

                entity.Property(e => e.WebName).HasMaxLength(150);
            });

            modelBuilder.Entity<AuthorizationRoles>(entity =>
            {
                entity.HasKey(e => new { e.DesignationId, e.PageFid, e.FunctionFid });

                entity.Property(e => e.DesignationId).HasColumnName("DesignationID");

                entity.Property(e => e.PageFid).HasColumnName("PageFID");

                entity.Property(e => e.FunctionFid).HasColumnName("FunctionFID");
            });

            modelBuilder.Entity<Cities>(entity =>
            {
                entity.HasKey(e => e.CityCode)
                    .HasName("PK__Cities__A5D4AA33D2470CAC");

                entity.Property(e => e.CityCode).ValueGeneratedNever();

                entity.Property(e => e.CityName).HasMaxLength(256);
            });

            modelBuilder.Entity<CommonLanguages>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CssClass).HasMaxLength(100);

                entity.Property(e => e.LanguageCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LanguageName).HasMaxLength(250);

                entity.Property(e => e.Remarks).HasMaxLength(250);

                entity.Property(e => e.ResourceKey).HasMaxLength(150);

                entity.Property(e => e.UniqueId)
                    .HasColumnName("UniqueID")
                    .HasMaxLength(12)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CommonResources>(entity =>
            {
                entity.HasKey(e => new { e.ResourceKey, e.LanguageFid });

                entity.Property(e => e.ResourceKey).HasMaxLength(150);

                entity.Property(e => e.LanguageFid).HasColumnName("LanguageFID");

                entity.Property(e => e.TypeOfResource).HasMaxLength(200);
            });

            modelBuilder.Entity<CommonValues>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ResourceKey).HasMaxLength(150);

                entity.Property(e => e.Text).HasMaxLength(250);

                entity.Property(e => e.UniqueId)
                    .HasColumnName("UniqueID")
                    .HasMaxLength(12);

                entity.Property(e => e.ValueGroup).HasMaxLength(250);

                entity.Property(e => e.ValueString).HasMaxLength(250);
            });

            modelBuilder.Entity<Countries>(entity =>
            {
                entity.HasKey(e => e.CountryCode)
                    .HasName("PK__Countrie__5D9B0D2DC2B56FFE");

                entity.Property(e => e.CountryCode).ValueGeneratedNever();

                entity.Property(e => e.CountryName).HasMaxLength(256);

                entity.Property(e => e.Description).HasMaxLength(456);
            });

            modelBuilder.Entity<Currencies>(entity =>
            {
                entity.HasKey(e => e.CurrencyCode)
                    .HasName("PK__Currenci__408426BE296F069F");

                entity.Property(e => e.CurrencyCode).HasMaxLength(10);

                entity.Property(e => e.City).HasMaxLength(100);

                entity.Property(e => e.Country).HasMaxLength(100);

                entity.Property(e => e.CultureCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.ResourceKey)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<PortalLanguageControls>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DomainPortalFid).HasColumnName("DomainPortalFID");

                entity.Property(e => e.EffectiveDate).HasColumnType("datetime");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.LanguageFid).HasColumnName("LanguageFID");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.PortalUniqueId)
                    .IsRequired()
                    .HasColumnName("PortalUniqueID")
                    .HasMaxLength(12);
            });

            modelBuilder.Entity<PortalLocationControls>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CityName).HasMaxLength(100);

                entity.Property(e => e.CountryName).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CssClass).HasMaxLength(100);

                entity.Property(e => e.DomainPortalFid).HasColumnName("DomainPortalFID");

                entity.Property(e => e.FileStreamFid).HasColumnName("FileStreamFID");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.PortalUniqueId)
                    .IsRequired()
                    .HasColumnName("PortalUniqueID")
                    .HasMaxLength(12);
            });

            modelBuilder.Entity<ZoneDistricts>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description).HasMaxLength(300);

                entity.Property(e => e.ZoneDistrictName).HasMaxLength(456);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}