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
	/// Gestione dati tavoli
	/// </summary>
	/// <param name="tavoloService"></param>
	[ApiController]
	[Route("[controller]")]
	public class TavoloController(TavoloService tavoloService) : Controller
	{
		private readonly TavoloService _tavoloService = tavoloService;

		/// <summary>
		/// Inserimento nuovo tavolo
		/// </summary>
		/// <param name="add"></param>
		/// <returns>Un messaggio di conferma dell'operazione</returns>
		/// <response code="201">Tavolo inserito con successo</response>
		/// <response code="400">Richiesta malformata</response>
		/// <response code="500">Errore interno al server</response>

		[HttpPost]
		public IActionResult Add(APITaConAdd add)
		{
			try
			{
				Tavolo tavoloDaAggiungere = new(_tavoloService.GetNextId(), add.NumeroPosti, add.Posizione);

				if (_tavoloService.Add(tavoloDaAggiungere))
				{
					return StatusCode(StatusCodes.Status201Created, "Tavolo inserito con successo");
				}
				else
				{
					return BadRequest("Impossibile inserire il tavolo. Verificare i dati e riprovare.");
				}
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
			}
		}

		/// <summary>
		/// Cancellazione tavolo
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
				if (_tavoloService.Delete(id))
				{
					return Ok("Tavolo cancellato con successo");
				}
				else
				{
					return BadRequest("Impossibile cancellare il tavolo. Verificare i dati e riprovare.");
				}
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
			}
		}

		/// <summary>
		/// Restituisce la lista di tutti i tavoli
		/// </summary>
		/// <returns>Vedi sommario</returns>
		/// <response code="200">OK</response>
		/// <response code="500">Errore interno al server</response>
		[HttpGet]
		public IActionResult Get()
		{
			try
			{
				IEnumerable<Tavolo> tavoli = _tavoloService.Get();

				return Ok(tavoli);
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
			}
		}

		/// <summary>
		/// Restituisce un tavolo in base all'id
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Vedi sommario</returns>
		/// <response code="200">OK</response>
		/// <response code="404">Tavolo non trovato</response>
		/// <response code="500">Errore interno al server</response>
		[HttpGet("{id}")]
		public IActionResult Get([FromRoute] uint id)
		{
			try
			{
				Tavolo? tavoloDaOttenere = _tavoloService.Get(id);
				if (tavoloDaOttenere is not null)
				{
					return Ok(tavoloDaOttenere);
				}
				else
				{
					return NotFound("Tavolo non trovato");
				}
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
			}
		}

		/// <summary>
		/// Aggiornamento tavolo
		/// </summary>
		/// <param name="tavoloDaAggiornare"></param>
		/// <returns>Un messaggio di conferma dell'operazione</returns>
		/// <response code="200">Tavolo aggiornato con successo</response>
		/// <response code="400">Richiesta malformata</response>
		/// <response code="500">Errore interno al server</response>
		[HttpPut]
		public IActionResult Update([FromBody] Tavolo tavoloDaAggiornare)
		{
			if (tavoloDaAggiornare is null)
			{
				return BadRequest("Dati del tavolo non validi.");
			}
			try
			{
				if (_tavoloService.Update(tavoloDaAggiornare))
				{
					return Ok("Tavolo aggiornato con successo");
				}
				else
				{
					return BadRequest("Impossibile aggiornare il tavolo. Verificare i dati e riprovare.");
				}
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
			}
		}
	}
}