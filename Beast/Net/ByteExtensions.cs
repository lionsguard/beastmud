namespace Beast.Net
{
	public static class ByteExtensions
	{
		public static bool EndsWith(this byte[] first, byte[] second)
		{
			if (first.Length < second.Length)
				return false;

			var lastIdx = first.Length - 1;
			for (var i = second.Length - 1; i >= 0; i--)
			{
				if (second[i] != first[lastIdx])
					return false;
				lastIdx--;
			}
			return true;
		}
	}
}