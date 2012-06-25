using System;
using Beast.Net;

namespace Beast.Text
{
	public class BasicTextParser : ITextParser
	{
		#region Implementation of ITextParser

		public IInput Parse(string text)
		{
			throw new NotImplementedException();
			//var cmd = new Command();
			//if (!string.IsNullOrEmpty(text))
			//{
			//    var words = text.Split(' ');
			//    if (words.Length > 0)
			//    {
			//        // First word is the command.
			//        cmd.Name = words[0];

			//        // Find the command definition.
			//        var def = CommandManager.GetDefinition(cmd.Name);
			//        if (def != null)
			//        {
			//            // Attempt to add an argument that maps to the remaining words of the input string.
			//            var remainder = new string[words.Length - 1];
			//            words.CopyTo(remainder, 1);
			//            for (var i = 0; i < def.Arguments.Count; i++)
			//            {
			//                cmd.Add(def.Arguments[i], remainder[i]);
			//            }
			//        }
			//    }
			//}
			//return cmd;
		}

		#endregion
	}
}