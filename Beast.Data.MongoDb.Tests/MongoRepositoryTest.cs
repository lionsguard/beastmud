using Beast.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Beast.Data.MongoDb.Tests
{
    /// <summary>
    ///This is a test class for MongoRepositoryTest and is intended
    ///to contain all MongoRepositoryTest Unit Tests
    ///</summary>
	[TestClass()]
	public class MongoRepositoryTest : RepositoryTestBase
    {
    	[ClassInitialize()]
		public static void InitTest(TestContext context)
		{
			Repository = new MongoRepository
			{
				ConnectionString = "mongodb://localhost/beast",
				DatabaseName = "beast"
			};
			Repository.Initialize();
		}

		[TestMethod()]
		public void SaveUserTest()
		{
			BaseSaveUserTest();
		}

		[TestMethod()]
		public void GetUserCountTest()
		{
			BaseGetUserCountTest();
		}

		[TestMethod()]
		public void GetUserTest()
		{
			BaseGetUserTest();
		}

		[TestMethod]
		public void SaveTemplateTest()
		{
			BaseSaveTemplateTest();
		}

		[TestMethod]
		public void GetTemplateTest()
		{
			BaseGetTemplateTest();
		}

		[TestMethod]
		public void GetPlaceTest()
		{
			BaseGetPlaceTest();
		}

		[TestMethod]
		public void SavePlaceTest()
		{
			BaseSavePlaceTest();
		}

		[TestMethod]
		public void GetTerrainTest()
		{
			BaseGetTerrainTest();
		}

		[TestMethod]
		public void SaveTerrainTest()
		{
			BaseSaveTerrainTest();
		}

		[TestMethod]
		public void GetCharacterTest()
		{
			BaseGetCharacterTest();
		}

		[TestMethod]
		public void SaveCharacterTest()
		{
			BaseSaveCharacterTest();
		}
	}
}
