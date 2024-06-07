using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es016.DAL.Models.AdHoc
{
	public class APICliConAdd(string nome, string cognome, DateTime? dataNascita = null, string? email = null, string? telefono = null)
	{
		[Required]
		public string Nome { get; } = nome;
		[Required]
		public string Cognome { get; } = cognome;
		public DateTime? DataNascita { get; } = dataNascita;
		public string? Email { get; } = email;
		public string? Telefono { get; } = telefono;
	}
}