using Agenda.Server.Data;
using Agenda.Server.Models.Agenda;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Agenda.Server.Controllers
{
    [Route("compromisso/[action]")]
    public class CompromissoController : Controller
    {
        private readonly AgendaContext _context;

        public CompromissoController(
            AgendaContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompromisso(
            [FromBody] Compromisso compromisso)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Compromisso.Add(compromisso);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCompromissos), new 
            { 
                id = compromisso.IdCompromisso 
            }, 
            compromisso);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompromisso(
            int id)
        {
            var compromisso = await _context.Compromisso.FindAsync(id);

            if (compromisso == null)
            {
                return NotFound();
            }

            _context.Compromisso.Remove(compromisso);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{id}")]
        public IActionResult GetCompromisso(
            int id)
        {
            var compromisso = _context.Compromisso.FirstOrDefault(c => c.IdCompromisso == id);

            if (compromisso == null)
            {
                return NotFound();
            }

            return Ok(compromisso);
        }

        [HttpPost]
        public IActionResult GetCompromissos(
            [FromBody] Compromisso contato)
        {
            if (contato == null)
            {
                return BadRequest();
            }

            var compromissoFiltrado = _context.Compromisso.AsEnumerable().Where(c => GetWhereContatoFiltrado(contato, c)).ToList();

            var compromissosEncontrados = compromissoFiltrado.AsEnumerable().Any()
                ? compromissoFiltrado
                : _context.Compromisso.AsEnumerable();

            if (compromissosEncontrados == null)
            {
                return NotFound();
            }

            return Ok(compromissosEncontrados);
        }

        private static bool GetWhereContatoFiltrado(
            Compromisso contato, 
            Compromisso c)
        {
            return c.Titulo.Contains(contato.Titulo)
                || c.Contato.NomeCompleto.Contains(contato.Contato.NomeCompleto)
                || c.Descricao.Contains(contato.Descricao);
        }

        [HttpGet]
        public IEnumerable<Compromisso> GetCompromissos()
        {
            return _context.Compromisso.Include(c => c.Contato);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompromisso(
            int id, 
            [FromBody] Compromisso updatedCompromisso)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != updatedCompromisso.IdCompromisso)
            {
                return BadRequest();
            }

            var existingCompromisso = await _context.Compromisso.FindAsync(id);

            if (existingCompromisso == null)
            {
                return NotFound();
            }

            existingCompromisso.DataCompromisso = updatedCompromisso.DataCompromisso;
            existingCompromisso.HorarioCompromisso = updatedCompromisso.HorarioCompromisso;
            existingCompromisso.Titulo = updatedCompromisso.Titulo;
            existingCompromisso.Descricao = updatedCompromisso.Descricao;
            existingCompromisso.FkIdContato = updatedCompromisso.FkIdContato;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}