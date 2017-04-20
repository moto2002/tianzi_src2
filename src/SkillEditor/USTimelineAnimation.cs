using System;
using System.Collections.Generic;
using UnityEngine;

namespace SkillEditor
{
	[Serializable]
	public class USTimelineAnimation : USTimelineBase
	{
		private Dictionary<int, AnimatorStateInfo> initialAnimatorStateInfo = new Dictionary<int, AnimatorStateInfo>();

		[SerializeField]
		private List<AnimationTrack> animationsTracks = new List<AnimationTrack>();

		[SerializeField]
		private Animator animator;

		[SerializeField]
		private USTimelineAnimationEditorRunner editorRunner;

		[SerializeField]
		private USTimelineAnimationGameRunner gameRunner;

		[SerializeField]
		private AnimationTimelineController animationTimelineController;

		[SerializeField]
		private Vector3 sourcePosition;

		[SerializeField]
		private Quaternion sourceOrientation;

		[SerializeField]
		private float sourceSpeed;

		private bool previousEnabled;

		public List<AnimationTrack> AnimationTracks
		{
			get
			{
				return this.animationsTracks;
			}
			private set
			{
				this.animationsTracks = value;
			}
		}

		private Animator Animator
		{
			get
			{
				if (this.animator == null)
				{
					this.animator = base.AffectedObject.GetComponent<Animator>();
				}
				return this.animator;
			}
		}

		private USTimelineAnimationEditorRunner EditorRunner
		{
			get
			{
				if (this.editorRunner == null)
				{
					this.editorRunner = ScriptableObject.CreateInstance<USTimelineAnimationEditorRunner>();
					this.editorRunner.Animator = this.Animator;
					this.editorRunner.AnimationTimeline = this;
				}
				return this.editorRunner;
			}
		}

		private USTimelineAnimationGameRunner GameRunner
		{
			get
			{
				if (this.gameRunner == null)
				{
					this.gameRunner = ScriptableObject.CreateInstance<USTimelineAnimationGameRunner>();
					this.gameRunner.Animator = this.Animator;
					this.gameRunner.AnimationTimeline = this;
				}
				return this.gameRunner;
			}
		}

		public override void StartTimeline()
		{
			throw new NotImplementedException();
		}

		public override void StopTimeline()
		{
			throw new NotImplementedException();
		}

		public override void EndTimeline()
		{
			base.EndTimeline();
			this.Animator.enabled = this.previousEnabled;
		}

		public void ResetAnimation()
		{
			throw new NotImplementedException();
		}

		public override void Process(float sequenceTime, float playbackRate)
		{
			bool flag = true;
			if (flag)
			{
				this.EditorRunner.Process(sequenceTime, playbackRate);
			}
			else
			{
				this.GameRunner.Process(sequenceTime, playbackRate);
			}
		}

		public override void PauseTimeline()
		{
			bool flag = true;
			if (flag)
			{
				this.EditorRunner.PauseTimeline();
			}
			else
			{
				this.GameRunner.PauseTimeline();
			}
		}

		public void AddTrack(AnimationTrack animationTrack)
		{
			this.animationsTracks.Add(animationTrack);
		}

		public void RemoveTrack(AnimationTrack animationTrack)
		{
			this.animationsTracks.Remove(animationTrack);
		}

		public void SetTracks(List<AnimationTrack> animationTracks)
		{
			this.AnimationTracks = animationTracks;
		}
	}
}
