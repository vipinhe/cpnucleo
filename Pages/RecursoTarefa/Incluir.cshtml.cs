﻿using dotnet_cpnucleo_pages.Repository.RecursoProjeto;
using dotnet_cpnucleo_pages.Repository.RecursoTarefa;
using dotnet_cpnucleo_pages.Repository.Tarefa;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace dotnet_cpnucleo_pages.Pages.RecursoTarefa
{
    [Authorize]
    public class IncluirModel : PageModel
    {
        private readonly IRecursoTarefaRepository _recursoTarefaRepository;

        private readonly IRecursoProjetoRepository _recursoProjetoRepository;

        private readonly ITarefaRepository _tarefaRepository;

        public IncluirModel(IRecursoTarefaRepository recursoTarefaRepository,
                                       IRecursoProjetoRepository recursoProjetoRepository,
                                       ITarefaRepository tarefaRepository)
        {
            _recursoTarefaRepository = recursoTarefaRepository;
            _recursoProjetoRepository = recursoProjetoRepository;
            _tarefaRepository = tarefaRepository;
        }

        [BindProperty]
        public RecursoTarefaItem RecursoTarefa { get; set; }

        public TarefaItem Tarefa { get; set; }

        public SelectList SelectRecursos { get; set; }

        public async Task<IActionResult> OnGetAsync(int idTarefa)
        {
            Tarefa = await _tarefaRepository.Consultar(idTarefa);

            SelectRecursos = new SelectList(await _recursoProjetoRepository.ListarPoridProjeto(Tarefa.IdProjeto), "Recurso.IdRecurso", "Recurso.Nome");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(RecursoTarefaItem recursoTarefa)
        {
            if (!ModelState.IsValid)
            {
                recursoTarefa.Tarefa = await _tarefaRepository.Consultar(recursoTarefa.IdTarefa);
                SelectRecursos = new SelectList(await _recursoProjetoRepository.ListarPoridProjeto(recursoTarefa.Tarefa.IdProjeto), "Recurso.IdRecurso", "Recurso.Nome");

                return Page();
            }

            await _recursoTarefaRepository.Incluir(recursoTarefa);

            return RedirectToPage("Listar", new { idTarefa = recursoTarefa.IdTarefa });
        }
    }
}