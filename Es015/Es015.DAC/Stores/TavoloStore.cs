using System.Collections.Generic;
using System.Linq;
using Es016.DAL.Models;

namespace Es016.DAL.Stores
{
	public class TavoloStore
	{
		private readonly List<Tavolo> _tavoli = new();
		public bool Add(Tavolo tavolo)
		{
			_tavoli.Add(tavolo);
			return true;
		}

		public bool Delete(uint id)
		{
			Tavolo? tavolo = Get(id);
			if (tavolo is not null)
			{
				_tavoli.Remove(tavolo);
				return true;
			}
			else
			{
				return false;
			}
		}

		public List<Tavolo> Get()
		{
			return _tavoli;
		}

		public Tavolo? Get(uint id)
		{
			return _tavoli.FirstOrDefault(t => t.Id == id);
		}

		public bool Update(Tavolo tavolo)
		{
			Tavolo? tavoloDaAggiornare = _tavoli.FirstOrDefault(t => t.Id == tavolo.Id);
			if (tavoloDaAggiornare is not null)
			{
				tavoloDaAggiornare.NumeroPosti = tavolo.NumeroPosti;
				tavoloDaAggiornare.Posizione = tavolo.Posizione;

				return true;
			}
			else
			{
				return false;
			}
		}
	}
}