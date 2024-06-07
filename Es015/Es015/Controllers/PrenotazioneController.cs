using Es016.BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Es016.DAL.Models;
using System.Collections.Generic;
using System;
using Es016.DAL.Models.AdHoc;

namespace Es016.API.Controllers
{
	/// <summary>
	/// Gestione dati prenotazioni
	/// </summary>
	/// <param name="prenotazioneService"></param>
	[ApiController]
	[Route("[controller]")]
	public class PrenotazioneController(PrenotazioneService prenotazioneService) : Controller
	{
		private readonly PrenotazioneService _prenotazioneService = prenotazioneService;

		/// <summary>
		/// Tavoli disponibili in base al numero di posti richiesti e alla data e ora di arrivo
		/// </summary>
		/// <param name="getAvailable"></param>
		/// <returns code="200">OK</returns>
		/// <returns code="404">Nessun tavolo disponibile</returns>
		/// <returns code="500">Errore interno al server</returns>
		[HttpPost("tavoli_disponibili")]
		public IActionResult GetAvailable([FromBody] APIPreConGetAvail getAvailable)
		{
			try
			{
				List<Tavolo>? tavoliDisponibili = _prenotazioneService.GetAvailable(getAvailable.NumeroPostiRichiesti, getAvailable.DataEOraArrivoDaPrenotare);
				if (tavoliDisponibili is not null)
				{
					return Ok(tavoliDisponibili);
				}
				else
				{
					return NotFound("Nessun tavolo disponibile secondo i criteri dati");
				}
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
			}
		}

		/// <summary>
		/// Inserimento nuova prenotazione
		/// </summary>
		/// <param name="add"></param>
		/// <returns>Un messaggio di conferma dell'operazione</returns>
		/// <response code="201">Prenotazione inserita con successo</response>
		/// <response code="400">Richiesta malformata</response>
		/// <response code="500">Errore interno al server</response>
		[HttpPost]
		public IActionResult Add([FromBody] APIPreConAdd add)
		{
			try
			{
				if (_prenotazioneService.AddFirstAvailable(add.NumeroPostiRichiesti, add.DataEOraArrivoDaPrenotare, add.IdCliente, add.Nome, add.Cognome, add.DataNascita, add.Email, add.Telefono))
				{
					return StatusCode(StatusCodes.Status201Created, "Prenotazione inserita con successo");
				}
				else
				{
					return BadRequest("Impossibile inserire la prenotazione. Verificare i dati e riprovare.");
				}
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
			}
		}



		/// <summary>
		/// Inserimento nuova prenotazione senza controlli
		/// </summary>
		/// <param name="prenotazioneDaAggiungere"></param>
		/// <returns>Un messaggio di conferma dell'operazione</returns>
		/// <response code="201">Prenotazione inserita con successo</response>
		/// <response code="400">Richiesta malformata</response>
		/// <response code="500">Errore interno al server</response>
		[HttpPost("inserimento_senza_controlli")]
		public IActionResult Add([FromBody] Prenotazione prenotazioneDaAggiungere)
		{
			if (prenotazioneDaAggiungere is null)
			{
				return BadRequest("Dati della prenotazione non validi.");
			}
			try
			{
				if (_prenotazioneService.Add(prenotazioneDaAggiungere))
				{
					return StatusCode(StatusCodes.Status201Created, "Prenotazione inserita con successo");
				}
				else
				{
					return BadRequest("Impossibile inserire la prenotazione. Verificare i dati e riprovare.");
				}
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
			}
		}

		/// <summary>
		/// Cancellazione prenotazione
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Un messaggio di conferma dell'operazione</returns>
		/// <response code="200">OK</response>
		/// <response code="400">Richiesta malformata</response>
		/// <response code="500">Errore interno al server</response>
		[HttpDelete("{id}")]
		public IActionResult Delete([FromRoute] uint id)
		{
			try
			{
				if (_prenotazioneService.Delete(id))
				{
					return Ok("Prenotazione cancellata con successo");
				}
				else
				{
					return BadRequest("Impossibile cancellare la prenotazione. Verificare i dati e riprovare.");
				}
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
			}
		}

		/// <summary>
		/// "L"ista di tutti le prenotazioni
		/// </summary>
		/// <response code="200">OK</response>
		/// <response code="500">Errore interno al server</response>
		[HttpGet]
		public IActionResult Get()
		{
			try
			{
				IEnumerable<Prenotazione> prenotazioni = _prenotazioneService.Get();

				return Ok(prenotazioni);
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
			}
		}

		/// <summary>
		/// Prenotazione in base all'id
		/// </summary>
		/// <param name="id"></param>
		/// <response code="200">OK</response>
		/// <response code="404">Prenotazione non trovata</response>
		/// <response code="500">Errore interno al server</response>
		[HttpGet("{id}")]
		public IActionResult Get([FromRoute] uint id)
		{
			try
			{
				Prenotazione? prenotazioneDaOttenere = _prenotazioneService.Get(id);
				if (prenotazioneDaOttenere is not null)
				{
					return Ok(prenotazioneDaOttenere);
				}
				else
				{
					return NotFound("Prenotazione non trovata");
				}
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
			}
		}

		/// <summary>
		/// Aggiornamento prenotazione
		/// </summary>
		/// <param name="prenotazioneDaAggiornare"></param>
		/// <returns>Un messaggio di conferma dell'operazione</returns>
		/// <response code="200">Prenotazione aggiornata con successo</response>
		/// <response code="400">Richiesta malformata</response>
		/// <response code="500">Errore interno al server</response>
		[HttpPut]
		public IActionResult Update([FromBody] Prenotazione prenotazioneDaAggiornare)
		{
			if (prenotazioneDaAggiornare is null)
			{
				return BadRequest("Dati del cliente non validi.");
			}
			try
			{
				if (_prenotazioneService.Update(prenotazioneDaAggiornare))
				{
					return StatusCode(200, "Cliente aggiornato con successo");
				}
				else
				{
					return BadRequest("Impossibile aggiornare il cliente. Verificare i dati e riprovare.");
				}
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
			}
		}
	}
}