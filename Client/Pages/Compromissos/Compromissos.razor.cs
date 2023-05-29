using Agenda.Client.Services;
using Agenda.Server.Models.Agenda;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;

namespace Agenda.Client.Pages.Compromissos
{
    public partial class Compromissos
    {
        protected Compromisso _compromisso { get; set; }

        protected IEnumerable<Compromisso> _compromissos { get; set; }

        protected bool _ehEdit { get; set; } = true;

        protected IEnumerable<Contato> _contatos { get; set; }

        protected int _contatoSelecionadoCount { get; set; }

        protected Contato _contatoSelecionado { get; set; }

        [Inject]
        protected ContatoService _contatoService { get; set; }

        protected int _count { get; set; }

        protected bool _erroVisivel { get; set; }

        protected RadzenDataGrid<Compromisso> _dataGrid { get; set; }

        protected string _pesquisar { get; set; } = "";

        [Inject]
        public CompromissoService _compromissoService { get; set; }

        [Inject]
        protected NotificationService _notificar { get; set; }

        [Inject]
        protected DialogService _dialogService { get; set; }

        protected async Task ButtonAdicionar(
            MouseEventArgs args)
        {
            _ehEdit = false;
            _compromisso = new Compromisso();
            _compromisso.DataCompromisso = DateTime.Now;
            _compromisso.HorarioCompromisso = DateTime.Now.TimeOfDay;
        }

        protected async Task ButtonCancelar(
            MouseEventArgs args)
        {

        }

        protected async Task LoadContatos(
            LoadDataArgs args)
        {
            try
            {
                var result = await _contatoService.GetContatosPorNome(args.Filter);
                _contatoSelecionado = result.FirstOrDefault();
                _contatoSelecionadoCount = result.Count();
                _contatos = result.ToList();

                if (!object.Equals(_compromisso.Contato, null))
                {
                    var valueResult = await _contatoService.GetContato(_compromisso.FkIdContato);
                    var firstItem = valueResult;
                    if (firstItem != null)
                    {
                        _contatoSelecionado = firstItem;
                    }
                }

            }
            catch (Exception ex)
            {
                _notificar.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Não foi possível carregar os contatos" });
            }
        }

        protected async Task DataGridCarregarDados(
            LoadDataArgs args)
        {
            try
            {
                var compromisso = GetCompromissoFiltro();
                var result = await _compromissoService.GetCompromissos(compromisso);
                _compromissos = result;
                if (result != null)
                    _count = result.Count();
                else
                    _count = 0;
            }
            catch (Exception ex)
            {
                _notificar.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Erro", Detail = $"Não foi possível carregar os compromissos" });
            }
        }

        protected async Task EditarLinha(
            Compromisso args)
        {
            _ehEdit = true;
            _compromisso = args;
        }

        protected async Task FormSubmit()
        {
            try
            {
                _compromisso.FkIdContato = _contatoSelecionado.IdContato;
                var result = await GetResultFormSubmit();
                if (!_ehEdit)
                    _notificar.Notify(NotificationSeverity.Success, null, "Compromisso criado com sucesso!");
                else
                    _notificar.Notify(NotificationSeverity.Success, null, "Compromisso alterado!");
                await DataGridCarregarDados(null);
            }
            catch (Exception ex)
            {
                _erroVisivel = true;
            }
        }

        private async Task<HttpResponseMessage> GetResultFormSubmit()
        {
            return _ehEdit
                ? await _compromissoService.UpdateCompromisso(_compromisso.IdCompromisso, _compromisso)
                : await _compromissoService.CreateCompromisso(_compromisso);
        }

        protected async Task GridButtonDeletar(
            MouseEventArgs args,
            Compromisso compromisso)
        {
            try
            {
                if (await _dialogService.Confirm("Deseja deletar essa informação?", "Confirmar") == true)
                {
                    var deleteResult = await _compromissoService.DeleteCompromisso(compromisso.IdCompromisso);

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
                    Summary = $"Erro",
                    Detail = $"Não foi possível deletar o compromisso"
                });
            }
        }

        protected async Task Pesquisar(
            ChangeEventArgs args)
        {
            _pesquisar = $"{args.Value}";
            await _dataGrid.GoToPage(0);
            await _dataGrid.Reload();
        }

        private Compromisso GetCompromissoFiltro()
        {
            return new Compromisso()
            {
                Descricao = _pesquisar,
                Contato = new Contato()
                {
                    NomeCompleto = _pesquisar
                },
                Titulo = _pesquisar
            };
        }
    }
}