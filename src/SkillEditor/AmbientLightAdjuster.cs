using System;
using UnityEngine;

namespace SkillEditor
{
	[ExecuteInEditMode]
	public class AmbientLightAdjuster : MonoBehaviour
	{
		public Color ambientLightColor = Color.red;

		private void Update()
		{
			RenderSettings.ambientLight = this.ambientLightColor;
		}
	}
}
