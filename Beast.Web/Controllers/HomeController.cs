using System.Web.Mvc;
using Beast.Net;
using Beast.Security;
using Beast.Web.Models;

namespace Beast.Web.Controllers
{
    public class HomeController : Controller
    {
		[HttpGet]
        public ActionResult Index()
        {
            return View(new LoginData
                        	{
                        		CanInstall = CanInstall()
                        	});
        }

		[HttpPost]
		public ActionResult Index(LoginData data)
		{
			data.CanInstall = CanInstall();
			if (ModelState.IsValid)
			{
				var input = new JsonInput { { "username", data.UserName }, { "password", data.Password } };
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
			}
			return View(data);
		}

		private static bool CanInstall()
		{
			return Game.Current.Config.Install && Game.Current.Repository.GetUserCount() == 0;
		}
    }
}
