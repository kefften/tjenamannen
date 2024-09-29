using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using tjenamannen.Models;

namespace tjenamannen.Controllers
{
	[AllowAnonymous]
	public class AccountController : Controller
	{
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IMapper _mapper;

		public AccountController(SignInManager<ApplicationUser> signInManager, 
			UserManager<ApplicationUser> userManager, 
			IMapper mapper, 
			RoleManager<IdentityRole> roleManager)
		{
			_signInManager = signInManager;
			_userManager = userManager;
			_mapper = mapper;
			_roleManager = roleManager;
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginModel model, string returnUrl = null)
		{
			ViewData["ReturnUrl"] = returnUrl;
			if (ModelState.IsValid)
			{
				var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
				if (result.Succeeded)
				{
					return RedirectToAction(nameof(HomeController.Index), "Home");
				}
				else
				{
					ModelState.AddModelError(string.Empty, "Invalid login attempt.");
					return View(model);
				}
			}
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Login");
		}
		// Register Action (GET)
		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(UserRegistrationModel userModel)
		{
			if (!ModelState.IsValid)
			{
				return View(userModel);
			}

			var user = _mapper.Map<ApplicationUser>(userModel);

			var roleExist = await _roleManager.RoleExistsAsync("Admin");
			if (!roleExist)
			{
				await _roleManager.CreateAsync(new IdentityRole("Admin"));
			}
			await _userManager.AddToRoleAsync(user, "Admin");

			var result = await _userManager.CreateAsync(user, userModel.Password);
			if (!result.Succeeded)
			{
				foreach (var error in result.Errors)
				{
					ModelState.TryAddModelError(error.Code, error.Description);
				}

				return View(userModel);
			}

			return RedirectToAction(nameof(HomeController.Index), "Home");
		}
		public IActionResult AccessDenied()
		{
			return View();
		}
	}

	public class UserRegistrationModel
	{
		[Required]
		public string UserName { get; set; }
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }
	}
	public class LoginModel
	{
		[Required]
		public string UserName { get; set; }
		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[Display(Name = "Remember me?")]
		public bool RememberMe { get; set; }
	}
}