using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Es016.DAL.Models
{
    public class Cliente(uint id, string nome, string cognome, DateTime? dataNascita = null, string? email = null, string? telefono = null)
    {
        public uint Id { get; } = id;

        [Display(Name = "Nome")]
        [Required]
        [StringLength(64, ErrorMessage = "La lunghezza del {0} non può essere maggiore di {1}")]
        public string Nome { get; set; } = nome;
        [Display(Name = "Cognome")]
		[Required]
		[StringLength(64, ErrorMessage = "La lunghezza del {0} non può essere maggiore di {1}")]
        public string Cognome { get; set; } = cognome;
        public DateTime? DataNascita { get; set; } = dataNascita;

		[Display(Name = "E-mail")]
		[EmailAddress(ErrorMessage = "Indirizzo e-mail invalido")]
        public string? Email { get; set; } = email;

		[Display(Name = "Telefono")]
		[Phone(ErrorMessage = "Numero di telefono invalido")]
		public string? Telefono { get; set; } = telefono;
	}
}