
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

		private const string PlaceName = "TestPlace";
		private static readonly Unit PlaceLocation = new Unit(1, 1, 1);
		private const string PlaceDesc = "This is the description for the test place.";

		private const string CharName = "Character1";
		private static readonly Unit CharPosition = new Unit(3,2,1);

		private const string UserId = "355D7BAA-9CD6-4B5A-AA91-4486E2AF6589";

		protected static IRepository Repository { get; set; }

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
			var user = Repository.GetUser(UserName);
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

		protected void BaseGetPlaceTest()
		{
			var place = Repository.GetPlace(PlaceLocation);
			Assert.IsNotNull(place);
			Assert.AreEqual(place.Name, PlaceName);
			Assert.AreEqual(place.Description, PlaceDesc);
			Assert.AreEqual(place.Terrain, 1);
			Assert.IsTrue(place.Exits.North);
			Assert.IsTrue(place.Exits.South);
		}

		protected void BaseSavePlaceTest()
		{
			var place = new Place
			            	{
			            		Name = PlaceName,
			            		Description = PlaceDesc,
			            		Location = PlaceLocation,
			            		Terrain = 1,
			            		Exits = {North = true, South = true}
			            	};
			Repository.SavePlace(place);
			Assert.IsNotNull(place.Id);
		}

		protected void BaseGetTerrainTest()
		{
			var terrain = Repository.GetTerrain();
			Assert.IsTrue(terrain.Count() == 2);
			Assert.IsNotNull(terrain.FirstOrDefault(t => t.Color == "Red"));
			Assert.IsNotNull(terrain.FirstOrDefault(t => t.Color == "Green"));
		}

		protected void BaseSaveTerrainTest()
		{
			var terrain1 = new Terrain
			               	{
			               		Id = 1,
			               		Name = "Terrain1",
			               		Color = "Red",
			               		WalkType = WalkTypes.Swim
			               	};
			var terrain2 = new Terrain
							{
								Id = 2,
								Name = "Terrain2",
								Color = "Green",
								WalkType = WalkTypes.Fly
							};
			Repository.SaveTerrain(terrain1);
			Repository.SaveTerrain(terrain2);
			
			Assert.IsTrue(Repository.GetTerrain().Count() == 2);
		}

		protected void BaseGetCharacterTest()
		{
			var character = Repository.GetCharacters(UserId).FirstOrDefault();
			Assert.IsNotNull(character);
		}

		protected void BaseSaveCharacterTest()
		{
			var character = new Character
			                	{
									Name = CharName,
									Position = CharPosition
			                	};
			character[CommonProperties.UserId] = UserId;
			Repository.SaveCharacter(character);
			Assert.IsNotNull(character);
			Assert.IsNotNull(character.Id);

			Assert.AreEqual(character.Name, CharName);
			Assert.AreEqual(character.Position, CharPosition);
		}
	}
}
