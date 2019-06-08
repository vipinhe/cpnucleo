﻿using Cpnucleo.Pages.Hubs;
using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Pages
{
    [Authorize]
    public class FluxoTrabalhoModel : PageModel
    {
        private readonly IWorkflowRepository _workflowRepository;

        private readonly ITarefaRepository _tarefaRepository;

        private readonly IHubContext<FluxoTrabalhoHub> _hubContext;

        public FluxoTrabalhoModel(IWorkflowRepository workflowRepository,
                                  ITarefaRepository tarefaRepository,
                                  IHubContext<FluxoTrabalhoHub> hubContext)
        {
            _workflowRepository = workflowRepository;
            _tarefaRepository = tarefaRepository;
            _hubContext = hubContext;
        }

        public WorkflowItem Workflow { get; set; }

        public IEnumerable<WorkflowItem> Lista { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Lista = await _workflowRepository.ListarTarefasWorkflow();

            int qtdLista = Lista.Count();
            qtdLista = qtdLista == 1 ? 2 : qtdLista;

            int i = 12 / qtdLista;
            ViewData["col-lg-*"] = i;

            return Page();
        }

        public IActionResult OnPost(int idTarefa, int idWorkflow)
        {
            _tarefaRepository.AlterarPorFluxoTrabalho(idTarefa, idWorkflow);

            _hubContext.Clients.All.SendAsync("send", "-> Lorem Ipsum");

            return Page();
        }
    }
}