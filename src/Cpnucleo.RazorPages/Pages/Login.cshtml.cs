﻿using Cpnucleo.Infra.CrossCutting.Util.Requests.Auth;
using Cpnucleo.RazorPages.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace Cpnucleo.RazorPages.Pages;

public class LoginModel : PageModel
{
    private readonly ICpnucleoAuthApiClient _cpnucleoAuthApiClient;
    private readonly IConfiguration _configuration;

    public LoginModel(ICpnucleoAuthApiClient cpnucleoApiClient, IConfiguration configuration)
    {
        _cpnucleoAuthApiClient = cpnucleoApiClient;
        _configuration = configuration;
    }

    [BindProperty]
    public AuthDTO Auth { get; set; }

    public async Task<IActionResult> OnGetAsync(string? returnUrl = null, bool logout = false)
    {
        try
        {
            if (logout)
            {
                await HttpContext.SignOutAsync();

                return RedirectToPage("Login");
            }

            ViewData["ReturnUrl"] = returnUrl;

            return Page();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _cpnucleoAuthApiClient.PostAsync<AuthResponse>("auth", new AuthRequest { Usuario = Auth.Usuario, Senha = Auth.Senha });

            if (result.Status == OperationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Usuário ou senha inválidos.");
                return Page();
            }
            else
            {
                IEnumerable<Claim> claims = new[]
                {
                    new Claim(ClaimTypes.PrimarySid, result.Recurso.Id.ToString()),
                    new Claim(ClaimTypes.Hash, result.Token)
                };

                ClaimsPrincipal principal = ClaimsService.CreateClaimsPrincipal(claims);

                int.TryParse(_configuration["Cookie:Expires"], out int expiresUtc);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(expiresUtc)
                    });

                return RedirectToLocal(returnUrl);
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }

    private IActionResult RedirectToLocal(string returnUrl)
    {
        if (Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }
        else
        {
            return RedirectToPage("Apontamento/Listar");
        }
    }
}
