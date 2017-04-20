using System;

namespace WellFired
{
	[USequencerEvent("Custom Event/Talk Action"), USequencerFriendlyName("Talk Action")]
	public class TalkAction : ActionBase
	{
		public string talkId;

		public override void FireEvent()
		{
			base.FireEvent();
			if (SequenceManager.GetInstance().isPlaying && !string.IsNullOrEmpty(this.talkId) && SequenceManager.GetInstance().ShowDailogPanel != null)
			{
				SequenceManager.GetInstance().ShowDailogPanel(this.talkId);
			}
		}

		public override void StopEvent()
		{
			if (SequenceManager.GetInstance().HideDailogPanel != null)
			{
				SequenceManager.GetInstance().HideDailogPanel();
			}
		}
	}
}
