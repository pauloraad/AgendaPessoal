using Agenda.Client.Pages.Contatos;
using Agenda.Server.Models.Agenda;
using System.Net.Http.Json;

namespace Agenda.Client.Services
{
    public class CompromissoService
    {
        private readonly HttpClient _httpClient;

        public CompromissoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> CreateCompromisso(
            Compromisso compromisso)
        {
            return await _httpClient.PostAsJsonAsync(
                "compromisso/CreateCompromisso",
                compromisso);
        }

        public async Task<HttpResponseMessage> DeleteCompromisso(
            int id)
        {
            return await _httpClient.DeleteAsync(
                $"compromisso/DeleteCompromisso/{id}");
        }

        public async Task<Compromisso> GetCompromisso(
            int id)
        {
            return await _httpClient.GetFromJsonAsync<Compromisso>(
                $"compromisso/GetCompromisso/{id}");
        }

        public async Task<IEnumerable<Compromisso>> GetCompromissos()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Compromisso>>(
                "compromisso/GetCompromissos");
        }

        public async Task<IEnumerable<Compromisso>> GetCompromissos(
            Compromisso compromisso)
        {
            if (compromisso == null)
            {
                return null;
            }

            var response = await _httpClient.PostAsJsonAsync(
                "compromisso/GetCompromissos",
                compromisso);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<Compromisso>>();
            }

            return null;
        }


        public async Task<HttpResponseMessage> UpdateCompromisso(
            int id,
            Compromisso compromisso)
        {
            return await _httpClient.PutAsJsonAsync(
                $"compromisso/UpdateCompromisso/{id}",
                compromisso);
        }
    }
}
