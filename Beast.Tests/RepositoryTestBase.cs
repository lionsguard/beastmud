
using System.Collections.Generic;
using Beast.Data;
using Beast.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Beast.Tests
{
	public abstract class RepositoryTestBase : TestBase
	{
		private const string UserName = "TestUserName";
		private const string Password = "TestPassword";
		private const string PasswordSalt = "Salt";
		private const string Email = "test@email.com";

		private const string TemplateName = "TestTemplate";
		private const string TemplateDesc = "Test template description";

		protected IRepository Repository { get; set; }

		protected void BaseSaveUserTest()
		{
			var user = new User
			{
				Logins = new List<Login>
							         	{
							         		new GenericLogin
							         			{
							         				UserName = UserName,
													Password = Password,
													PasswordSalt = PasswordSalt,
													Email = Email
							         			}
							         	}
			};
			Repository.SaveUser(user);
			Assert.IsNotNull(user.Id);
		}

		protected void BaseGetUserCountTest()
		{
			var count = Repository.GetUserCount();
			Assert.IsTrue(count > 0);
		}
		
		protected void BaseGetUserTest()
		{
			var user = Repository.GetUser(new GenericLogin
			{
				UserName = UserName
			});
			Assert.IsTrue(user.Logins.Count > 0);
			var login = user.Logins[0] as GenericLogin;
			Assert.IsNotNull(login);
			Assert.AreEqual(login.UserName, UserName);
			Assert.AreEqual(login.Email, Email);
			Assert.AreEqual(login.Password, Password);
			Assert.AreEqual(login.PasswordSalt, PasswordSalt);
		}

		protected void BaseSaveTemplateTest()
		{
			var template = new GameObject
			{
				Name = TemplateName,
				Description = TemplateDesc
			};
			Repository.SaveTemplate(template);
			Assert.IsNotNull(template.Id);

		}

		protected void BaseGetTemplateTest()
		{
			var template = Repository.GetTemplate(TemplateName);
			Assert.IsNotNull(template);
			Assert.AreEqual(template.Name, TemplateName);
			Assert.AreEqual(template.Description, TemplateDesc);
		}
	}
}
