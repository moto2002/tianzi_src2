using System;
using UnityEngine;
using UnityEngine.UI;

namespace SkillEditor
{
	[RequireComponent(typeof(Button))]
	public class ReplaySequenceButton : MonoBehaviour
	{
		[SerializeField]
		private USSequencer sequenceToPlay;

		[SerializeField]
		private bool manageInteractiveState = true;

		private void Start()
		{
			Button button = base.GetComponent<Button>();
			if (!button)
			{
				Debug.LogError("The component Play Sequence button must be added to a Unity UI Button");
				return;
			}
			if (!this.sequenceToPlay)
			{
				Debug.LogError("The Sequence to play field must be hooked up in the Inspector");
				return;
			}
			button.onClick.AddListener(delegate
			{
				this.PlaySequence();
			});
			button.interactable = (this.sequenceToPlay.RunningTime > this.sequenceToPlay.Duration);
			if (this.manageInteractiveState)
			{
				USSequencer expr_9F = this.sequenceToPlay;
				expr_9F.PlaybackStarted = (USSequencer.PlaybackDelegate)Delegate.Combine(expr_9F.PlaybackStarted, new USSequencer.PlaybackDelegate(delegate(USSequencer sequence)
				{
					button.interactable = false;
				}));
				USSequencer expr_C6 = this.sequenceToPlay;
				expr_C6.PlaybackPaused = (USSequencer.PlaybackDelegate)Delegate.Combine(expr_C6.PlaybackPaused, new USSequencer.PlaybackDelegate(delegate(USSequencer sequence)
				{
					button.interactable = false;
				}));
				USSequencer expr_ED = this.sequenceToPlay;
				expr_ED.PlaybackFinished = (USSequencer.PlaybackDelegate)Delegate.Combine(expr_ED.PlaybackFinished, new USSequencer.PlaybackDelegate(delegate(USSequencer sequence)
				{
					button.interactable = true;
				}));
				USSequencer expr_114 = this.sequenceToPlay;
				expr_114.PlaybackStopped = (USSequencer.PlaybackDelegate)Delegate.Combine(expr_114.PlaybackStopped, new USSequencer.PlaybackDelegate(delegate(USSequencer sequence)
				{
					button.interactable = false;
				}));
			}
		}

		private void RunningTimeUpdated(USSequencer sequence)
		{
			Button component = base.GetComponent<Button>();
			bool flag = USRuntimeUtility.CanPlaySequence(sequence);
			component.interactable = flag;
			Debug.Log(flag);
		}

		private void PlaySequence()
		{
			if (this.sequenceToPlay.RunningTime >= this.sequenceToPlay.Duration)
			{
				this.sequenceToPlay.RunningTime = 0f;
			}
			this.sequenceToPlay.Play();
		}
	}
}
