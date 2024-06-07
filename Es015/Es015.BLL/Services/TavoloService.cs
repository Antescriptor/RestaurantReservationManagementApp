using Es016.DAL.Models;
using Es016.DAL.Stores;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Es016.BLL.Services
{
	public class TavoloService(TavoloStore tavoloStore)
    {
		private readonly TavoloStore _tavoloStore = tavoloStore;
		public bool Add(uint numeroPosti, string posizione)
		{
			uint id = GetNextId();
			Tavolo tavolo = new(id, numeroPosti, posizione);
			return Add(tavolo);
		}
		public bool Add(Tavolo tavolo)
		{
			return _tavoloStore.Add(tavolo);
		}
		public bool Delete(uint id)
		{
			return _tavoloStore.Delete(id);
		}
		public List<Tavolo> Get()
		{
			return _tavoloStore.Get();
		}
		public Tavolo Get(uint id)
		{
			Tavolo? tavoloOttenuto = _tavoloStore.Get(id);
			if (tavoloOttenuto is not null)
			{
				return tavoloOttenuto;
			}
			else
			{
				throw new Exception("Tavolo non presente");
			}
		}
		public uint GetNextId()
		{
			List<Tavolo> tavolo = Get();
			if (tavolo.Count == 0)
			{
				return 1;
			}
			else
			{
				return tavolo.Last().Id + 1;
			}
		}
		public List<Tavolo>? Search(uint? numeroPersone, string? posizione)
		{
			List<Tavolo> tavoli = _tavoloStore.Get();

			List<Tavolo>? clientiTrovati = tavoli
				.Where(t =>
					(numeroPersone is not null) ? t.NumeroPosti == numeroPersone : true &&
					(posizione is not null) ? t.Posizione == posizione : true)
				?.ToList();

			return clientiTrovati;
		}
		public bool Update(Tavolo tavolo)
		{
			return _tavoloStore.Update(tavolo);
		}
	}
}