using System;
using System.ComponentModel.DataAnnotations;

namespace ApiDokument.Models
{
    public class Dokument
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Nazwa jest wymagana")]
        [MaxLength(200)]
        public string Nazwa { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string? Opis { get; set; }

        public Guid? PokojId { get; set; }

        public Guid? PrzedmiotId { get; set; }

        [MaxLength(500)]
        public string? LokalizacjaFizyczna { get; set; }

        [MaxLength(500)]
        public string? SciezkaPliku { get; set; }

        [MaxLength(50)]
        public string? TypPliku { get; set; }
    }
}