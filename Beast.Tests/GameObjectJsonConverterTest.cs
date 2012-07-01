using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Beast.Tests
{
    
    
    /// <summary>
    ///This is a test class for GameObjectJsonConverterTest and is intended
    ///to contain all GameObjectJsonConverterTest Unit Tests
    ///</summary>
	[TestClass()]
	public class GameObjectJsonConverterTest : TestBase
	{

		#region Additional test attributes
		// 
		//You can use the following additional attributes as you write your tests:
		//
		//Use ClassInitialize to run code before running the first test in the class
		//[ClassInitialize()]
		//public static void MyClassInitialize(TestContext testContext)
		//{
		//}
		//
		//Use ClassCleanup to run code after all tests in a class have run
		//[ClassCleanup()]
		//public static void MyClassCleanup()
		//{
		//}
		//
		//Use TestInitialize to run code before running each test
		//[TestInitialize()]
		//public void MyTestInitialize()
		//{
		//}
		//
		//Use TestCleanup to run code after each test has run
		//[TestCleanup()]
		//public void MyTestCleanup()
		//{
		//}
		//
		#endregion

		private const string ExpectedJsonFormat = "{0}\"Id\":\"{2}\",\"Name\":\"{3}\",\"Description\":\"{4}\"{1}";
		private const string Name = "TestObject";
		private const string Desc = "Test object's description...";

		[TestMethod()]
		public void GameObjectJsonSerializationTest()
		{
			var id = Guid.NewGuid().ToString();

			var expectedJson = string.Format(ExpectedJsonFormat, "{","}", id, Name, Desc);

			var obj = new GameObject
			          	{
			          		Id = id,
			          		Name = Name,
			          		Description = Desc
			          	};

			var json = obj.ToJson();
			TestContext.WriteLine("JSON: {0}", json);

			Assert.AreEqual(json, expectedJson);
		}

		[TestMethod()]
		public void GameObjectJsonDeserializationTest()
		{
			var id = Guid.NewGuid().ToString();

			var json = string.Format(ExpectedJsonFormat, "{", "}", id, Name, Desc);
			var obj = json.FromJson<GameObject>();

			Assert.IsNotNull(obj);
			
			TestContext.WriteLine("DESERIALIZED OBJECT JSON: {0}", obj.ToJson());

			Assert.AreEqual(obj.Id, id);
			Assert.AreEqual(obj.Name, Name);
			Assert.AreEqual(obj.Description, Desc);
		}
	}
}
