using System;
using UnityEngine;

namespace SkillEditor
{
	public class AutoPlaySequence : MonoBehaviour
	{
		public USSequencer sequence;

		public float delay = 1f;

		private float currentTime;

		private bool hasPlayed;

		private void Start()
		{
			if (!this.sequence)
			{
				Debug.LogError("You have added an AutoPlaySequence, however you haven't assigned it a sequence", base.gameObject);
				return;
			}
		}

		private void Update()
		{
			if (this.hasPlayed)
			{
				return;
			}
			this.currentTime += Time.deltaTime;
			if (this.currentTime >= this.delay && this.sequence)
			{
				this.sequence.Play();
				this.hasPlayed = true;
			}
		}
	}
}
