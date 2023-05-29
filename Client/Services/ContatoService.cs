using Agenda.Server.Models.Agenda;
using Microsoft.AspNetCore.Components;
using Radzen;
using System.Net.Http.Json;
using System.Text.Encodings.Web;

namespace Agenda.Client.Services
{
    public class ContatoService
    {
        private readonly HttpClient _httpClient;

        private readonly NavigationManager _navigationManager;

        public ContatoService(
            HttpClient httpClient,
            NavigationManager navigationManager)
        {
            _httpClient = httpClient;
            _navigationManager = navigationManager;
        }

        public async Task<HttpResponseMessage> CreateContato(
            Contato contato)
        {

            return await _httpClient.PostAsJsonAsync(
                "contato/CreateContato",
                contato);
        }

        public async Task<HttpResponseMessage> DeleteContato(
            int id)
        {
            return await _httpClient.DeleteAsync(
                $"contato/DeleteContato/{id}");
        }

        public async Task<Contato> GetContato(
            int id)
        {
            return await _httpClient.GetFromJsonAsync<Contato>(
                $"contato/GetContato/{id}");
        }

        public async Task<IEnumerable<Contato>> GetContatosPorNome(
            string nome)
        {
            if (string.IsNullOrEmpty(nome))
            {
                return await _httpClient.GetFromJsonAsync<IEnumerable<Contato>>(
                    "contato/GetContatos");
            }
            return await _httpClient.GetFromJsonAsync<IEnumerable<Contato>>(
                $"contato/GetContatosPorNome/{nome}");
        }



        public async Task<IEnumerable<Contato>> GetContato(
            Contato contato)
        {
            if (contato == null)
            {
                return null;
            }

            var response = await _httpClient.PostAsJsonAsync(
                "contato/GetContato",
                contato);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<Contato>>();
            }

            return null;
        }

        public async Task<IEnumerable<Contato>> GetContatos()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Contato>>(
                "contato/GetContatos");
        }


        public async Task<HttpResponseMessage> UpdateContato(
            int id,
            Contato contato)
        {
            return await _httpClient.PutAsJsonAsync(
                $"contato/UpdateContato/{id}",
                contato);
        }

        public async System.Threading.Tasks.Task ExportarContatosToXML(
            Query query = null,
            string fileName = null)
        {
            _navigationManager.NavigateTo(
                query != null
                ? query.ToUrl(GetToUrl(fileName))
                : $"export/Agenda/contatos/xml(fileName='{(GetFileName(fileName))}')",
                true);
        }

        private static string GetFileName(
            string fileName)
        {
            return !string.IsNullOrEmpty(fileName)
                ? UrlEncoder.Default.Encode(fileName)
                : "Export";
        }

        private static string GetToUrl(
            string fileName)
        {
            return $"export/Agenda/contatos/xml(fileName='{(!string.IsNullOrEmpty(fileName)
                ? UrlEncoder.Default.Encode(fileName)
                : "Export")}')";
        }
    }
}
