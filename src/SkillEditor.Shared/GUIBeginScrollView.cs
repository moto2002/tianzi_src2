using System;
using UnityEngine;

namespace SkillEditor.Shared
{
	public class GUIBeginScrollView : IDisposable
	{
		public Vector2 Scroll
		{
			get;
			set;
		}

		public GUIBeginScrollView(Vector2 scrollPosition)
		{
			this.Scroll = GUILayout.BeginScrollView(scrollPosition, new GUILayoutOption[0]);
		}

		public GUIBeginScrollView(Vector2 scrollPosition, params GUILayoutOption[] options)
		{
			this.Scroll = GUILayout.BeginScrollView(scrollPosition, options);
		}

		public GUIBeginScrollView(Vector2 scrollPosition, GUIStyle style)
		{
			this.Scroll = GUILayout.BeginScrollView(scrollPosition, style);
		}

		public GUIBeginScrollView(Vector2 scrollPosition, GUIStyle style, params GUILayoutOption[] options)
		{
			this.Scroll = GUILayout.BeginScrollView(scrollPosition, style, options);
		}

		public GUIBeginScrollView(Vector2 scrollPosition, GUIStyle horizontalScrollBar, GUIStyle verticalScrollBar)
		{
			this.Scroll = GUILayout.BeginScrollView(scrollPosition, horizontalScrollBar, verticalScrollBar, new GUILayoutOption[0]);
		}

		public GUIBeginScrollView(Vector2 scrollPosition, bool alwaysShowHorizontalScrollBar, bool alwaysShowVerticalScrollBar, GUIStyle horizontalScrollBar, GUIStyle verticalScrollBar)
		{
			this.Scroll = GUILayout.BeginScrollView(scrollPosition, alwaysShowHorizontalScrollBar, alwaysShowVerticalScrollBar, horizontalScrollBar, verticalScrollBar, new GUILayoutOption[0]);
		}

		public GUIBeginScrollView(Vector2 scrollPosition, bool alwaysShowHorizontalScrollBar, bool alwaysShowVerticalScrollBar)
		{
			this.Scroll = GUILayout.BeginScrollView(scrollPosition, alwaysShowHorizontalScrollBar, alwaysShowVerticalScrollBar, new GUILayoutOption[0]);
		}

		public GUIBeginScrollView(Vector2 scrollPosition, GUIStyle horizontalScrollBar, GUIStyle verticalScrollBar, params GUILayoutOption[] options)
		{
			this.Scroll = GUILayout.BeginScrollView(scrollPosition, horizontalScrollBar, verticalScrollBar, options);
		}

		public void Dispose()
		{
			GUILayout.EndScrollView();
		}
	}
}
