using Beast.Configuration;
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

		private static bool _initialized;

		//[AssemblyInitialize]
		public static void Init(TestContext context)
		{
			Init(context, new BeastSection());
		}

		public static void Init(TestContext context, BeastSection config)
		{
			TestContextLogger.TestContext = context;

			if (_initialized)
				return;

			_game = Game.Current;
			_game.Start(config);

			Connection = ConnectionManager.Create(new TestContextConnectionFactory(context));

			_initialized = true;
		}

		[AssemblyCleanup]
		public static void Cleanup()
		{
			_game.Stop();
			_game = null;
		}
	}
}