using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Identity.Infrastructure.Database.Entities;

namespace Identity.Infrastructure.Database
{
    public class IdentityDbContext : DbContext
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        public DbSet<RoleControls> RoleControls { get; set; }
        public DbSet<UserTokens> UserTokens { get; set; }
        public DbSet<SigninControls> SigninControls { get; set; }
        public DbSet<SSOAuthentication> SSOAuthentication { get; set; }
        

        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {
            Database.EnsureCreated();
            DataMigration();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SSOAuthentication>(entity =>
            {
                entity.ToTable("SSOAuthentication");
                entity.HasKey(k => k.Id);

                entity.Property(k => k.Id)
                .HasColumnName("Id");

                entity.Property(k => k.AuthId)
                .HasColumnName("AuthId");

                entity.Property(k => k.DomainId)
                .HasColumnName("DomainId");

                entity.Property(k => k.Token)
                .HasColumnName("Token");

                entity.Property(k => k.RedirectUrl)
                .HasColumnName("RedirectUrl");

                entity.Property(k => k.Type)
                .HasColumnName("Type");

                entity.Property(k => k.UserUid)
                .HasColumnName("UserUid");
            });

            modelBuilder.Entity<SigninControls>(entity =>
            {
                entity.ToTable("SigninControls");
                entity.HasKey(k => k.Id);

                entity.Property(k => k.Id)
                .HasColumnName("Id");

                entity.Property(k => k.CurrentDomainUid)
                .HasColumnName("CurrentDomainUid");

                entity.Property(k => k.CurrentDomainName)
                .HasColumnName("CurrentDomainName");

                entity.Property(k => k.ToGoDomainUid)
                .HasColumnName("ToGoDomainUid");

                entity.Property(k => k.ToGoDomainName)
                .HasColumnName("ToGoDomainName");

                entity.Property(k => k.ToGoDomainCallbackUrl)
                .HasColumnName("ToGoDomainCallbackUrl");

            });

            modelBuilder.Entity<UserTokens>(entity =>
            {
                entity.ToTable("UserTokens");
                entity.Property(k => k.Id)
                .HasColumnName("Id");
                entity.Property(k => k.AccessToken)
                .HasColumnName("AccessToken");
                entity.Property(k => k.ReturnUrl)
                .HasColumnName("ReturnUrl");
                entity.Property(k => k.UserFid)
                .HasColumnName("UserFid");
                entity.Property(k => k.Deleted)
                .HasColumnName("Deleted");
                entity.HasKey(k => k.Id);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("Users");

                entity.HasKey(k => k.Id);
                entity.Property(k => k.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

                entity.Property(k => k.UserId)
                .HasColumnName("UserId")
                .HasColumnType("uniqueidentifier");

                entity.Property(k => k.UniqueId)
                .HasColumnName("UniqueId")
                .HasMaxLength(12)
                .Metadata.AfterSaveBehavior = Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore;

                entity.Property(k => k.FirstName)
                .HasColumnName("FirstName")
                .HasMaxLength(20);

                entity.Property(k => k.LastName)
                .HasColumnName("LastName")
                .HasMaxLength(20);

                entity.Property(k => k.Designation)
                .HasColumnName("Designation")
                .HasMaxLength(30);

                entity.Property(k => k.MerchantFid)
                .HasColumnName("MerchantFid");

                entity.Property(k => k.DomainFid)
                .HasColumnName("DomainFid");

                entity.Property(k => k.ImageId)
                .HasColumnName("ImageId");

                entity.Property(k => k.LangId)
                .HasColumnName("LangId");

                entity.Property(k => k.Street)
                .HasColumnName("Street")
                .HasMaxLength(40);

                entity.Property(k => k.City)
                .HasColumnName("City")
                .HasMaxLength(20);

                entity.Property(k => k.State)
                .HasColumnName("State")
                .HasMaxLength(20);

                entity.Property(k => k.Country)
                .HasColumnName("Country")
                .HasMaxLength(20);

                entity.Property(k => k.ZipCode)
                .HasColumnName("ZipCode")
                .HasMaxLength(10);

                entity.Property(k => k.Deleted)
                .HasColumnName("Deleted");

                entity.Property(k => k.IsActivated)
                .HasColumnName("IsActivated");

                entity.Property(k => k.RefreshToken)
                .HasColumnName("RefreshToken")
                .HasMaxLength(12);

                entity.Property(k => k.CreatedBy)
                .HasColumnName("CreatedBy")
                .HasColumnType("uniqueidentifier");

                entity.Property(k => k.CreatedDate)
                .HasColumnName("CreatedDate")
                .HasColumnType("datetime");

                entity.Property(k => k.TokenEffectiveDate)
                .HasColumnName("TokenEffectiveDate")
                .HasColumnType("datetime");

                entity.Property(k => k.TokenEffectiveTimeStick)
                .HasColumnName("TokenEffectiveTimeStick")
                .HasColumnType("bigint");

                entity.Property(k => k.DisplayName)
                .IsRequired()
                .HasColumnName("DisplayName")
                .HasMaxLength(30);

                entity.Property(k => k.Title)
               .HasColumnName("Title")
               .HasMaxLength(30);

                entity.Property(k => k.Birthday)
               .HasColumnName("Birthday")
               .HasColumnType("datetime");

                entity.Ignore(k=> k.Roles);
            });

            modelBuilder.Entity<Roles>(entity => {
                entity.ToTable("Roles");
                entity.Ignore(k => k.SubordinateRoles);
                entity.Property(k => k.Id).ValueGeneratedOnAdd();
                entity.HasKey(k => k.Id);
                entity.Property(k => k.Name).HasMaxLength(30);
                entity.Property(k => k.DomainFid).HasColumnName("DomainFid");
                entity.Property(k => k.DomainFid).HasMaxLength(12);
            });

            modelBuilder.Entity<UserRoles>(entity => {
                entity.ToTable("UserRoles");
                entity.HasKey(k=> new { k.UserFid, k.RoleFid});
                entity.HasOne(k => k.User).WithMany(k=>k.UserRoles).HasForeignKey(k=> k.UserFid);
                entity.HasOne(k => k.Role).WithMany(k => k.UserRoles).HasForeignKey(k => k.RoleFid);
            });

            modelBuilder.Entity<RoleControls>(entity=> {
                entity.ToTable("RoleControls");
                entity.HasKey(k => new { k.SuperiorFid, k.SubordinateFid });
                entity.Property(k => k.SuperiorFid).HasColumnName("SuperiorFid");
                entity.Property(k => k.SubordinateFid).HasColumnName("SubordinateFid");
                entity.HasOne(k => k.SubordinateRole).WithMany(k => k.SubordinateRoles).HasForeignKey(k => k.SubordinateFid);
            });
        }

        private void DataMigration() 
        {
            if(Roles.CountAsync().Result == 0)
            {
                var roles = new List<Roles>() {
                new Roles(){
                    Name = "Developer",
                    NormalizedName = "DEVELOPER",
                    DomainFid = "K140Q2XAJAAF"
                },
                new Roles(){
                    Name = "Super Administrator",
                    NormalizedName = "SUPERADMINISTRATOR",
                    DomainFid = "K140Q2XAJAAF"
                },
                new Roles(){
                    Name = "Dining Administrator",
                    NormalizedName = "DININGADMINISTRATOR",
                    DomainFid = "K140Q2XAJAAF"
                },
                new Roles(){
                    Name = "Dining Merchant Manager",
                    NormalizedName = "DININGMERCHANTMANAGER",
                    DomainFid = "K140Q2XAJAAF"
                },
                new Roles(){
                    Name = "Dining Merchant",
                    NormalizedName = "DININGMERCHANT",
                    DomainFid = "K140Q2XAJAAF"
                },
                new Roles(){
                    Name = "Yacht Administrator",
                    NormalizedName = "YACHTADMINISTRATOR",
                    DomainFid = "K140Q2XAJAAF"
                },
                new Roles(){
                    Name = "Yacht Merchant Manager",
                    NormalizedName = "YACHTMERCHANTMANAGER",
                    DomainFid = "K140Q2XAJAAF"
                },
                new Roles(){
                    Name = "Yacht Merchant",
                    NormalizedName = "YACHTMERCHANT",
                    DomainFid = "K140Q2XAJAAF"
                },
            };
                Roles.AddRange(roles);
                SaveChanges();
            }
            if (RoleControls.CountAsync().Result == 0)
            {
                var rolecontrolls = new List<RoleControls>() {
                    new RoleControls(){
                        SuperiorFid = 1,
                        SubordinateFid = 2
                    },
                    new RoleControls(){
                        SuperiorFid = 1,
                        SubordinateFid = 3
                    },
                    new RoleControls(){
                        SuperiorFid = 1,
                        SubordinateFid = 4
                    },
                    new RoleControls(){
                        SuperiorFid = 1,
                        SubordinateFid = 5
                    },new RoleControls(){
                        SuperiorFid = 1,
                        SubordinateFid = 6
                    },new RoleControls(){
                        SuperiorFid = 1,
                        SubordinateFid = 7
                    },
                    new RoleControls(){
                        SuperiorFid = 1,
                        SubordinateFid = 8
                    },
                    new RoleControls(){
                        SuperiorFid = 5,
                        SubordinateFid = 3
                    },
                    new RoleControls(){
                        SuperiorFid = 5,
                        SubordinateFid = 4
                    },
                    new RoleControls(){
                        SuperiorFid = 5,
                        SubordinateFid = 6
                    },
                    new RoleControls(){
                        SuperiorFid = 5,
                        SubordinateFid = 7
                    },
                    new RoleControls(){
                        SuperiorFid = 5,
                        SubordinateFid = 8
                    },
                    new RoleControls(){
                        SuperiorFid = 3,
                        SubordinateFid = 4
                    },
                    new RoleControls(){
                        SuperiorFid = 3,
                        SubordinateFid = 5
                    },
                    new RoleControls(){
                        SuperiorFid = 3,
                        SubordinateFid = 6
                    },
                    new RoleControls(){
                        SuperiorFid = 3,
                        SubordinateFid = 7
                    },

                };
                RoleControls.AddRange(rolecontrolls);
                base.SaveChanges();
            }
        }
    }
}
