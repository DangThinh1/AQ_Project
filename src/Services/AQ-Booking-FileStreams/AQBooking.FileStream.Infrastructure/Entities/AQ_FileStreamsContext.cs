using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AQBooking.FileStream.Infrastructure.Entities
{
    public partial class AQ_FileStreamsContext : DbContext
    {
        public AQ_FileStreamsContext()
        {
        }

        public AQ_FileStreamsContext(DbContextOptions<AQ_FileStreamsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<FileStreamData> FileStreamData { get; set; }
        public virtual DbSet<FileStreamInfo> FileStreamInfo { get; set; }
        public virtual DbSet<FileType> FileType { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<FileStreamData>(entity =>
            {
                entity.HasKey(e => e.FileId);

                entity.Property(e => e.FileId).ValueGeneratedNever();

                entity.Property(e => e.FileData).IsRequired();
            });

            modelBuilder.Entity<FileStreamInfo>(entity =>
            {
                entity.HasKey(e => e.FileId);

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.FileExtentions)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.OriginalName).HasMaxLength(250);

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(e => e.PathThumb12).HasMaxLength(256);

                entity.Property(e => e.PathThumb14).HasMaxLength(256);

                entity.Property(e => e.PathThumb16).HasMaxLength(256);

                entity.Property(e => e.PathThumb18).HasMaxLength(256);

                entity.Property(e => e.Seoname)
                    .HasColumnName("SEOName")
                    .HasMaxLength(500);

                entity.Property(e => e.UniqueId)
                    .IsRequired()
                    .HasColumnName("UniqueID")
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.UploadedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UploadedDateUtc)
                    .HasColumnName("UploadedDateUTC")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<FileType>(entity =>
            {
                entity.Property(e => e.FileTypeName).HasMaxLength(50);
            });
        }
    }
}
