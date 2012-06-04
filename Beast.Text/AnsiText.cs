using System.Text;

namespace Beast.Text
{
	public struct AnsiText
	{
		private string _text;
		private AnsiColor _color;

		private bool _bold;
		private bool _italics;
		private bool _underline;
		private bool _strikethrough;
		private bool _clear;

		public AnsiText(string text)
		{
			_text = text;
			_color = AnsiColor.White;

			_bold = false;
			_italics = false;
			_underline = false;
			_strikethrough = false;
			_clear = false;
		}

		public AnsiText Color(AnsiColor color)
		{
			_color = color;
			return this;
		}

		public AnsiText Clear()
		{
			_clear = true;
			return this;
		}

		public AnsiText Bold()
		{
			_bold = true;
			return this;
		}

		public AnsiText Italics()
		{
			_italics = true;
			return this;
		}

		public AnsiText Underline()
		{
			_underline = true;
			return this;
		}

		public AnsiText StrikeThrough()
		{
			_strikethrough = true;
			return this;
		}

		public override string ToString()
		{
			var sb = new StringBuilder();

			if (_clear)
				sb.Append("\x1B[0m");
			if (_bold)
				sb.Append("\x1B[1m");
			if (_italics)
				sb.Append("\x1B[3m");
			if (_underline)
				sb.Append("\x1B[4m");
			if (_strikethrough)
				sb.Append("\x1B[9m");

			sb.Append("\x1B[").Append(((int)_color).ToString()).Append("m");
			sb.Append(_text);

			if (_bold)
				sb.Append("\x1B[22m");
			if (_italics)
				sb.Append("\x1B[23m");
			if (_underline)
				sb.Append("\x1B[24m");
			if (_strikethrough)
				sb.Append("\x1B[29m");

			return sb.ToString();
		}
	}
}