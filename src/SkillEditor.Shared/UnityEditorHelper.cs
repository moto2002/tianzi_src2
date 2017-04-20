using System;
using UnityEngine;

namespace SkillEditor.Shared
{
	public class UnityEditorHelper : IUnityEditorHelper
	{
		private Action listeners = delegate
		{
		};

		public void AddUpdateListener(Action listener)
		{
		}

		public void RemoveUpdateListener(Action listener)
		{
		}

		private void Update()
		{
			this.listeners();
		}

		public bool IsPrefab(GameObject testObject)
		{
			return false;
		}
	}
}
