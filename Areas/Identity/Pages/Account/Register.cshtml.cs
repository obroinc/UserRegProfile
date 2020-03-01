using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using UserRegProfile.Data;
using UserRegProfile.Models;
using UserRegProfile.Utility;

namespace UserRegProfile.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;
        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole>roleManager,
            ApplicationDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _db = db;
            _roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel

                    {
            [Required]          
            [Display(Name = "Contact's First Name")]
            public string First_Name { get; set; }

            [Required]
            [Display(Name = "Contact's Last Name")]
            public string Last_Name { get; set; }

            [Display(Name = "Player First Name")]
            public string Player1_First_Name { get; set; }
            [Display(Name = "Player Last Name")]
            public string Player1_Last_Name { get; set; }
            [Display(Name = "Choose Team")]
            public string Player1_Team { get; set; }
            [Display(Name = "Player First Name")]
            public string Player2_First_Name { get; set; }
            [Display(Name = "Player First Name")]
            public string Player2_Last_Name { get; set; }
            [Display(Name = "Choose Team")]
            public string Player2_Team { get; set; }

            [Required]
            [Display(Name = "Mobile Phone Number (To Recieve Club update Information)")]
            public string PhoneNumber { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            
        }
        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                //Changing fron Idenetity User to ApplicationUser with extra details from reg
                var user = new ApplicationUser { UserName = Input.Email, 
                    Email = Input.Email,
                    First_Name=Input.First_Name,
                    Last_Name=Input.Last_Name,
                    Player1_First_Name=Input.Player1_First_Name,
                    Player1_Last_Name=Input.Player1_Last_Name,
                    Player1_Team=Input.Player1_Team,
                    Player2_First_Name=Input.Player2_First_Name,
                    Player2_Last_Name=Input.Player2_Last_Name,
                    Player2_Team=Input.Player2_Team,
                    PhoneNumber=Input.PhoneNumber
                };

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)

                {
                    if(!await _roleManager.RoleExistsAsync(SD.AdminUser))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(SD.AdminUser));

                    }

                    if (!await _roleManager.RoleExistsAsync(SD.MemberUser))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(SD.MemberUser));

                    }
                    if (!await _roleManager.RoleExistsAsync(SD.CoachUser))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(SD.CoachUser));

                    }

                    await _userManager.AddToRoleAsync(user, SD.AdminUser);

                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
