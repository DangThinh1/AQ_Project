using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace YachtMerchant.Infrastructure.Entities
{
    public partial class YachtMerchantDbContext : DbContext
    {
        public YachtMerchantDbContext()
        {
        }

        public YachtMerchantDbContext(DbContextOptions<YachtMerchantDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AuthorizationFunctions> AuthorizationFunctions { get; set; }
        public virtual DbSet<AuthorizationModules> AuthorizationModules { get; set; }
        public virtual DbSet<AuthorizationPageFunctions> AuthorizationPageFunctions { get; set; }
        public virtual DbSet<AuthorizationPages> AuthorizationPages { get; set; }
        public virtual DbSet<AuthorizationRoles> AuthorizationRoles { get; set; }
        public virtual DbSet<CommonLanguages> CommonLanguages { get; set; }
        public virtual DbSet<CommonResources> CommonResources { get; set; }
        public virtual DbSet<CommonValues> CommonValues { get; set; }
        public virtual DbSet<Currencies> Currencies { get; set; }
        public virtual DbSet<PortLocations> PortLocations { get; set; }
        public virtual DbSet<PortalLanguageControls> PortalLanguageControls { get; set; }
        public virtual DbSet<PortalLocationControls> PortalLocationControls { get; set; }
        public virtual DbSet<YachtAttributeValues> YachtAttributeValues { get; set; }
        public virtual DbSet<YachtAttributes> YachtAttributes { get; set; }
        public virtual DbSet<YachtCharteringPaymentLogs> YachtCharteringPaymentLogs { get; set; }
        public virtual DbSet<YachtCharterings> YachtCharterings { get; set; }
        public virtual DbSet<YachtCounters> YachtCounters { get; set; }
        public virtual DbSet<YachtDestinationPlanDetails> YachtDestinationPlanDetails { get; set; }
        public virtual DbSet<YachtDestinationPlans> YachtDestinationPlans { get; set; }
        public virtual DbSet<YachtFileStreams> YachtFileStreams { get; set; }
        public virtual DbSet<YachtInformationDetails> YachtInformationDetails { get; set; }
        public virtual DbSet<YachtInformations> YachtInformations { get; set; }
        public virtual DbSet<YachtMerchantAgreements> YachtMerchantAgreements { get; set; }
        public virtual DbSet<YachtMerchantAqmgts> YachtMerchantAqmgts { get; set; }
        public virtual DbSet<YachtMerchantUsers> YachtMerchantUsers { get; set; }
        public virtual DbSet<YachtMerchants> YachtMerchants { get; set; }
        public virtual DbSet<YachtNonOperationDays> YachtNonOperationDays { get; set; }
        public virtual DbSet<YachtOtherInformations> YachtOtherInformations { get; set; }
        public virtual DbSet<YachtPorts> YachtPorts { get; set; }
        public virtual DbSet<YachtPricingPlanDetails> YachtPricingPlanDetails { get; set; }
        public virtual DbSet<YachtPricingPlanInformations> YachtPricingPlanInformations { get; set; }
        public virtual DbSet<YachtPricingPlans> YachtPricingPlans { get; set; }
        public virtual DbSet<Yachts> Yachts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                //optionsBuilder.UseSqlServer("Data Source=172.16.10.137,1600;Initial Catalog=AQ_Bookings;Persist Security Info=True;User ID=erp;Password=erp2017");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity<AuthorizationFunctions>(entity =>
            {
                entity.HasKey(e => e.FuntionId);

                entity.Property(e => e.FuntionId).HasColumnName("FuntionID");

                entity.Property(e => e.FunctionName).HasMaxLength(150);
            });

            modelBuilder.Entity<AuthorizationModules>(entity =>
            {
                entity.HasKey(e => e.ModuleId);

                entity.Property(e => e.ModuleId).HasColumnName("ModuleID");

                entity.Property(e => e.ActivatedDate).HasColumnType("datetime");

                entity.Property(e => e.ControllerName).HasMaxLength(150);

                entity.Property(e => e.DisplayName).HasMaxLength(150);

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

            modelBuilder.Entity<CommonLanguages>(entity =>
            {
                entity.ToTable("CommonLanguages");

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
                entity.HasKey(e => new { e.LanguageFid, e.ResourceKey });

                entity.ToTable("CommonResources");

                entity.HasIndex(e => e.ResourceKey)
                    .HasName("UQ__Common_R__3B5BC9BE7703A653")
                    .IsUnique();

                entity.Property(e => e.LanguageFid).HasColumnName("LanguageFID");

                entity.Property(e => e.ResourceKey).HasMaxLength(150);

                entity.Property(e => e.TypeOfResource).HasMaxLength(200);
            });

            modelBuilder.Entity<CommonValues>(entity =>
            {
                entity.ToTable("CommonValues");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ResourceKey).HasMaxLength(150);

                entity.Property(e => e.Text).HasMaxLength(250);

                entity.Property(e => e.UniqueId)
                    .HasColumnName("UniqueID")
                    .HasMaxLength(12);

                entity.Property(e => e.ValueGroup).HasMaxLength(250);

                entity.Property(e => e.ValueString).HasMaxLength(250);
            });

            modelBuilder.Entity<Currencies>(entity =>
            {
                entity.HasKey(e => e.CurrencyCode)
                    .HasName("PK__Currenci__408426BE296F069F");

                entity.Property(e => e.CurrencyCode)
                    .HasMaxLength(10)
                    .ValueGeneratedNever();

                entity.Property(e => e.City).HasMaxLength(100);

                entity.Property(e => e.Country).HasMaxLength(100);

                entity.Property(e => e.CultureCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.ResourceKey)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<PortLocations>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.City).HasMaxLength(256);

                entity.Property(e => e.Country).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.PickupPointName).HasMaxLength(256);

                entity.Property(e => e.UniqueId)
                    .IsRequired()
                    .HasColumnName("UniqueID")
                    .HasMaxLength(12);

                entity.Property(e => e.ZoneDistrict).HasMaxLength(256);
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

            modelBuilder.Entity<YachtAttributeValues>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AttributeCategoryFid).HasColumnName("AttributeCategoryFID");

                entity.Property(e => e.AttributeFid).HasColumnName("AttributeFID");

                entity.Property(e => e.AttributeValue).HasMaxLength(256);

                entity.Property(e => e.EffectiveDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.YachtFid).HasColumnName("YachtFID");
            });

            modelBuilder.Entity<YachtAttributes>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AttributeCategoryFid).HasColumnName("AttributeCategoryFID");

                entity.Property(e => e.AttributeName)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Deleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.IconCssClass).HasMaxLength(50);

                entity.Property(e => e.IsDefault).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Remarks).HasMaxLength(250);

                entity.Property(e => e.ResourceKey)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.UniqueId)
                    .IsRequired()
                    .HasColumnName("UniqueID")
                    .HasMaxLength(12)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<YachtCharteringPaymentLogs>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CharteringFid).HasColumnName("CharteringFID");

                entity.Property(e => e.CultureCode).HasMaxLength(6);

                entity.Property(e => e.CurrencyCode).HasMaxLength(10);

                entity.Property(e => e.PaymentBy).HasMaxLength(100);

                entity.Property(e => e.PaymentDate).HasColumnType("datetime");

                entity.Property(e => e.PaymentMethod).HasMaxLength(100);

                entity.Property(e => e.PaymentRef).HasMaxLength(100);

                entity.Property(e => e.Remark).HasMaxLength(1000);

                entity.Property(e => e.StatusFid).HasColumnName("StatusFID");
            });

            modelBuilder.Entity<YachtCharterings>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BookingDate).HasColumnType("datetime");

                entity.Property(e => e.CancelReason).HasMaxLength(2000);

                entity.Property(e => e.CharterDateFrom).HasColumnType("datetime");

                entity.Property(e => e.CharterDateTo).HasColumnType("datetime");

                entity.Property(e => e.ContactNo).HasMaxLength(20);

                entity.Property(e => e.CultureCode).HasMaxLength(6);

                entity.Property(e => e.CurrencyCode).HasMaxLength(10);

                entity.Property(e => e.CustomerFid).HasColumnName("CustomerFID");

                entity.Property(e => e.CustomerName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.CustomerNote).HasMaxLength(2000);

                entity.Property(e => e.PaymentCurrency).HasMaxLength(10);

                entity.Property(e => e.PaymentExchangeDate).HasColumnType("datetime");

                entity.Property(e => e.PricingDetailFid).HasColumnName("PricingDetailFID");

                entity.Property(e => e.PricingTypeFid).HasColumnName("PricingTypeFID");

                entity.Property(e => e.PricingTypeResKey).HasMaxLength(150);

                entity.Property(e => e.ProcessedDate).HasColumnType("datetime");

                entity.Property(e => e.ProcessedRemark).HasMaxLength(2000);

                entity.Property(e => e.ReservationEmail)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.SpecialRequestDescriptions).HasMaxLength(2000);

                entity.Property(e => e.StatusFid).HasColumnName("StatusFID");

                entity.Property(e => e.UniqueId)
                    .IsRequired()
                    .HasColumnName("UniqueID")
                    .HasMaxLength(12);

                entity.Property(e => e.YachtFid).HasColumnName("YachtFID");

                entity.Property(e => e.YachtPortFid).HasColumnName("YachtPortFID");

                entity.Property(e => e.YachtPortName).HasMaxLength(256);
            });

            modelBuilder.Entity<YachtCounters>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.YachtFid).HasColumnName("YachtFID");

                entity.Property(e => e.YachtUniqueId)
                    .IsRequired()
                    .HasColumnName("YachtUniqueID")
                    .HasMaxLength(12);
            });

            modelBuilder.Entity<YachtDestinationPlanDetails>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DestinationPlanFid).HasColumnName("DestinationPlanFID");

                entity.Property(e => e.FileStreamFid).HasColumnName("FileStreamFID");

                entity.Property(e => e.FileTypeFid).HasColumnName("FileTypeFID");

                entity.Property(e => e.LanguageFid).HasColumnName("LanguageFID");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(456);
            });

            modelBuilder.Entity<YachtDestinationPlans>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DefaultDestinationName).HasMaxLength(456);

                entity.Property(e => e.EffectiveEndDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveStartDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.YachtFid).HasColumnName("YachtFID");
            });

            modelBuilder.Entity<YachtFileStreams>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActivatedDate).HasColumnType("datetime");

                entity.Property(e => e.FileStreamFid).HasColumnName("FileStreamFID");

                entity.Property(e => e.FileTypeFid).HasColumnName("FileTypeFID");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.YachtFid).HasColumnName("YachtFID");
            });

            modelBuilder.Entity<YachtInformationDetails>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActivatedDate).HasColumnType("datetime");

                entity.Property(e => e.FileStreamFid).HasColumnName("FileStreamFID");

                entity.Property(e => e.FileTypeFid).HasColumnName("FileTypeFID");

                entity.Property(e => e.InformationFid).HasColumnName("InformationFID");

                entity.Property(e => e.LanguageFid).HasColumnName("LanguageFID");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ShortDescriptions).HasMaxLength(1000);

                entity.Property(e => e.Title).HasMaxLength(456);

                entity.Property(e => e.UniqueId)
                    .IsRequired()
                    .HasColumnName("UniqueID")
                    .HasMaxLength(12);
            });

            modelBuilder.Entity<YachtInformations>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActivatedDate).HasColumnType("datetime");

                entity.Property(e => e.DefaultTitle).HasMaxLength(456);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Remark).HasMaxLength(2000);

                entity.Property(e => e.UniqueId)
                    .IsRequired()
                    .HasColumnName("UniqueID")
                    .HasMaxLength(12);

                entity.Property(e => e.YachtFid).HasColumnName("YachtFID");
            });

            modelBuilder.Entity<YachtMerchantAgreements>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ContractedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveEndDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveStartDate).HasColumnType("datetime");

                entity.Property(e => e.FileStreamFid).HasColumnName("FileStreamFID");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.MerchantFid).HasColumnName("MerchantFID");

                entity.Property(e => e.Remark).HasMaxLength(2000);

                entity.Property(e => e.UniqueId)
                    .IsRequired()
                    .HasColumnName("UniqueID")
                    .HasMaxLength(12);
            });

            modelBuilder.Entity<YachtMerchantAqmgts>(entity =>
            {
                entity.ToTable("YachtMerchantAQMgts");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AqadminUserFid).HasColumnName("AQAdminUserFID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveEndDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveStartDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.MerchantFid).HasColumnName("MerchantFID");

                entity.Property(e => e.MerchantName).HasMaxLength(100);

                entity.Property(e => e.Remark).HasMaxLength(1000);

                entity.Property(e => e.UserEmail).HasMaxLength(100);

                entity.Property(e => e.UserName).HasMaxLength(100);
            });

            modelBuilder.Entity<YachtMerchantUsers>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveEndDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveStartDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.MerchantFid).HasColumnName("MerchantFID");

                entity.Property(e => e.MerchantName).HasMaxLength(100);

                entity.Property(e => e.UserEmail).HasMaxLength(100);

                entity.Property(e => e.UserFid).HasColumnName("UserFID");

                entity.Property(e => e.UserName).HasMaxLength(100);
            });

            modelBuilder.Entity<YachtMerchants>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AccountSize).HasDefaultValueSql("((5))");

                entity.Property(e => e.Address1).HasMaxLength(456);

                entity.Property(e => e.Address2).HasMaxLength(456);

                entity.Property(e => e.City).HasMaxLength(256);

                entity.Property(e => e.ContactNumber1).HasMaxLength(50);

                entity.Property(e => e.ContactNumber2).HasMaxLength(50);

                entity.Property(e => e.Country).HasMaxLength(256);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EmailAddress1).HasMaxLength(100);

                entity.Property(e => e.EmailAddress2).HasMaxLength(100);

                entity.Property(e => e.ExpiredDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.MerchantName).HasMaxLength(456);

                entity.Property(e => e.Remark).HasMaxLength(2000);

                entity.Property(e => e.State).HasMaxLength(456);

                entity.Property(e => e.UniqueId)
                    .IsRequired()
                    .HasColumnName("UniqueID")
                    .HasMaxLength(12);

                entity.Property(e => e.ZipCode).HasMaxLength(10);

                entity.Property(e => e.ZoneFid).HasColumnName("ZoneFID");
            });

            modelBuilder.Entity<YachtNonOperationDays>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Remark).HasMaxLength(2000);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.YachtFid).HasColumnName("YachtFID");
            });

            modelBuilder.Entity<YachtOtherInformations>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActivatedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.FileStreamFid).HasColumnName("FileStreamFID");

                entity.Property(e => e.FileTypeFid).HasColumnName("FileTypeFID");

                entity.Property(e => e.InfoTypeFid).HasColumnName("InfoTypeFID");

                entity.Property(e => e.LanguageFid).HasColumnName("LanguageFID");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Title).HasMaxLength(256);

                entity.Property(e => e.UniqueId)
                    .IsRequired()
                    .HasColumnName("UniqueID")
                    .HasMaxLength(12);

                entity.Property(e => e.YachtFid).HasColumnName("YachtFID");
            });

            modelBuilder.Entity<YachtPorts>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.PortFid).HasColumnName("PortFID");

                entity.Property(e => e.PortName).HasMaxLength(256);

                entity.Property(e => e.PortTypeFid).HasColumnName("PortTypeFID");

                entity.Property(e => e.PortTypeResKey).HasMaxLength(150);

                entity.Property(e => e.Remark).HasMaxLength(2000);

                entity.Property(e => e.YachtFid).HasColumnName("YachtFID");
            });

            modelBuilder.Entity<YachtPricingPlanDetails>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CultureCode).HasMaxLength(6);

                entity.Property(e => e.CurrencyCode).HasMaxLength(10);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.PricingPlanFid).HasColumnName("PricingPlanFID");

                entity.Property(e => e.PricingTypeFid).HasColumnName("PricingTypeFID");

                entity.Property(e => e.PricingTypeResKey).HasMaxLength(150);
            });

            modelBuilder.Entity<YachtPricingPlanInformations>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.LanguageFid).HasColumnName("LanguageFID");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.PricingPlanFid).HasColumnName("PricingPlanFID");

                entity.Property(e => e.Remark).HasMaxLength(2000);
            });

            modelBuilder.Entity<YachtPricingPlans>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveEndDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.PlanName).HasMaxLength(456);

                entity.Property(e => e.PricingCategoryFid).HasColumnName("PricingCategoryFID");

                entity.Property(e => e.PricingCategoryResKey).HasMaxLength(150);

                entity.Property(e => e.Remark).HasMaxLength(2000);

                entity.Property(e => e.UniqueId)
                    .IsRequired()
                    .HasColumnName("UniqueID")
                    .HasMaxLength(12);

                entity.Property(e => e.YachtFid).HasColumnName("YachtFID");

                entity.Property(e => e.YachtPortFid).HasColumnName("YachtPortFID");

                entity.Property(e => e.YachtPortName).HasMaxLength(256);

                entity.HasMany(e => e.yachtPricingDetail).WithOne(e => e.PricingPlan).HasForeignKey(e => e.PricingPlanFid);
            });

            modelBuilder.Entity<Yachts>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Ac).HasColumnName("AC");

                entity.Property(e => e.Acsurcharge).HasColumnName("ACSurcharge");

                entity.Property(e => e.BuilderName).HasMaxLength(256);

                entity.Property(e => e.CharterCategoryFid).HasColumnName("CharterCategoryFID");

                entity.Property(e => e.CharterCategoryResKey).HasMaxLength(150);

                entity.Property(e => e.CharterTypeFid).HasColumnName("CharterTypeFID");

                entity.Property(e => e.CharterTypeResKey).HasMaxLength(150);

                entity.Property(e => e.City).HasMaxLength(100);

                entity.Property(e => e.Country).HasMaxLength(100);

                entity.Property(e => e.CountryRegistered).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DesignerName).HasMaxLength(256);

                entity.Property(e => e.DestinationTypeFid).HasColumnName("DestinationTypeFID");

                entity.Property(e => e.DestinationTypeResKey).HasMaxLength(150);

                entity.Property(e => e.Electricity).HasMaxLength(256);

                entity.Property(e => e.EngineGenerators).HasMaxLength(256);

                entity.Property(e => e.HullTypeFid).HasColumnName("HullTypeFID");

                entity.Property(e => e.HullTypeResKey).HasMaxLength(150);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Material).HasMaxLength(256);

                entity.Property(e => e.MerchantFid).HasColumnName("MerchantFID");

                entity.Property(e => e.MoreDetailNote).HasMaxLength(2000);

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.Refrigerator).HasMaxLength(256);

                entity.Property(e => e.Stabilisers).HasMaxLength(256);

                entity.Property(e => e.UniqueID)
                    .IsRequired()
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.YachtTypeFid).HasColumnName("YachtTypeFID");

                entity.Property(e => e.YachtTypeResKey).HasMaxLength(150);

                entity.Property(e => e.Year).HasMaxLength(100);
            });

            #region relationship creating

            modelBuilder.Entity<YachtCounters>()
                .HasOne(k => k.Yacht)
                .WithOne(k => k.Counters)
                .HasForeignKey<YachtCounters>(k => k.YachtFid);

            modelBuilder.Entity<YachtAttributeValues>()
                .HasOne(k => k.Yacht)
                .WithMany(k => k.AttributeValues)
                .HasForeignKey(k => k.YachtFid);

            modelBuilder.Entity<YachtCharterings>()
                .HasOne(k => k.Yacht)
                .WithMany(k => k.Charterings)
                .HasForeignKey(k => k.YachtFid);

            modelBuilder.Entity<YachtDestinationPlans>()
                .HasOne(k => k.Yacht)
                .WithMany(k => k.DestinationPlans)
                .HasForeignKey(k => k.YachtFid);

            modelBuilder.Entity<YachtFileStreams>()
                .HasOne(k => k.Yacht)
                .WithMany(k => k.FileStreams)
                .HasForeignKey(k => k.YachtFid);

            modelBuilder.Entity<YachtInformations>()
                .HasOne(k => k.Yacht)
                .WithMany(k => k.Informations)
                .HasForeignKey(k => k.YachtFid);

            modelBuilder.Entity<YachtOtherInformations>()
                .HasOne(k => k.Yacht)
                .WithMany(k => k.OtherInformations)
                .HasForeignKey(k => k.YachtFid);

            modelBuilder.Entity<YachtNonOperationDays>()
                .HasOne(k => k.Yacht)
                .WithMany(k => k.NonOperationDays)
                .HasForeignKey(k => k.YachtFid);

            modelBuilder.Entity<YachtPorts>()
                .HasOne(k => k.Yacht)
                .WithMany(k => k.Ports)
                .HasForeignKey(k => k.YachtFid);

            #endregion relationship creating

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}