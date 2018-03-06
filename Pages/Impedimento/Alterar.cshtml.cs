﻿using dotnet_cpnucleo_pages.Repository;
using dotnet_cpnucleo_pages.Repository.Impedimento;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace dotnet_cpnucleo_pages.Pages.Impedimento
{
    [Authorize]
    public class AlterarModel : PageModel
    {
        private readonly IRepository<ImpedimentoItem> _impedimentoRepository;

        public AlterarModel(IRepository<ImpedimentoItem> impedimentoRepository) => _impedimentoRepository = impedimentoRepository;

        [BindProperty]
        public ImpedimentoItem Impedimento { get; set; }

        public async Task<IActionResult> OnGetAsync(int idImpedimento)
        {
            Impedimento = await _impedimentoRepository.Consultar(idImpedimento);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(ImpedimentoItem impedimento)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _impedimentoRepository.Alterar(impedimento);

            return RedirectToPage("Listar");
        }
    }
}