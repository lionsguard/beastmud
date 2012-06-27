using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Beast.Tests
{
    
    
    /// <summary>
    ///This is a test class for CommandManagerTest and is intended
    ///to contain all CommandManagerTest Unit Tests
    ///</summary>
	[TestClass()]
	public class CommandManagerTest : TestBase
	{

		#region Additional test attributes
		// 
		//You can use the following additional attributes as you write your tests:
		//
		//Use ClassInitialize to run code before running the first test in the class
		//[ClassInitialize()]
		//public static void MyClassInitialize(TestContext testContext)
		//{
		//    Init();
		//}
		//
		//Use ClassCleanup to run code after all tests in a class have run
		//[ClassCleanup()]
		//public static void MyClassCleanup()
		//{
		//    Cleanup();
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

		private static void ExecuteCommandTest(string command, params KeyValuePair<string, object>[] args)
		{
			var input = new TestInput(command, args);
			Game.Current.Commands.Execute(input, Connection);
			Connection.Flush();
		}

		[TestMethod]
		public void CommandHelpTest()
		{
			ExecuteCommandTest("help", new KeyValuePair<string, object>("command", "help"));
		}

		[TestMethod]
		public void CommandWhoTest()
		{
			ExecuteCommandTest("who");
		}
	}
}
