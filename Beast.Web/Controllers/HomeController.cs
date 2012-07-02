using System.Web.Mvc;
using Beast.Net;
using Beast.Security;
using Beast.Web.Models;

namespace Beast.Web.Controllers
{
    public class HomeController : BeastController
    {
		[Authorize]
		public ActionResult Index()
		{
			return View(new DashboardData
			            	{
			            		UserCount = Game.Current.Repository.GetUserCount(),
								CharacterCount = Game.Current.Repository.GetCharacterCount()
			            	});
		}

		[HttpGet]
        public ActionResult Login()
        {
            return View(new LoginData
                        	{
								ReturnUrl = Request.Params["returnUrl"],
                        		CanInstall = CanInstall()
                        	});
        }

		[HttpPost]
		public ActionResult Login(LoginData data)
		{
			data.CanInstall = CanInstall();
			if (ModelState.IsValid)
			{
				var input = new JsonInput { { "username", data.UserName }, { "password", data.Password } };
				input.CommandName = "login";
				if (data.CanInstall)
				{
					User user;
					if (Game.Current.Users.TryGetUser(input, out user))
					{
						// User already created.
						ModelState.AddModelError("Login", CommonResources.LoginAlreadyExists);
					}

					if (user == null)
					{
						// Try to create the user account.
						user = new User
								{
									AccessLevel = UserAccessLevel.God
								};
						Login login;
						if (!Game.Current.Users.TryAddLogin(input, out login))
						{
							ModelState.AddModelError("Login", CommonResources.LoginAlreadyExists);
						}
						if (login != null)
						{
							user.Logins.Add(login);
							Game.Current.Repository.SaveUser(user);
						}
					}
				}

				// Attempt to login.
				var response = ProcessInput(input);
				if (response != null && response.Success)
				{
					Authorize((string) response.Data, data.RememberMe);

					// Successful login
					if (!string.IsNullOrEmpty(data.ReturnUrl))
						return Redirect(data.ReturnUrl);
					return RedirectToAction("index");
				}

				// Login failed
				ModelState.AddModelError("Login", response != null ? response.Error : CommonResources.LoginInvalid);
			}
			return View(data);
		}

		public ActionResult Logout()
		{
			SignOut();
			return RedirectToAction("login");
		}

		private static bool CanInstall()
		{
			return Game.Current.Config.Install && Game.Current.Repository.GetUserCount() == 0;
		}
    }
}
