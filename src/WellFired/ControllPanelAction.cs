using System;
using UnityEngine;

namespace WellFired
{
	[USequencerEvent("Custom Event/Controll Panel Action"), USequencerFriendlyName("Controll Panel Action")]
	public class ControllPanelAction : ActionBase
	{
		public bool isCustomTime;

		public override void UpdateEvent()
		{
			if (!this.isCustomTime)
			{
				base.FireTime = 1f;
				base.Duration = base.Sequence.Duration - 1f;
			}
		}

		public override void FireEvent()
		{
			base.FireEvent();
			if (Application.isPlaying && SequenceManager.GetInstance().ShowControllPanel != null)
			{
				SequenceManager.GetInstance().ShowControllPanel();
			}
		}

		public override void EndEvent()
		{
			this.StopEvent();
		}

		public override void StopEvent()
		{
			this.UndoEvent();
		}

		public override void UndoEvent()
		{
			if (Application.isPlaying && SequenceManager.GetInstance().HideControllPanel != null)
			{
				SequenceManager.GetInstance().HideControllPanel();
			}
		}
	}
}
