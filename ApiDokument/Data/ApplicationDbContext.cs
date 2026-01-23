using ApiDokument.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiDokument.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Dokument> Dokumenty { get; set; }
        public DbSet<DokumentVersion> DocumentVersions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<DokumentVersion>()
                .HasIndex(v => v.DokumentId);

            builder.Entity<DokumentVersion>()
                .HasIndex(v => new { v.DokumentId, v.NumerWersji });
        }
    }
}