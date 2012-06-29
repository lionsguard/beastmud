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
		[TestInitialize()]
		public void InitTest()
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
	}
}
