using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiDokument.Models
{
    public class DokumentVersion
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid DokumentId { get; set; }

        [Required]
        public int NumerWersji { get; set; }

        [Required]
        public string SciezkaPliku { get; set; } = string.Empty;

        [Required]
        public string TypPliku { get; set; } = string.Empty;

        public DateTime DataDodania { get; set; } = DateTime.Now;

        public string? Opis { get; set; }

        [ForeignKey("DokumentId")]
        public Dokument? Dokument { get; set; }
    }
}