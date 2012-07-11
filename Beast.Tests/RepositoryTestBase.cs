
using System.Collections.Generic;
using System.Linq;
using Beast.Data;
using Beast.Mobiles;
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

		private const string PlaceId = "E2AECB42-7C0B-4640-AFA2-F99EF13F2578";
		private const string PlaceName = "TestPlace";
		private const string PlaceDesc = "This is the description for the test place.";

		private const string CharName = "Character1";

		private const string UserId = "355D7BAA-9CD6-4B5A-AA91-4486E2AF6589";

		protected static IUserRepository Users { get; set; }
		protected static ITemplateRepository Templates { get; set; }
		protected static IPlaceRepository Places { get; set; }
		protected static ICharacterRepository Characters { get; set; }

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
			Users.SaveUser(user);
			Assert.IsNotNull(user.Id);
		}

		protected void BaseGetUserCountTest()
		{
			var count = Users.GetUserCount();
			Assert.IsTrue(count > 0);
		}
		
		protected void BaseGetUserTest()
		{
			var user = Users.GetUser(UserName);
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
			Templates.SaveTemplate(template);
			Assert.IsNotNull(template.Id);

		}

		protected void BaseGetTemplateTest()
		{
			var template = Templates.GetTemplate(TemplateName);
			Assert.IsNotNull(template);
			Assert.AreEqual(template.Name, TemplateName);
			Assert.AreEqual(template.Description, TemplateDesc);
		}

		protected void BaseGetPlaceTest()
		{
			var place = Places.GetPlace(PlaceId);
			Assert.IsNotNull(place);
			Assert.AreEqual(place.Name, PlaceName);
			Assert.AreEqual(place.Description, PlaceDesc);
			Assert.AreEqual(place.Terrain, 1);
		}

		protected void BaseSavePlaceTest()
		{
			var place = new Place
			            	{
								Id = PlaceId,
			            		Name = PlaceName,
			            		Description = PlaceDesc,
			            		Terrain = 1,
			            	};
			Places.SavePlace(place);
			Assert.IsNotNull(place.Id);
		}

		protected void BaseGetCharacterTest()
		{
			var character = Characters.GetCharacters(UserId).FirstOrDefault();
			Assert.IsNotNull(character);
		}

		protected void BaseSaveCharacterTest()
		{
			var character = new Character
			                	{
									Name = CharName
			                	};
			character[CommonProperties.UserId] = UserId;
			Characters.SaveCharacter(character);
			Assert.IsNotNull(character);
			Assert.IsNotNull(character.Id);

			Assert.AreEqual(character.Name, CharName);
		}
	}
}
