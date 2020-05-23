using Microsoft.EntityFrameworkCore;

namespace YachtMerchant.Infrastructure.Database.Entities
{
    public partial class YachtOperatorDbContext_Backup : DbContext
    {
        public YachtOperatorDbContext_Backup()
        {
        }

        public YachtOperatorDbContext_Backup(DbContextOptions<YachtOperatorDbContext_Backup> options)
            : base(options)
        {
        }

        public virtual DbSet<PortLocations> PortLocations { get; set; }
        public virtual DbSet<YachtAdditionalServiceControls> YachtAdditionalServiceControls { get; set; }
        public virtual DbSet<YachtAdditionalServiceDetails> YachtAdditionalServiceDetails { get; set; }
        public virtual DbSet<YachtAdditionalServices> YachtAdditionalServices { get; set; }
        public virtual DbSet<YachtAttributeValues> YachtAttributeValues { get; set; }
        public virtual DbSet<YachtAttributes> YachtAttributes { get; set; }
        public virtual DbSet<YachtCharteringDetails> YachtCharteringDetails { get; set; }
        public virtual DbSet<YachtCharteringPaymentLogs> YachtCharteringPaymentLogs { get; set; }
        public virtual DbSet<YachtCharteringProcessingFees> YachtCharteringProcessingFees { get; set; }
        public virtual DbSet<YachtCharteringSchedules> YachtCharteringSchedules { get; set; }
        public virtual DbSet<YachtCharterings> YachtCharterings { get; set; }
        public virtual DbSet<YachtCounters> YachtCounters { get; set; }
        public virtual DbSet<YachtExternalRefLinks> YachtExternalRefLinks { get; set; }
        public virtual DbSet<YachtFileStreams> YachtFileStreams { get; set; }
        public virtual DbSet<YachtInformationDetails> YachtInformationDetails { get; set; }
        public virtual DbSet<YachtInformations> YachtInformations { get; set; }
        public virtual DbSet<YachtMerchantAgreements> YachtMerchantAgreements { get; set; }
        public virtual DbSet<YachtMerchantAqmgts> YachtMerchantAqmgts { get; set; }
        public virtual DbSet<YachtMerchantCharterFeeOptions> YachtMerchantCharterFeeOptions { get; set; }
        public virtual DbSet<YachtMerchantFileStreams> YachtMerchantFileStreams { get; set; }
        public virtual DbSet<YachtMerchantInformationDetails> YachtMerchantInformationDetails { get; set; }
        public virtual DbSet<YachtMerchantInformations> YachtMerchantInformations { get; set; }
        public virtual DbSet<YachtMerchantProductInventories> YachtMerchantProductInventories { get; set; }
        public virtual DbSet<YachtMerchantProductPricings> YachtMerchantProductPricings { get; set; }
        public virtual DbSet<YachtMerchantProductSuppliers> YachtMerchantProductSuppliers { get; set; }
        public virtual DbSet<YachtMerchantProductVendors> YachtMerchantProductVendors { get; set; }
        public virtual DbSet<YachtMerchantUsers> YachtMerchantUsers { get; set; }
        public virtual DbSet<YachtMerchants> YachtMerchants { get; set; }
        public virtual DbSet<YachtNonOperationDays> YachtNonOperationDays { get; set; }
        public virtual DbSet<YachtOptions> YachtOptions { get; set; }
        public virtual DbSet<YachtOtherInformations> YachtOtherInformations { get; set; }
        public virtual DbSet<YachtPorts> YachtPorts { get; set; }
        public virtual DbSet<YachtPricingPlanDetails> YachtPricingPlanDetails { get; set; }
        public virtual DbSet<YachtPricingPlanInformations> YachtPricingPlanInformations { get; set; }
        public virtual DbSet<YachtPricingPlans> YachtPricingPlans { get; set; }
        public virtual DbSet<YachtTourAttributeValues> YachtTourAttributeValues { get; set; }
        public virtual DbSet<YachtTourAttributes> YachtTourAttributes { get; set; }
        public virtual DbSet<YachtTourCharterDetails> YachtTourCharterDetails { get; set; }
        public virtual DbSet<YachtTourCharterPaymentLogs> YachtTourCharterPaymentLogs { get; set; }
        public virtual DbSet<YachtTourCharterProcessingFees> YachtTourCharterProcessingFees { get; set; }
        public virtual DbSet<YachtTourCharterSchedules> YachtTourCharterSchedules { get; set; }
        public virtual DbSet<YachtTourCharters> YachtTourCharters { get; set; }
        public virtual DbSet<YachtTourCounters> YachtTourCounters { get; set; }
        public virtual DbSet<YachtTourExternalRefLinks> YachtTourExternalRefLinks { get; set; }
        public virtual DbSet<YachtTourFileStreams> YachtTourFileStreams { get; set; }
        public virtual DbSet<YachtTourInformationDetails> YachtTourInformationDetails { get; set; }
        public virtual DbSet<YachtTourInformations> YachtTourInformations { get; set; }
        public virtual DbSet<YachtTourNonOperationDays> YachtTourNonOperationDays { get; set; }
        public virtual DbSet<YachtTourOperationDetails> YachtTourOperationDetails { get; set; }
        public virtual DbSet<YachtTourPricings> YachtTourPricings { get; set; }
        public virtual DbSet<YachtTours> YachtTours { get; set; }
        public virtual DbSet<Yachts> Yachts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

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

