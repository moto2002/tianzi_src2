using System;
using UnityEngine;
using UnityEngine.UI;

namespace SkillEditor
{
	[RequireComponent(typeof(Button))]
	public class PlayPauseSequenceToggleButton : MonoBehaviour
	{
		[SerializeField]
		private string pausedText = "Play";

		[SerializeField]
		private string playingText = "Pause";

		[SerializeField]
		private USSequencer sequenceToPlay;

		private Text cachedButtonLabel;

		private void Start()
		{
			Button component = base.GetComponent<Button>();
			if (!component)
			{
				Debug.LogError("The component Play Sequence button must be added to a Unity UI Button");
				return;
			}
			if (!this.sequenceToPlay)
			{
				Debug.LogError("The Sequence to play field must be hooked up in the Inspector");
				return;
			}
			component.onClick.AddListener(delegate
			{
				this.ToggleSequence();
			});
			this.cachedButtonLabel = component.GetComponentInChildren<Text>();
		}

		private void Update()
		{
			if (this.sequenceToPlay.IsPlaying)
			{
				this.cachedButtonLabel.text = this.playingText;
			}
			else
			{
				this.cachedButtonLabel.text = this.pausedText;
			}
		}

		private void ToggleSequence()
		{
			if (this.sequenceToPlay.RunningTime >= this.sequenceToPlay.Duration)
			{
				this.sequenceToPlay.RunningTime = 0f;
				this.sequenceToPlay.Play();
			}
			else if (this.sequenceToPlay.IsPlaying)
			{
				this.sequenceToPlay.Pause();
			}
			else
			{
				this.sequenceToPlay.Play();
			}
		}
	}
}
