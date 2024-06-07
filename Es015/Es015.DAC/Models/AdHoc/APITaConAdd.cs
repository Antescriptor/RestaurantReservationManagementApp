using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Es016.DAL.Models.AdHoc
{
	public class APITaConAdd(uint numeroPosti, string posizione)
	{
		[Required]
		public uint NumeroPosti { get; } = numeroPosti;
		[Required]
		public string Posizione { get; } = posizione;
	}
}