            modelBuilder.Entity<YachtAdditionalServiceControls>(entity =>
            {
                entity.HasKey(e => new { e.AdditionalServiceFid, e.YachtFid })
                    .HasName("PK__YachtAdd__2B65AF62AD241566");

                entity.Property(e => e.AdditionalServiceFid).HasColumnName("AdditionalServiceFID");

                entity.Property(e => e.YachtFid).HasColumnName("YachtFID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveEndDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Remark).HasMaxLength(2000);
            });

            modelBuilder.Entity<YachtAdditionalServiceDetails>(entity =>
            {
                entity.HasKey(e => new { e.AdditionalServiceFid, e.ProductFid })
                    .HasName("PK__YachtAdd__90A8393C94643549");

                entity.Property(e => e.AdditionalServiceFid).HasColumnName("AdditionalServiceFID");

                entity.Property(e => e.ProductFid).HasColumnName("ProductFID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveEndDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Remark).HasMaxLength(2000);
            });

            modelBuilder.Entity<YachtAdditionalServices>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActiveFrom).HasColumnType("datetime");

                entity.Property(e => e.ActiveTo).HasColumnType("datetime");

                entity.Property(e => e.AdditonalServiceTypeFid).HasColumnName("AdditonalServiceTypeFID");

                entity.Property(e => e.AdditonalServiceTypeResKey).HasMaxLength(150);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.MerchantFid).HasColumnName("MerchantFID");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.Remark).HasMaxLength(2000);

                entity.Property(e => e.UniqueId)
                    .IsRequired()
                    .HasColumnName("UniqueID")
                    .HasMaxLength(12)
                    .IsUnicode(false);
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

