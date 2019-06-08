﻿using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Pages.RecursoTarefa
{
    [Authorize]
    public class RemoverModel : PageModel
    {
        private readonly IRecursoTarefaRepository _recursoTarefaRepository;

        private readonly ITarefaRepository _tarefaRepository;

        public RemoverModel(IRecursoTarefaRepository recursoTarefaRepository,
                                       ITarefaRepository tarefaRepository)
        {
            _recursoTarefaRepository = recursoTarefaRepository;
            _tarefaRepository = tarefaRepository;
        }

        [BindProperty]
        public RecursoTarefaItem RecursoTarefa { get; set; }

        public async Task<IActionResult> OnGetAsync(int idRecursoTarefa)
        {
            RecursoTarefa = await _recursoTarefaRepository.Consultar(idRecursoTarefa);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(RecursoTarefaItem recursoTarefa)
        {
            if (!ModelState.IsValid)
            {
                recursoTarefa.Tarefa = await _tarefaRepository.Consultar(recursoTarefa.IdTarefa);

                return Page();
            }

            await _recursoTarefaRepository.Remover(recursoTarefa);

            return RedirectToPage("Listar", new { idTarefa = recursoTarefa.IdTarefa });
        }
    }
}