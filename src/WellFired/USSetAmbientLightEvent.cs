using System;
using UnityEngine;

namespace WellFired
{
	[USequencerEvent("Light/Set Ambient Light"), USequencerEventHideDuration, USequencerFriendlyName("Set Ambient Light")]
	public class USSetAmbientLightEvent : USEventBase
	{
		public Color lightColor = Color.red;

		private Color prevLightColor;

		public override void FireEvent()
		{
			this.prevLightColor = RenderSettings.ambientLight;
			RenderSettings.ambientLight = this.lightColor;
		}

		public override void ProcessEvent(float deltaTime)
		{
		}

		public override void StopEvent()
		{
			this.UndoEvent();
		}

		public override void UndoEvent()
		{
			RenderSettings.ambientLight = this.prevLightColor;
		}
	}
}
