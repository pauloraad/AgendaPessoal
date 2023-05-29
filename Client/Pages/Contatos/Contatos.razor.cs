using Agenda.Client.Services;
using Agenda.Server.Models.Agenda;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using System;

namespace Agenda.Client.Pages.Contatos
{
    public partial class Contatos
    {
        protected Contato _contato { get; set; }

        [Inject]
        public ContatoService _contatoService { get; set; }

        protected IEnumerable<Contato> _contatos { get; set; }

        protected RadzenDataGrid<Contato> _dataGrid { get; set; }

        protected bool _ehEdit { get; set; } = true;

        protected bool _erroVisivel { get; set; }

        [Inject]
        protected NotificationService _notificar { get; set; }

        protected int _count { get; set; }

        [Inject]
        protected DialogService _dialogService { get; set; }

        protected string _pesquisa { get; set; } = "";

        protected async Task ButtonAdicionar(
            MouseEventArgs args)
        {
            _ehEdit = false;
            _contato = new Contato();
            _contato.DataNascimento = DateTime.Now;
        }

        protected async Task ButtonCancelar(
            MouseEventArgs args)
        {

        }

        protected async Task DataGridCarregarDados(
            LoadDataArgs args)
        {
            try
            {
                var contato = GetContatoPesquisa();
                var result = await _contatoService.GetContato(contato);
                _count = result.Count();
                _contatos = result;
            }
            catch (Exception ex)
            {
                _notificar.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Erro", Detail = $"Não foi possível carregar os contatos" });
            }
        }

        protected async Task EditarLinha(
            Contato args)
        {
            _ehEdit = true;
            _contato = args;
        }

        protected async Task FormSubmit()
        {
            try
            {
                var result = await GetFormSubmitResult();
                if (_ehEdit)
                    _notificar.Notify(NotificationSeverity.Success, null, "Contato alterado!");
                else
                    _notificar.Notify(NotificationSeverity.Success, null, "Contato criado com sucesso!");
                await DataGridCarregarDados(null);
            }
            catch (Exception ex)
            {
                _erroVisivel = true;
            }
        }

        private async Task<HttpResponseMessage> GetFormSubmitResult()
        {
            return _ehEdit
                   ? await _contatoService.UpdateContato(
                       _contato.IdContato,
                       _contato)
                   : await _contatoService.CreateContato(
                       _contato);
        }

        protected async Task GridButtonDeletar(
            MouseEventArgs args,
            Contato contato)
        {
            try
            {
                if (await _dialogService.Confirm("Deseja deletar essa informação?", "Confirmar") == true)
                {
                    var deleteResult = await _contatoService.DeleteContato(
                        contato.IdContato);

                    if (deleteResult != null)
                    {
                        await _dataGrid.Reload();
                    }
                }
            }
            catch (Exception ex)
            {
                _notificar.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error",
                    Detail = $"Unable to delete Contato"
                });
            }
        }

        protected async Task Pesquisar(
            ChangeEventArgs args)
        {
            _pesquisa = $"{args.Value}";
            await _dataGrid.GoToPage(0);
            await _dataGrid.Reload();
        }

        private Contato GetContatoPesquisa()
        {
            return new Contato
            {
                Bairro = _pesquisa,
                Cep = _pesquisa,
                Cidade = _pesquisa,
                Complemento = _pesquisa,
                Email = _pesquisa,
                Estado = _pesquisa,
                NomeCompleto = _pesquisa,
                RuaAvenida = _pesquisa,
                Telefone1 = _pesquisa,
                Telefone2 = _pesquisa
            };
        }

        protected async Task Exportar(
            RadzenSplitButtonItem args)
        {
            await _contatoService.ExportarContatosToXML(GetQueryExportarContatos(), "Contatos");
        }

        private Query GetQueryExportarContatos()
        {
            return new Query
            {
                Filter = $@"{(string.IsNullOrEmpty(_dataGrid.Query.Filter) ? "true" : _dataGrid.Query.Filter)}",
                OrderBy = $"{_dataGrid.Query.OrderBy}",
                Expand = "",
                Select = string.Join(",", _dataGrid.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
            };
        }
    }
}