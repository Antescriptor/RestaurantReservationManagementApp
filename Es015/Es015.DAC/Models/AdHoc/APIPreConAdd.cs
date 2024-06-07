using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es016.DAL.Models.AdHoc
{
	public class APIPreConAdd(uint numeroPostiRichiesti, DateTime dataEOraArrivoDaPrenotare, uint? idCliente = null, string? nome = null, string? cognome = null, DateTime? dataNascita = null, string? email = null, string? telefono = null)
	{
		[Required]
		public uint NumeroPostiRichiesti { get; } = numeroPostiRichiesti;
		[Required]
		public DateTime DataEOraArrivoDaPrenotare { get; } = dataEOraArrivoDaPrenotare;
		public uint? IdCliente { get; } = idCliente;
		public string? Nome { get; } = nome;
		public string? Cognome { get; } = cognome;
		public DateTime? DataNascita { get; } = dataNascita;
		public string? Email { get; } = email;
		public string? Telefono { get; } = telefono;
	}
}
