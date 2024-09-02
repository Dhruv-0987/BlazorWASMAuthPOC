using System.Text;
using Authsignal;
using Duende.IdentityServer;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Services;
using IdentityServerAspNetIdentity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServerAspNetIdentity.Pages.Login;

[SecurityHeaders]
[AllowAnonymous]
public class Callback : PageModel
{
    private readonly IIdentityServerInteractionService _interactionService;
    private readonly IAuthsignalClient _authSignalClient;
    private readonly IEventService _eventService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEventService _events;

    public Callback(
        IIdentityServerInteractionService interactionService,
        IAuthsignalClient authSignalClient, 
        IEventService eventService,
        UserManager<ApplicationUser> userManager,
        IEventService events)
    {
        _interactionService = interactionService;
        _authSignalClient = authSignalClient;
        _eventService = eventService;
        _userManager = userManager;
        _events = events;
    }

    public async Task<IActionResult> OnGet(string returnUrl, string? token)
    {
        var decodedReturnUrl = Encoding.UTF8.GetString(Convert.FromBase64String(returnUrl));
        
        if (token == null)
        {
            return Redirect("https://localhost:5001/Account/Login?ReturnUrl=" + decodedReturnUrl);
        }

        var context = await _interactionService.GetAuthorizationContextAsync(decodedReturnUrl);

        var validateChallengeRequest = new ValidateChallengeRequest(token);
        var validateChallengeResponse = await _authSignalClient.ValidateChallenge(validateChallengeRequest);

        if (validateChallengeResponse.State is not UserActionState.CHALLENGE_SUCCEEDED)
        {
            return Redirect("https://localhost:5001/Account/Login?ReturnUrl=" + decodedReturnUrl);
        }

        var userId = validateChallengeRequest.UserId;
        var user = new ApplicationUser();
        
        if (userId is not null)
        {
            user = await _userManager.FindByIdAsync(userId);
        }

        if (user is not null)
            await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName,
                clientId: context?.Client.ClientId));

        if (context is not null)
        {
            return Redirect(decodedReturnUrl);
        }

        return Page();
    }
}