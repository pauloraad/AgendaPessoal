using Agenda.Server.Data;
using Microsoft.AspNetCore.Mvc;

namespace Agenda.Server.Controllers
{
    public class ExportarAgendaController : ExportarController
    {
        private readonly AgendaContext _context;

        public ExportarAgendaController(
            AgendaContext context)
        {
            _context = context;
        }

        [HttpGet("/export/Agenda/contatos/xml")]
        [HttpGet("/export/Agenda/contatos/xml(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportContatosToCSV(string fileName = null)
        {
            return ToXML(AplicarQuery(_context.Contato, Request.Query), fileName);
        }
    }
}
