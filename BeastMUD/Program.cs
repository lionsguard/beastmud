﻿using System;
using System.Net;
using Beast;
using Beast.Net;
using Beast.Text;

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
				var game = Game.Current;
				game.Start();

				var listener = new SocketListener();
				listener.Start(new IPEndPoint(IPAddress.Any, 4500), new TextMessageFormatter(), c => c.Write(new NotificationMessage{Category = 6, Text = "Welcome to Beast MUD"}));

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
