using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Es016.DAL.Models
{
    public class Tavolo(uint id, uint numeroPosti, string posizione)
    {
		public uint Id { get; } = id;

		[Display(Name = "Numero posti")]
		[Required]
		[Range(1, uint.MaxValue)]
		public uint NumeroPosti { get; set; } = numeroPosti;

		[Display(Name = "Posizione")]
		[Required]
		[StringLength(8, ErrorMessage = "La lunghezza della {0} non può essere maggiore di {1}")]
		public string? Posizione { get; set; } = posizione;
	}
}