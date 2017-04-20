using System;

namespace WellFired
{
	[USequencerEvent("Custom Event/Show Mask Action"), USequencerFriendlyName("Show Mask Action")]
	public class ShowMaskAction : ActionBase
	{
		public override void FireEvent()
		{
			if (SequenceManager.GetInstance().ShowMaskPanel != null)
			{
				SequenceManager.GetInstance().ShowMaskPanel();
			}
		}
	}
}
