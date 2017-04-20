using System;
using UnityEngine;

namespace SkillEditor.Shared
{
	public interface IUnityEditorHelper
	{
		void AddUpdateListener(Action listener);

		void RemoveUpdateListener(Action listener);

		bool IsPrefab(GameObject testObject);
	}
}
