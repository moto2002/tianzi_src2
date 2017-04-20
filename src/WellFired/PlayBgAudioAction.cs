using System;
using UnityEngine;

namespace WellFired
{
	[USequencerEvent("Custom Event/Play BackGround Audio Action"), USequencerFriendlyName("Play BackGround Audio Action")]
	public class PlayBgAudioAction : ActionBase
	{
		public string audioId = string.Empty;

		public AudioClip audioClip;

		public override void UpdateEvent()
		{
			if (this.audioClip != null)
			{
				base.Duration = this.audioClip.length;
			}
			this.audioClip = null;
		}

		public override void FireEvent()
		{
			if (SequenceManager.GetInstance().PlayAudio != null)
			{
				SequenceManager.GetInstance().PlayBgAudio(this.audioId);
			}
		}

		public override void StopEvent()
		{
			this.UndoEvent();
		}

		public override void EndEvent()
		{
			this.UndoEvent();
		}

		public override void UndoEvent()
		{
			if (SequenceManager.GetInstance().StopAudio != null)
			{
				SequenceManager.GetInstance().PlayBgSoundFunc(true);
			}
		}
	}
}
