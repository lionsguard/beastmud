
using System.ComponentModel.DataAnnotations;

namespace Beast.Web.Models
{
	public class LoginData
	{
		[Required]
		public string UserName { get; set; }
		[Required]
		public string Password { get; set; }
		public bool RememberMe { get; set; }
		public bool CanInstall { get; set; }
	}
}