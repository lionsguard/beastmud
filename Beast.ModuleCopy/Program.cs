using System;
using System.IO;
using System.Xml.Linq;

namespace Beast.ModuleCopy
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("BeastMUD Module Copy Utility");

			if (args == null || args.Length == 0)
			{
				Console.WriteLine("Missing path to the modules.xml file.");
				return;
			}
			var xmlFilePath = args[0];
			Console.WriteLine("XML File Path = {0}", xmlFilePath);

			if (args.Length < 1)
			{
				Console.WriteLine("Output directory not defined.");
				return;
			}
			var outputPath = args[1];
			Console.WriteLine("Output Path = {0}", outputPath);

			if (args.Length < 2)
			{
				Console.WriteLine("Root directory not defined.");
				return;
			}
			var rootPath = args[2];
			Console.WriteLine("Root Path = {0}", rootPath);

			try
			{
				var doc = XDocument.Load(xmlFilePath);
				var root = doc.Root;
				if (root == null)
				{
					Console.WriteLine("XML document is missing the root node.");
					return;
				}

				foreach (var element in root.Elements("module"))
				{
					var attrPath = element.Attribute("path");
					if (attrPath == null)
					{
						Console.WriteLine("Module is missing the 'path' attribute.");
						continue;
					}
					var filePath = Path.Combine(rootPath, attrPath.Value);
					Console.WriteLine("Processing Module {0}", filePath);
					if (!File.Exists(filePath))
					{
						Console.WriteLine("File does not exist '{0}'.", filePath);
						continue;
					}
					if (!Directory.Exists(outputPath))
						Directory.CreateDirectory(outputPath);

					try
					{
						File.Copy(filePath, Path.Combine(outputPath, Path.GetFileName(filePath)), true);
					}
					catch (Exception fe)
					{
						Console.WriteLine("Error: {0}", fe);
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}
	}
}
