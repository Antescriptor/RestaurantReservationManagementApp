using Es016.BLL.Utils;
using Es016.DAL.Models;
using Es016.DAL.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;

namespace Es016.BLL.Services
{
	public class PrenotazioneService(PrenotazioneStore prenotazioneStore, ClienteService clienteService, TavoloService tavoloService)
	{
		private readonly PrenotazioneStore _prenotazioneStore = prenotazioneStore;
		private readonly ClienteService _clienteService = clienteService;
		private readonly TavoloService _tavoloService = tavoloService;

		public List<Tavolo>? GetAvailable(uint numeroPostiRichiesti, DateTime dataEOraArrivoDaPrenotare)
		{
			List<Tavolo> tavoli = _tavoloService.Get();
			if (tavoli.Count < 1) return null;

			List<Cliente> clienti = _clienteService.Get();
			if (clienti.Count < 1) return tavoli;

			List<Prenotazione> prenotazioni = _prenotazioneStore.Get();

			List<Tavolo> tavoliDisponibili;

			if (prenotazioni is null || prenotazioni.Count < 1)
			{
				tavoliDisponibili = tavoli.Where(t => t.NumeroPosti >= numeroPostiRichiesti && dataEOraArrivoDaPrenotare >= DateTime.Now).ToList();
				return tavoliDisponibili;
			}
			else
			{
				var tavoliSenzaPrenotazioni =
					from tavolo in tavoli
					where
						!prenotazioni.Any(p => p.IdTavolo == tavolo.Id) &&
						tavolo.NumeroPosti >= numeroPostiRichiesti &&
						dataEOraArrivoDaPrenotare >= DateTime.Now
					select tavolo;

				var tavoliConPrenotrazioniEFiltrati =
					from tavolo in tavoli
					join prenotazione in prenotazioni
					on tavolo.Id equals prenotazione.IdTavolo
					where
						tavolo.NumeroPosti >= numeroPostiRichiesti &&
						(dataEOraArrivoDaPrenotare >= prenotazione.DataEOraArrivo.AddHours(2) || dataEOraArrivoDaPrenotare.AddHours(2) <= prenotazione.DataEOraArrivo) &&
						dataEOraArrivoDaPrenotare >= DateTime.Now

					select tavolo;

				/* Query LINQ equivalenti
				var tavoliSenzaPrenotazioni = tavoli
					.Where(tavolo => !prenotazioni.Any(p => p.IdTavolo == tavolo.Id));

				var tavoliConPrenotrazioniEFiltrati = tavoli
					.Join(prenotazioni, tavolo => tavolo.Id, prenotazione => prenotazione.IdTavolo,
						(tavolo, prenotazione) => new { Tavolo = tavolo, Prenotazione = prenotazione })
					.Where(tp => tp.Tavolo.NumeroPosti >= numeroPostiRichiesti &&
						(dataEOraArrivoDaPrenotare >= tp.Prenotazione.DataEOraArrivo.AddHours(2) ||
						dataEOraArrivoDaPrenotare.AddHours(2) <= tp.Prenotazione.DataEOraArrivo))
					.Select(tp => tp.Tavolo);
				*/

				tavoliDisponibili = tavoliSenzaPrenotazioni.Union(tavoliConPrenotrazioniEFiltrati).ToList();
			}


			if (tavoliDisponibili.Count < 1)
			{
				return null;
			}
			else
			{
				return tavoliDisponibili;
			}
		}
		public bool AddFirstAvailable(uint numeroPostiRichiesti, DateTime dataEOraArrivoDaPrenotare, uint? idCliente = null, string? nome = null, string? cognome = null, DateTime? dataNascita = null, string? email = null, string? telefono = null)
		{
			List<Tavolo>? tavoliDisponibili = GetAvailable(numeroPostiRichiesti, dataEOraArrivoDaPrenotare);
			if (tavoliDisponibili is null || tavoliDisponibili.Count < 1) return false;
			uint idTavolo = tavoliDisponibili.First().Id;

			List<Cliente> clienti = _clienteService.Get();
			if (clienti.Count < 1) return false;
			List<uint> idClienti = clienti.Select(clienti => clienti.Id).ToList();

			if (idCliente is null)
			{
				if (nome is null || cognome is null) return false;
				uint idClienteDaAggiungere = _clienteService.GetNextId();
				Cliente clienteDaAggiungere = new(idClienteDaAggiungere, nome, cognome, dataNascita, email, telefono);
				if (_clienteService.Add(clienteDaAggiungere))
				{
					idCliente = idClienteDaAggiungere;
				}
				else
				{
					return false;
				}
			}
			else if (!idClienti.Contains((uint)idCliente))
			{
				return false;
			}

			uint id = GetNextId();

			Prenotazione prenotazioneDaAggiungere = new(id, idTavolo, (uint)idCliente, dataEOraArrivoDaPrenotare);

			if (Add(prenotazioneDaAggiungere))
			{
				return true;
			}
			else
			{
				return false;
			}

		}


		public bool Add(Prenotazione prenotazione)
		{
			return _prenotazioneStore.Add(prenotazione);
		}
		public bool Delete(uint id)
		{
			return _prenotazioneStore.Delete(id);
		}
		public List<Prenotazione> Get()
		{
			return _prenotazioneStore.Get();
		}
		public Prenotazione Get(uint id)
		{
			Prenotazione? prenotazioneOttenuta = _prenotazioneStore.Get(id);
			if (prenotazioneOttenuta is not null)
			{
				return prenotazioneOttenuta;
			}
			else
			{
				throw new Exception("Prenotazione non presente");
			}
		}
		public uint GetNextId()
		{
			List<Prenotazione> prenotazioni = Get();
			if (prenotazioni.Count == 0)
			{
				return 1;
			}
			else
			{
				return prenotazioni.Last().Id + 1;
			}
		}
		public List<Prenotazione>? Search(uint? idTavolo, uint? idCliente, DateTime? dataEOraArrivo)
		{
			List<Prenotazione> prenotazioni = _prenotazioneStore.Get();

			List<Prenotazione>? prenotazioniTrovate = prenotazioni
				.Where(p =>
					(idTavolo is not null) ? p.IdTavolo == idTavolo : true &&
					(idCliente is not null) ? p.IdCliente == idCliente : true &&
					(dataEOraArrivo is not null) ? p.DataEOraArrivo == dataEOraArrivo : true)
				?.ToList();

			return prenotazioniTrovate;
		}
		public bool Update(Prenotazione prenotazione)
		{
			return _prenotazioneStore.Update(prenotazione);
		}
	}
}