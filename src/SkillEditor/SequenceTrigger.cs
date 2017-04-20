using System;
using UnityEngine;

namespace SkillEditor
{
	[RequireComponent(typeof(Collider))]
	public class SequenceTrigger : MonoBehaviour
	{
		public bool isPlayerTrigger;

		public bool isMainCameraTrigger;

		public GameObject triggerObject;

		public USSequencer sequenceToPlay;

		private void OnTriggerEnter(Collider other)
		{
			if (!this.sequenceToPlay)
			{
				Debug.LogWarning("You have triggered a sequence in your scene, however, you didn't assign it a Sequence To Play", base.gameObject);
				return;
			}
			if (this.sequenceToPlay.IsPlaying)
			{
				return;
			}
			if (other.CompareTag("MainCamera") && this.isMainCameraTrigger)
			{
				this.sequenceToPlay.Play();
				return;
			}
			if (other.CompareTag("Player") && this.isPlayerTrigger)
			{
				this.sequenceToPlay.Play();
				return;
			}
			if (other.gameObject == this.triggerObject)
			{
				this.sequenceToPlay.Play();
				return;
			}
		}
	}
}
