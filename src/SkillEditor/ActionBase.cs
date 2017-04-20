using System;
using UnityEngine;

namespace SkillEditor
{
	public abstract class ActionBase : USEventBase
	{
		protected bool isPrepare;

		public virtual float prepareTime
		{
			get
			{
				return base.FireTime - 3f;
			}
		}

		public virtual bool actionEnd
		{
			get
			{
				return base.Sequence.RunningTime >= base.FireTime + base.Duration;
			}
		}

		public void Update()
		{
			if (base.Sequence != null && !this.isPrepare && base.Sequence.RunningTime >= this.prepareTime)
			{
				this.PrepareEvent();
				this.isPrepare = true;
			}
			base.Duration = Mathf.Max(1E-05f, base.Duration);
			this.UpdateEvent();
		}

		public virtual void UpdateEvent()
		{
		}

		public virtual void PrepareEvent()
		{
		}

		public override void FireEvent()
		{
		}

		public override void ProcessEvent(float runningTime)
		{
		}

		public override void EndEvent()
		{
		}
	}
}
