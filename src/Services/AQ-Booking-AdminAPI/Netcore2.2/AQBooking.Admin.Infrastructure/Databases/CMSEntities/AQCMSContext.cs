using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AQBooking.Admin.Infrastructure.Databases.CMSEntities
{
    public partial class AQCMSContext : DbContext
    {
        public AQCMSContext()
        {
        }

        public AQCMSContext(DbContextOptions<AQCMSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<PostDetails> PostDetails { get; set; }
        public virtual DbSet<PostFileStreams> PostFileStreams { get; set; }
        public virtual DbSet<Posts> Posts { get; set; }
        public virtual DbSet<Subscribers> Subscribers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=103.97.125.19;Database=AQ_CMS;User ID=aqbooking_user;Password=AQUser@123#;Integrated Security=False;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity<PostDetails>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActivatedDate).HasColumnType("datetime");

                entity.Property(e => e.FileStreamFid).HasColumnName("FileStreamFID");

                entity.Property(e => e.FileTypeFid).HasColumnName("FileTypeFID");

                entity.Property(e => e.KeyWord).HasMaxLength(255);

                entity.Property(e => e.LanguageFid).HasColumnName("LanguageFID");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.MetaDescription).HasMaxLength(255);

                entity.Property(e => e.PostFid).HasColumnName("PostFId");

                entity.Property(e => e.Title).HasMaxLength(255);

                entity.Property(e => e.UniqueId)
                    .IsRequired()
                    .HasMaxLength(12);
            });

            modelBuilder.Entity<PostFileStreams>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActivatedDate).HasColumnType("datetime");

                entity.Property(e => e.FileCategoryFid).HasColumnName("FileCategoryFID");

                entity.Property(e => e.FileCategoryResKey).HasMaxLength(150);

                entity.Property(e => e.FileStreamFid).HasColumnName("FileStreamFID");

                entity.Property(e => e.FileTypeFid).HasColumnName("FileTypeFID");

                entity.Property(e => e.FileTypeResKey).HasMaxLength(150);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.PostFid).HasColumnName("PostFID");
            });

            modelBuilder.Entity<Posts>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DefaultTitle).HasMaxLength(255);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.PostCategoryFid).HasColumnName("PostCategoryFID");

                entity.Property(e => e.PostCategoryResKey).HasMaxLength(150);

                entity.Property(e => e.UniqueId)
                    .IsRequired()
                    .HasMaxLength(12);
            });

            modelBuilder.Entity<Subscribers>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ModuleName).HasMaxLength(255);

                entity.Property(e => e.SourceUrl).HasMaxLength(255);
            });
        }
    }
}
