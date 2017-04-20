using System;
using UnityEngine;

namespace WellFired
{
	[USequencerEvent("Fullscreen/Display Image"), USequencerEventHideDuration, USequencerFriendlyName("Display Image")]
	public class USDisplayImageEvent : USEventBase
	{
		public UILayer uiLayer;

		public AnimationCurve fadeCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f),
			new Keyframe(3f, 1f),
			new Keyframe(4f, 0f)
		});

		public Texture2D displayImage;

		public UIPosition displayPosition;

		public UIPosition anchorPosition;

		private float currentCurveSampleTime;

		public override void FireEvent()
		{
			if (!this.displayImage)
			{
				Debug.LogWarning("Trying to use a DisplayImage Event, but you didn't give it an image to display", this);
			}
		}

		public override void ProcessEvent(float deltaTime)
		{
			this.currentCurveSampleTime = deltaTime;
		}

		public override void EndEvent()
		{
			float b = this.fadeCurve.Evaluate(this.fadeCurve.keys[this.fadeCurve.length - 1].time);
			b = Mathf.Min(Mathf.Max(0f, b), 1f);
		}

		public override void StopEvent()
		{
			this.UndoEvent();
		}

		public override void UndoEvent()
		{
			this.currentCurveSampleTime = 0f;
		}
	}
}
