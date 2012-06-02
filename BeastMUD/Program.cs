using System;
using System.IO;
using Beast;

namespace BeastMUD
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("BeastMUD v1.0");
			Console.WriteLine();
			try
			{
				var game = new Game(new GameSettings
				                    	{
											FileRepositoryPath = Directory.GetCurrentDirectory() + "\\content"
				                    	});
				game.Start();
				while (game.IsRunning)
				{
					var input = Console.ReadLine();
					if (string.IsNullOrEmpty(input))
						continue;

					if (input == "quit")
					{
						game.Stop();
						continue;
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				Console.WriteLine("Press any key to exit...");
				Console.ReadKey();
			}
		}
	}
}
