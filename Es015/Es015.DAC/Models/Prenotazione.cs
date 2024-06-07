using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Es016.DAL.Models
{
    public class Prenotazione (uint id, uint idTavolo, uint idCliente, DateTime dataEOraArrivo)
    {
        public uint Id { get; } = id;

        [Display(Name = "Id Tavolo")]
        [Required]
        [Range(1, uint.MaxValue)]

		public uint IdTavolo { get; set; } = idTavolo;

        [Display(Name = "Id Cliente")]
        [Required]
		[Range(1, uint.MaxValue)]
		public uint IdCliente { get; set; } = idCliente;

		[Display(Name = "Data e ora arrivo")]
		[Required]
		[DataType(DataType.DateTime)]
		public DateTime DataEOraArrivo { get; set; } = dataEOraArrivo;
	}
}