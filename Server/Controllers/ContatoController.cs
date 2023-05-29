using Agenda.Server.Data;
using Agenda.Server.Models.Agenda;
using Microsoft.AspNetCore.Mvc;

namespace Agenda.Server.Controllers
{
    [Route("contato/[action]")]
    public class ContatoController : Controller
    {
        private readonly AgendaContext _context;

        public ContatoController(
            AgendaContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult CreateContato(
            [FromBody] Contato dadosContato)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (dadosContato == null)
                {
                    return BadRequest();
                }

                _context.Contato.Add(dadosContato);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetContato), new { id = dadosContato.IdContato }, dadosContato);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteContato(
            int id)
        {
            var contato = _context.Contato.FirstOrDefault(c => c.IdContato == id);

            if (contato == null)
            {
                return NotFound();
            }

            _context.Contato.Remove(contato);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpGet("{id}")]
        public IActionResult GetContato(
            int id)
        {
            var contato = _context.Contato.FirstOrDefault(c => c.IdContato == id);

            if (contato == null)
            {
                return NotFound();
            }

            return Ok(contato);
        }

        [HttpGet("{nome}")]
        public IActionResult GetContatosPorNome(
            string nome)
        {
            if (string.IsNullOrEmpty(nome))
            {
                return Ok(_context.Contato.ToList());
            }

            var contatos = _context.Contato.AsEnumerable().Where(c => c.NomeCompleto.ToLower().Contains(nome.ToLower())).ToList();

            if (contatos.Any())
            {
                return Ok(contatos);
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult GetContato(
            [FromBody] Contato contato)
        {
            if (contato == null)
            {
                return BadRequest();
            }

            var contatosFiltrados = _context.Contato.AsEnumerable().Where(c => GetWhereContatoFiltrado(contato, c)).ToList();

            var contatosEncontrados = contatosFiltrados.AsEnumerable().Any()
                ? contatosFiltrados
                : _context.Contato.AsEnumerable();

            if (contatosEncontrados == null)
            {
                return NotFound();
            }

            return Ok(contatosEncontrados);
        }

        private static bool GetWhereContatoFiltrado(
            Contato contato, 
            Contato c)
        {
            return c.Bairro.ToLower().Contains(contato.Bairro.ToLower()) ||
                   c.Cep.ToLower().Contains(contato.Cep.ToLower()) ||
                   c.Cidade.ToLower().Contains(contato.Cidade.ToLower()) ||
                   c.Complemento.ToLower().Contains(contato.Complemento.ToLower()) ||
                   c.Email.ToLower().Contains(contato.Email.ToLower()) ||
                   c.Estado.ToLower().Contains(contato.Estado.ToLower()) ||
                   c.NomeCompleto.ToLower().Contains(contato.NomeCompleto.ToLower()) ||
                   c.RuaAvenida.ToLower().Contains(contato.RuaAvenida.ToLower()) ||
                   c.Telefone1.ToLower().Contains(contato.Telefone1.ToLower()) ||
                   c.Telefone2.ToLower().Contains(contato.Telefone2.ToLower());
        }

        [HttpGet]
        public IActionResult GetContatos()
        {
            var contatos = _context.Contato.ToList();
            return Ok(contatos);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateContato(
            int id, 
            [FromBody] Contato dadosContato)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (dadosContato == null || dadosContato.IdContato != id)
                {
                    return BadRequest();
                }

                var contato = _context.Contato.FirstOrDefault(c => c.IdContato == id);

                if (contato == null)
                {
                    return NotFound();
                }

                PopularUpdateContato(
                    dadosContato, 
                    contato);

                _context.SaveChanges();

                return NoContent();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        private static void PopularUpdateContato(
            Contato dadosContato, 
            Contato contato)
        {
            contato.NomeCompleto = dadosContato.NomeCompleto;
            contato.Telefone1 = dadosContato.Telefone1;
            contato.Telefone2 = dadosContato.Telefone2;
            contato.Email = dadosContato.Email;
            contato.DataNascimento = dadosContato.DataNascimento;
            contato.Cep = dadosContato.Cep;
            contato.RuaAvenida = dadosContato.RuaAvenida;
            contato.Numero = dadosContato.Numero;
            contato.Complemento = dadosContato.Complemento;
            contato.Bairro = dadosContato.Bairro;
            contato.Cidade = dadosContato.Cidade;
            contato.Estado = dadosContato.Estado;
        }
    }
}