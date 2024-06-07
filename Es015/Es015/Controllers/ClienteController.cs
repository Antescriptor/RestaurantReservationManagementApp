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
	/// Gestione anagrafica clienti
	/// </summary>
	/// <param name="clienteService"></param>
	[ApiController]
	[Route("[controller]")]
	public class ClienteController(ClienteService clienteService) : Controller
	{
		private readonly ClienteService _clienteService = clienteService;

		/// <summary>
		/// Inserimento nuovo cliente
		/// </summary>
		/// <param name="add"></param>
		/// <returns>Un messaggio di conferma dell'operazione</returns>
		/// <response code="201">Cliente inserito con successo</response>
		/// <response code="400">Richiesta malformata</response>
		/// <response code="500">Errore interno al server</response>
		[HttpPost]
		public IActionResult Add([FromBody] APICliConAdd add)
		{
			try
			{
				Cliente clienteDaInserire = new(_clienteService.GetNextId(), add.Nome, add.Cognome, add.DataNascita, add.Email, add.Telefono);

				if (_clienteService.Add(clienteDaInserire))
				{
					return StatusCode(StatusCodes.Status201Created, "Cliente inserito con successo");
				}
				else
				{
					return BadRequest("Impossibile inserire il cliente. Verificare i dati e riprovare.");
				}
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
			}
		}

		/// <summary>
		/// Cancellazione cliente
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
				if (_clienteService.Delete(id))
				{
					return Ok("Cliente cancellato con successo");
				}
				else
				{
					return BadRequest("Impossibile cancellare il cliente. Verificare i dati e riprovare.");
				}
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
			}
		}

		/// <summary>
		/// Restituisce la lista di tutti i clienti
		/// </summary>
		/// <returns>Vedi sommario</returns>
		/// <response code="200">OK</response>
		/// <response code="500">Errore interno al server</response>
		[HttpGet]
		public IActionResult Get()
		{
			try
			{
				IEnumerable<Cliente> clienti = _clienteService.Get();

				//return StatusCode(StatusCodes.Status200OK, clienti);
				return Ok(clienti);
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
			}
		}

		/// <summary>
		/// Restituisce un cliente in base all'id
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Vedi sommario</returns>
		/// <response code="200">OK</response>
		/// <response code="404">Cliente non trovato</response>
		/// <response code="500">Errore interno al server</response>
		[HttpGet("{id}")]
		public IActionResult Get([FromRoute] uint id)
		{
			try
			{
				Cliente? clienteDaOttenere = _clienteService.Get(id);
				if (clienteDaOttenere is not null)
				{
					return Ok(clienteDaOttenere);
				}
				else
				{
					return NotFound("Cliente non trovato");
				}
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
			}
		}

		/// <summary>
		/// Aggiornamento cliente
		/// </summary>
		/// <param name="clienteDaAggiornare"></param>
		/// <returns>Un messaggio di conferma dell'operazione</returns>
		/// <response code="200">Cliente aggiornato con successo</response>
		/// <response code="400">Richiesta malformata</response>
		/// <response code="500">Errore interno al server</response>
		[HttpPut]
		public IActionResult Update([FromBody] Cliente clienteDaAggiornare)
		{
			if (clienteDaAggiornare is null)
			{
				return BadRequest("Dati del cliente non validi.");
			}
			try
			{
				if (_clienteService.Update(clienteDaAggiornare))
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

/*		/// <summary>
		/// Aggiornamento cliente con parametri in ingresso
		/// </summary>
		/// <param name="id"></param>
		/// <param name="nome"></param>
		/// <param name="cognome"></param>
		/// <param name="dataNascita"></param>
		/// <param name="email"></param>
		/// <param name="telefono"></param>
		/// <returns>Un messaggio di conferma dell'operazione</returns>
		/// <returns Code="200">OK</returns>
		/// <returns Code="400">Richiesta malformata</returns>
		/// <returns Code="500">Errore interno al server</returns>

		[HttpPut("withParameters")]
		public IActionResult Update([FromRoute] uint id, string? nome = null, string? cognome = null, DateOnly? dataNascita = null, string? email = null, string? telefono = null)
		{
			try
			{
				if (_clienteService.Update(id, nome, cognome, dataNascita, email, telefono))
				{
					return Ok("Cliente aggiornato con successo");
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
		}*/
	}
}