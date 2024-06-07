using Es016.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Es016.DAL.Stores
{
	public class PrenotazioneStore
	{
		private readonly List<Prenotazione> _prenotazioni = new();
		public bool Add(Prenotazione prenotazioni)
		{
			_prenotazioni.Add(prenotazioni);
			return true;
		}

		public bool Delete(uint id)
		{
			Prenotazione? prenotazione = Get(id);
			if (prenotazione is not null)
			{
				_prenotazioni.Remove(prenotazione);
				return true;
			}
			else
			{
				return false;
			}
		}

		public List<Prenotazione> Get()
		{
			return _prenotazioni;
		}

		public Prenotazione? Get(uint id)
		{
			return _prenotazioni.FirstOrDefault(p => p.Id == id);
		}

		public bool Update(Prenotazione prenotazione)
		{
			Prenotazione? prenotazioneDaAggiornare = _prenotazioni.FirstOrDefault(p => p.Id == prenotazione.Id);
			if (prenotazioneDaAggiornare is not null)
			{
				prenotazioneDaAggiornare.IdTavolo = prenotazione.IdTavolo;
				prenotazioneDaAggiornare.IdCliente = prenotazione.IdCliente;
				prenotazioneDaAggiornare.DataEOraArrivo = prenotazione.DataEOraArrivo;
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
