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
        public virtual DbSet<Association> Association { get; set; }
        public virtual DbSet<ContentValue> ContentValue { get; set; }

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
                entity.HasOne(d => d.ArticleContent)
                    .WithMany(p => p.ArticleArticleContent)
                    .HasForeignKey(d => d.ArticleContentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ArticleContentId_ContentValue");

                entity.HasOne(d => d.ArticleExtensionContent)
                    .WithMany(p => p.ArticleArticleExtensionContent)
                    .HasForeignKey(d => d.ArticleExtensionContentId)
                    .HasConstraintName("FK_ArticleExtensionContentId_ContentValue");

                entity.HasOne(d => d.ArticleModifierContent)
                    .WithMany(p => p.ArticleArticleModifierContent)
                    .HasForeignKey(d => d.ArticleModifierContentId)
                    .HasConstraintName("FK_ArticleModifierContentId_ContentValue");
            });

            modelBuilder.Entity<Association>(entity =>
            {
                entity.HasOne(d => d.Article)
                    .WithMany(p => p.Association)
                    .HasForeignKey(d => d.ArticleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AssociationArticleId_Article");

                entity.HasOne(d => d.AssociationTypeContent)
                    .WithMany(p => p.AssociationAssociationTypeContent)
                    .HasForeignKey(d => d.AssociationTypeContentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AssociationTypeContentId_ContentValue");

                entity.HasOne(d => d.AssociationValueContent)
                    .WithMany(p => p.AssociationAssociationValueContent)
                    .HasForeignKey(d => d.AssociationValueContentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AssociationValueContentId_ContentValue");

                entity.HasOne(d => d.AssociationValueModifierContent)
                    .WithMany(p => p.AssociationAssociationValueModifierContent)
                    .HasForeignKey(d => d.AssociationValueModifierContentId)
                    .HasConstraintName("FK_AssociationValueModifierContentId_ContentValue");
            });

            modelBuilder.Entity<ContentValue>(entity =>
            {
                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(100);
            });
        }
    }
}
