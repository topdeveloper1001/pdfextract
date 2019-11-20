using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ExtractPDF.Models
{
    public partial class PDFDBContext : DbContext
    {
        public PDFDBContext()
        {
        }

        public PDFDBContext(DbContextOptions<PDFDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Article> Article { get; set; }
        public virtual DbSet<ArticleModifier> ArticleModifier { get; set; }
        public virtual DbSet<ArticleModifierContent> ArticleModifierContent { get; set; }
        public virtual DbSet<Association> Association { get; set; }
        public virtual DbSet<AssociationType> AssociationType { get; set; }
        public virtual DbSet<AssociationTypeContent> AssociationTypeContent { get; set; }
        public virtual DbSet<AssociationValue> AssociationValue { get; set; }
        public virtual DbSet<AssociationValueContent> AssociationValueContent { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=PDFDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_ArticleParentId_Article");
            });

            modelBuilder.Entity<ArticleModifier>(entity =>
            {
                entity.HasOne(d => d.Article)
                    .WithMany(p => p.ArticleModifier)
                    .HasForeignKey(d => d.ArticleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ArticleModifier_ArticleId");

                entity.HasOne(d => d.ArticleModifierContent)
                    .WithMany(p => p.ArticleModifier)
                    .HasForeignKey(d => d.ArticleModifierContentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ArticleModifier_ArticleModifierContentId");
            });

            modelBuilder.Entity<ArticleModifierContent>(entity =>
            {
                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Association>(entity =>
            {
                entity.HasOne(d => d.Article)
                    .WithMany(p => p.Association)
                    .HasForeignKey(d => d.ArticleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Association_ArticleId");

                entity.HasOne(d => d.AssociationType)
                    .WithMany(p => p.Association)
                    .HasForeignKey(d => d.AssociationTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Association_AssociationTypeId");

                entity.HasOne(d => d.AssociationValue)
                    .WithMany(p => p.Association)
                    .HasForeignKey(d => d.AssociationValueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Association_AssociationValueId");
            });

            modelBuilder.Entity<AssociationType>(entity =>
            {
                entity.HasOne(d => d.AssociationTypeContent)
                    .WithMany(p => p.AssociationType)
                    .HasForeignKey(d => d.AssociationTypeContentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AssociationType_AssociationTypeContentId");
            });

            modelBuilder.Entity<AssociationTypeContent>(entity =>
            {
                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<AssociationValue>(entity =>
            {
                entity.HasOne(d => d.AssociationValueContent)
                    .WithMany(p => p.AssociationValue)
                    .HasForeignKey(d => d.AssociationValueContentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AssociationValue_AssociationValueContentId");
            });

            modelBuilder.Entity<AssociationValueContent>(entity =>
            {
                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(100);
            });
        }
    }
}
