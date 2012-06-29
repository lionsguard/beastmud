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

		[AssemblyInitialize]
		public static void Init(TestContext context)
		{
			TestContextLogger.TestContext = context;

			_game = Game.Current;
			_game.Start(new BeastSection
							{
								Repository = new RepositoryElement
								             	{
								             		Type = typeof(TestRepository).AssemblyQualifiedName
								             	}
							});

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