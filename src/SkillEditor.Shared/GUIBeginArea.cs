using System;
using UnityEngine;

namespace SkillEditor.Shared
{
	public class GUIBeginArea : IDisposable
	{
		public GUIBeginArea(Rect area)
		{
			GUILayout.BeginArea(area);
		}

		public GUIBeginArea(Rect area, string content)
		{
			GUILayout.BeginArea(area, content);
		}

		public GUIBeginArea(Rect area, string content, string style)
		{
			GUILayout.BeginArea(area, content, style);
		}

		public void Dispose()
		{
			GUILayout.EndArea();
		}
	}
}
