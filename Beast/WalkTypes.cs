using System;

namespace Beast
{
	[Flags]
	public enum WalkTypes
	{
		None = 0,
		Walk = 1,
		Mount = 2,
		Swim = 4,
		Fly = 8,
	}
}