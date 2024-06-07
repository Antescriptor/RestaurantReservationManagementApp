using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es016.DAL.Models.AdHoc
{
	public class APIPreConGetAvail(uint numeroPostiRichiesti, DateTime dataEOraArrivoDaPrenotare)
	{
		[Required]
		public uint NumeroPostiRichiesti { get; } = numeroPostiRichiesti;
		[Required]
		public DateTime DataEOraArrivoDaPrenotare { get; } = dataEOraArrivoDaPrenotare;
	}
}
