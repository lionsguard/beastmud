using Beast.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Beast.Tests
{
	[TestClass]
	public abstract class TestBase
	{
		private static Game _game;

		public static IConnection Connection { get; private set; }

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext { get; set; }

		[AssemblyInitialize]
		public static void Init(TestContext context)
		{
			TestContextLogger.TestContext = context;

			_game = new Game(new GameSettings
								{
									FileRepositoryPath = @"C:\Projects\lionsguard\github\beastmud\BeastMUD\bin\Debug\data",
									ModuleTypes = new[] { typeof(TestContextLogger)}
								});
			_game.Start();

			Connection = ConnectionManager.Create(new TestContextConnectionFactory(context));
		}

		[AssemblyCleanup]
		public static void Cleanup()
		{
			_game.Stop();
			_game = null;
		}
	}
}