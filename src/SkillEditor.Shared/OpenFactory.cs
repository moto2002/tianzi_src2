using System;

namespace SkillEditor.Shared
{
	public static class OpenFactory
	{
		public static bool PlatformCanOpen()
		{
			return false;
		}

		public static IOpen CreateOpen()
		{
			throw new Exception("Platform doesn't support open commands, try to call OpenFactory.PlatformCanOpen");
		}
	}
}