            modelBuilder.Entity<YachtCharteringDetails>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CharteringFid).HasColumnName("CharteringFID");

                entity.Property(e => e.CultureCode).HasMaxLength(6);

                entity.Property(e => e.CurrencyCode).HasMaxLength(10);

                entity.Property(e => e.ItemName).HasMaxLength(456);

                entity.Property(e => e.ItemTypeFid).HasColumnName("ItemTypeFID");

                entity.Property(e => e.ItemTypeResKey).HasMaxLength(150);

                entity.Property(e => e.RefFid).HasColumnName("RefFID");

                entity.Property(e => e.Remark).HasMaxLength(2000);

                entity.Property(e => e.YachtFid).HasColumnName("YachtFID");
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

            modelBuilder.Entity<YachtCharteringProcessingFees>(entity =>
            {
                entity.HasKey(e => e.CharteringId)
                    .HasName("PK__YachtCha__C627D3C057D1023C");

                entity.Property(e => e.CharteringId)
                    .HasColumnName("CharteringID")
                    .ValueGeneratedNever();

                entity.Property(e => e.GeneratedDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Remark).HasMaxLength(2000);
            });

            modelBuilder.Entity<YachtCharteringSchedules>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AssignedDate).HasColumnType("datetime");

                entity.Property(e => e.CharteringFid).HasColumnName("CharteringFID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveEndDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveStartDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Remark).HasMaxLength(2000);

                entity.Property(e => e.RoleFid).HasColumnName("RoleFID");

                entity.Property(e => e.RoleResKey).HasMaxLength(150);

                entity.Property(e => e.UserFid).HasColumnName("UserFID");

                entity.Property(e => e.YachtFid).HasColumnName("YachtFID");
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

                entity.Property(e => e.PricingTypeFid).HasColumnName("PricingTypeFID");

                entity.Property(e => e.PricingTypeResKey).HasMaxLength(150);

                entity.Property(e => e.ProcessedDate).HasColumnType("datetime");

                entity.Property(e => e.ProcessedRemark).HasMaxLength(2000);

                entity.Property(e => e.ReservationEmail)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.SourceFid)
                    .HasColumnName("SourceFID")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.SourceResKey).HasMaxLength(150);

                entity.Property(e => e.SpecialRequestDescriptions).HasMaxLength(2000);

                entity.Property(e => e.StatusFid).HasColumnName("StatusFID");

                entity.Property(e => e.StatusResKey).HasMaxLength(150);

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
                entity.HasKey(e => e.YachtId)
                    .HasName("PK__YachtCou__0EE60D3386E9828E");

                entity.Property(e => e.YachtId)
                    .HasColumnName("YachtID")
                    .ValueGeneratedNever();

                entity.Property(e => e.YachtUniqueId)
                    .IsRequired()
                    .HasColumnName("YachtUniqueID")
                    .HasMaxLength(12);
            });

            modelBuilder.Entity<YachtExternalRefLinks>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveEndDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.LinkTypeFid).HasColumnName("LinkTypeFID");

                entity.Property(e => e.LinkTypeResKey).HasMaxLength(150);

                entity.Property(e => e.Name).HasMaxLength(456);

                entity.Property(e => e.Remark).HasMaxLength(2000);

                entity.Property(e => e.UrlLink).HasMaxLength(256);

                entity.Property(e => e.YachtFid).HasColumnName("YachtFID");
            });

            modelBuilder.Entity<YachtFileStreams>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActivatedDate).HasColumnType("datetime");

                entity.Property(e => e.FileCategoryFid).HasColumnName("FileCategoryFID");

                entity.Property(e => e.FileCategoryResKey).HasMaxLength(150);

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

            modelBuilder.Entity<YachtMerchantCharterFeeOptions>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BookingFeeOptionFid).HasColumnName("BookingFeeOptionFID");

                entity.Property(e => e.BookingFeeOptionResKey).HasMaxLength(150);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveEndDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveStartDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.MerchantFid).HasColumnName("MerchantFID");

                entity.Property(e => e.Remark).HasMaxLength(1000);

                entity.Property(e => e.SchemeBasedOptionFid).HasColumnName("SchemeBasedOptionFID");

                entity.Property(e => e.SchemeBasedOptionResKey).HasMaxLength(150);
            });

            modelBuilder.Entity<YachtMerchantFileStreams>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActivatedDate).HasColumnType("datetime");

                entity.Property(e => e.FileStreamFid).HasColumnName("FileStreamFID");

                entity.Property(e => e.FileTypeFid).HasColumnName("FileTypeFID");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.MerchantFid).HasColumnName("MerchantFID");
            });

            modelBuilder.Entity<YachtMerchantInformationDetails>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActivatedDate).HasColumnType("datetime");

                entity.Property(e => e.FileStreamFid).HasColumnName("FileStreamFID");

                entity.Property(e => e.FileTypeFid).HasColumnName("FileTypeFID");

                entity.Property(e => e.InformationFid).HasColumnName("InformationFID");

                entity.Property(e => e.LanguageFid)
                    .HasColumnName("LanguageFID")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ShortDescriptions).HasMaxLength(1500);

                entity.Property(e => e.Title).HasMaxLength(456);

                entity.Property(e => e.UniqueId)
                    .IsRequired()
                    .HasColumnName("UniqueID")
                    .HasMaxLength(12);
            });

            modelBuilder.Entity<YachtMerchantInformations>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActivatedDate).HasColumnType("datetime");

                entity.Property(e => e.DefaultTitle).HasMaxLength(456);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.MerchantFid).HasColumnName("MerchantFID");

                entity.Property(e => e.Remark).HasMaxLength(2000);

                entity.Property(e => e.UniqueId)
                    .IsRequired()
                    .HasColumnName("UniqueID")
                    .HasMaxLength(12);
            });

            modelBuilder.Entity<YachtMerchantProductInventories>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.GsttypeFid).HasColumnName("GSTTypeFID");

                entity.Property(e => e.GsttypeResKey)
                    .HasColumnName("GSTTypeResKey")
                    .HasMaxLength(150);

                entity.Property(e => e.ItemUnitFid).HasColumnName("ItemUnitFID");

                entity.Property(e => e.ItemUnitResKey).HasMaxLength(150);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.MerchantFid).HasColumnName("MerchantFID");

                entity.Property(e => e.PriceTypeFid).HasColumnName("PriceTypeFID");

                entity.Property(e => e.PriceTypeResKey).HasMaxLength(150);

                entity.Property(e => e.ProductCategoryFid).HasColumnName("ProductCategoryFID");

                entity.Property(e => e.ProductCategoryResKey).HasMaxLength(150);

                entity.Property(e => e.ProductCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(456);

                entity.Property(e => e.UniqueId)
                    .IsRequired()
                    .HasColumnName("UniqueID")
                    .HasMaxLength(12)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<YachtMerchantProductPricings>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CultureCode).HasMaxLength(6);

                entity.Property(e => e.CurrencyCode).HasMaxLength(10);

                entity.Property(e => e.EffectiveDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveEndDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ProductFid).HasColumnName("ProductFID");
            });

            modelBuilder.Entity<YachtMerchantProductSuppliers>(entity =>
            {
                entity.HasKey(e => new { e.ProductFid, e.VendorFid })
                    .HasName("PK__YachtMer__1D4A7138FADCCACB");

                entity.Property(e => e.ProductFid).HasColumnName("ProductFID");

                entity.Property(e => e.VendorFid).HasColumnName("VendorFID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveEndDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Remark).HasMaxLength(2000);
            });

            modelBuilder.Entity<YachtMerchantProductVendors>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address).HasMaxLength(450);

                entity.Property(e => e.ContactNo).HasMaxLength(20);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.MerchantFid).HasColumnName("MerchantFID");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.ProductCategoryFid).HasColumnName("ProductCategoryFID");

                entity.Property(e => e.ProductCategoryResKey).HasMaxLength(150);

                entity.Property(e => e.Remark).HasMaxLength(2000);

                entity.Property(e => e.UniqueId)
                    .IsRequired()
                    .HasColumnName("UniqueID")
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.VendorTypeFid).HasColumnName("VendorTypeFID");

                entity.Property(e => e.VendorTypeResKey).HasMaxLength(150);
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

                entity.Property(e => e.LandingPageOptionFid).HasColumnName("LandingPageOptionFID");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.MerchantName).HasMaxLength(456);

                entity.Property(e => e.MerchantTypeFid)
                    .HasColumnName("MerchantTypeFID")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.MerchantTypeResKey).HasMaxLength(150);

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

            modelBuilder.Entity<YachtOptions>(entity =>
            {
                entity.HasKey(e => e.YachtId)
                    .HasName("PK__YachtOpt__0EE60D33C2E5B62D");

                entity.Property(e => e.YachtId)
                    .HasColumnName("YachtID")
                    .ValueGeneratedNever();

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.YachtUniqueId)
                    .IsRequired()
                    .HasColumnName("YachtUniqueID")
                    .HasMaxLength(12);
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
            });

            modelBuilder.Entity<YachtTourAttributeValues>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AttributeCategoryFid).HasColumnName("AttributeCategoryFID");

                entity.Property(e => e.AttributeFid).HasColumnName("AttributeFID");

                entity.Property(e => e.AttributeValue).HasMaxLength(256);

                entity.Property(e => e.EffectiveDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.YachtTourFid).HasColumnName("YachtTourFID");
            });

            modelBuilder.Entity<YachtTourAttributes>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AttributeCategoryFid).HasColumnName("AttributeCategoryFID");

                entity.Property(e => e.AttributeName)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.IconCssClass).HasMaxLength(50);

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

            modelBuilder.Entity<YachtTourCharterDetails>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CultureCode).HasMaxLength(6);

                entity.Property(e => e.CurrencyCode).HasMaxLength(10);

                entity.Property(e => e.ItemName).HasMaxLength(456);

                entity.Property(e => e.ItemTypeFid).HasColumnName("ItemTypeFID");

                entity.Property(e => e.ItemTypeResKey).HasMaxLength(150);

                entity.Property(e => e.RefFid).HasColumnName("RefFID");

                entity.Property(e => e.Remark).HasMaxLength(2000);

                entity.Property(e => e.TourCharterFid).HasColumnName("TourCharterFID");
            });

            modelBuilder.Entity<YachtTourCharterPaymentLogs>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CultureCode).HasMaxLength(6);

                entity.Property(e => e.CurrencyCode).HasMaxLength(10);

                entity.Property(e => e.PaymentBy).HasMaxLength(100);

                entity.Property(e => e.PaymentDate).HasColumnType("datetime");

                entity.Property(e => e.PaymentMethod).HasMaxLength(100);

                entity.Property(e => e.PaymentRef).HasMaxLength(100);

                entity.Property(e => e.Remark).HasMaxLength(1000);

                entity.Property(e => e.StatusFid).HasColumnName("StatusFID");

                entity.Property(e => e.TourCharterFid).HasColumnName("TourCharterFID");
            });

            modelBuilder.Entity<YachtTourCharterProcessingFees>(entity =>
            {
                entity.HasKey(e => e.TourCharterFid)
                    .HasName("PK__YachtTou__B52D7057EF474901");

                entity.Property(e => e.TourCharterFid)
                    .HasColumnName("TourCharterFID")
                    .ValueGeneratedNever();

                entity.Property(e => e.GeneratedDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Remark).HasMaxLength(2000);
            });

            modelBuilder.Entity<YachtTourCharterSchedules>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AssignedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveEndDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveStartDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Remark).HasMaxLength(2000);

                entity.Property(e => e.RoleFid).HasColumnName("RoleFID");

                entity.Property(e => e.RoleResKey).HasMaxLength(150);

                entity.Property(e => e.TourCharterFid).HasColumnName("TourCharterFID");

                entity.Property(e => e.UserFid).HasColumnName("UserFID");

                entity.Property(e => e.YachtFid).HasColumnName("YachtFID");
            });

            modelBuilder.Entity<YachtTourCharters>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BookingDate).HasColumnType("datetime");

                entity.Property(e => e.CancelReason).HasMaxLength(2000);

                entity.Property(e => e.ContactNo).HasMaxLength(20);

                entity.Property(e => e.CultureCode).HasMaxLength(6);

                entity.Property(e => e.CurrencyCode).HasMaxLength(10);

                entity.Property(e => e.CustomerFid).HasColumnName("CustomerFID");

                entity.Property(e => e.CustomerName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.CustomerNote).HasMaxLength(2000);

                entity.Property(e => e.DateFrom).HasColumnType("datetime");

                entity.Property(e => e.DateTo).HasColumnType("datetime");

                entity.Property(e => e.PaymentCurrency).HasMaxLength(10);

                entity.Property(e => e.PaymentExchangeDate).HasColumnType("datetime");

                entity.Property(e => e.ProcessedDate).HasColumnType("datetime");

                entity.Property(e => e.ProcessedRemark).HasMaxLength(2000);

                entity.Property(e => e.ReservationEmail)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.SourceFid)
                    .HasColumnName("SourceFID")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.SourceResKey).HasMaxLength(150);

                entity.Property(e => e.SpecialRequestDescriptions).HasMaxLength(2000);

                entity.Property(e => e.StatusFid).HasColumnName("StatusFID");

                entity.Property(e => e.UniqueId)
                    .IsRequired()
                    .HasColumnName("UniqueID")
                    .HasMaxLength(12);

                entity.Property(e => e.YachtFid).HasColumnName("YachtFID");

                entity.Property(e => e.YachtPortFid).HasColumnName("YachtPortFID");

                entity.Property(e => e.YachtPortName).HasMaxLength(256);

                entity.Property(e => e.YachtTourFid).HasColumnName("YachtTourFID");
            });

            modelBuilder.Entity<YachtTourCounters>(entity =>
            {
                entity.HasKey(e => e.YachtTourId)
                    .HasName("PK__YachtTou__9827ACDEBF0F21AA");

                entity.Property(e => e.YachtTourId)
                    .HasColumnName("YachtTourID")
                    .ValueGeneratedNever();

                entity.Property(e => e.YachtTourUniqueId)
                    .IsRequired()
                    .HasColumnName("YachtTourUniqueID")
                    .HasMaxLength(12);
            });

            modelBuilder.Entity<YachtTourExternalRefLinks>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveEndDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.LinkTypeFid).HasColumnName("LinkTypeFID");

                entity.Property(e => e.LinkTypeResKey).HasMaxLength(150);

                entity.Property(e => e.Name).HasMaxLength(456);

                entity.Property(e => e.Remark).HasMaxLength(2000);

                entity.Property(e => e.UrlLink).HasMaxLength(256);

                entity.Property(e => e.YachtTourFid).HasColumnName("YachtTourFID");
            });

            modelBuilder.Entity<YachtTourFileStreams>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActivatedDate).HasColumnType("datetime");

                entity.Property(e => e.FileCategoryFid).HasColumnName("FileCategoryFID");

                entity.Property(e => e.FileCategoryResKey).HasMaxLength(150);

                entity.Property(e => e.FileStreamFid).HasColumnName("FileStreamFID");

                entity.Property(e => e.FileTypeFid).HasColumnName("FileTypeFID");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.YachtTourFid).HasColumnName("YachtTourFID");
            });

            modelBuilder.Entity<YachtTourInformationDetails>(entity =>
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

            modelBuilder.Entity<YachtTourInformations>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActivatedDate).HasColumnType("datetime");

                entity.Property(e => e.DefaultTitle).HasMaxLength(456);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Remark).HasMaxLength(2000);

                entity.Property(e => e.TourFid).HasColumnName("TourFID");

                entity.Property(e => e.TourInformationTypeFid).HasColumnName("TourInformationTypeFID");

                entity.Property(e => e.TourInformationTypeResKey).HasMaxLength(150);

                entity.Property(e => e.UniqueId)
                    .IsRequired()
                    .HasColumnName("UniqueID")
                    .HasMaxLength(12);
            });

            modelBuilder.Entity<YachtTourNonOperationDays>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Remark).HasMaxLength(2000);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.YachtFid).HasColumnName("YachtFID");

                entity.Property(e => e.YachtTourFid).HasColumnName("YachtTourFID");
            });

            modelBuilder.Entity<YachtTourOperationDetails>(entity =>
            {
                entity.HasKey(e => new { e.TourFid, e.YachtFid })
                    .HasName("PK__YachtTou__D00EF26B02C38AD0");

                entity.Property(e => e.TourFid).HasColumnName("TourFID");

                entity.Property(e => e.YachtFid).HasColumnName("YachtFID");

                entity.Property(e => e.EffectiveDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveEndDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.MerchantFid).HasColumnName("MerchantFID");

                entity.Property(e => e.Remark).HasMaxLength(2000);
            });

            modelBuilder.Entity<YachtTourPricings>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveEndDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Remark).HasMaxLength(2000);

                entity.Property(e => e.TourFid).HasColumnName("TourFID");

                entity.Property(e => e.TourPricingTypeFid).HasColumnName("TourPricingTypeFID");

                entity.Property(e => e.TourPricingTypeResKey).HasMaxLength(150);

                entity.Property(e => e.YachtFid).HasColumnName("YachtFID");
            });

            modelBuilder.Entity<YachtTours>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.City).HasMaxLength(100);

                entity.Property(e => e.CityFid).HasColumnName("CityFID");

                entity.Property(e => e.Country).HasMaxLength(100);

                entity.Property(e => e.CountryFid).HasColumnName("CountryFID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveEndDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.LocationFid).HasColumnName("LocationFID");

                entity.Property(e => e.LocationName).HasMaxLength(100);

                entity.Property(e => e.MerchantFid).HasColumnName("MerchantFID");

                entity.Property(e => e.Remark).HasMaxLength(200);

                entity.Property(e => e.TourCategoryFid).HasColumnName("TourCategoryFID");

                entity.Property(e => e.TourCategoryResKey).HasMaxLength(150);

                entity.Property(e => e.TourDurationUnitResKey).HasMaxLength(150);

                entity.Property(e => e.TourDurationUnitTypeFid).HasColumnName("TourDurationUnitTypeFID");

                entity.Property(e => e.TourName).HasMaxLength(456);

                entity.Property(e => e.UniqueId)
                    .IsRequired()
                    .HasColumnName("UniqueID")
                    .HasMaxLength(12)
                    .IsUnicode(false);
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

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.OperationTypeFid)
                    .HasColumnName("OperationTypeFID")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.OperationTypeResKey).HasMaxLength(150);

                entity.Property(e => e.Refrigerator).HasMaxLength(256);

                entity.Property(e => e.Stabilisers).HasMaxLength(256);

                entity.Property(e => e.UniqueId)
                    .IsRequired()
                    .HasColumnName("UniqueID")
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.YachtTypeFid).HasColumnName("YachtTypeFID");

                entity.Property(e => e.YachtTypeResKey).HasMaxLength(150);

                entity.Property(e => e.Year).HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}