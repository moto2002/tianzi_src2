using System;
using UnityEngine;

namespace SkillEditor
{
	public class NewUSInternalKeyframe
	{
		public float value;

		public float time;

		public float inTangent;

		public float outTangent;

		public bool brokenTangents;

		public NewUSInternalCurve curve;

		public float Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
				if (this.curve != null)
				{
					this.curve.BuildAnimationCurveFromInternalCurve();
				}
			}
		}

		public float Time
		{
			get
			{
				return this.time;
			}
			set
			{
				this.time = value;
				if (this.curve != null)
				{
					this.time = Mathf.Max(0f, this.time);
					if (this.curve.Duration < this.time)
					{
						this.curve.Duration = this.time;
					}
					this.curve.BuildAnimationCurveFromInternalCurve();
				}
			}
		}

		public float InTangent
		{
			get
			{
				return this.inTangent;
			}
			set
			{
				this.inTangent = value;
				if (this.curve != null)
				{
					this.curve.BuildAnimationCurveFromInternalCurve();
				}
			}
		}

		public float OutTangent
		{
			get
			{
				return this.outTangent;
			}
			set
			{
				this.outTangent = value;
				if (this.curve != null)
				{
					this.curve.BuildAnimationCurveFromInternalCurve();
				}
			}
		}

		public bool BrokenTangents
		{
			get
			{
				return this.brokenTangents;
			}
			set
			{
				this.brokenTangents = value;
				if (this.curve != null)
				{
					this.curve.BuildAnimationCurveFromInternalCurve();
				}
			}
		}

		public void Init(USInternalKeyframeSerialize fSerialize)
		{
			this.time = fSerialize.time;
			this.value = fSerialize.value;
			this.inTangent = fSerialize.inTangent;
			this.outTangent = fSerialize.outTangent;
			this.brokenTangents = fSerialize.brokenTangents;
		}

		private void OnEnable()
		{
		}

		public void ConvertFrom(Keyframe keyframe)
		{
			this.value = keyframe.value;
			this.time = keyframe.time;
			this.inTangent = keyframe.inTangent;
			this.outTangent = keyframe.outTangent;
		}

		public void Smooth()
		{
			if (this.curve == null)
			{
				return;
			}
			int num = -1;
			for (int i = 0; i < this.curve.UnityAnimationCurve.keys.Length; i++)
			{
				Keyframe keyframe = this.curve.UnityAnimationCurve.keys[i];
				if (Mathf.Approximately(keyframe.time, this.time))
				{
					num = i;
				}
				if (num != -1)
				{
					break;
				}
			}
			if (num == -1)
			{
				return;
			}
			this.curve.UnityAnimationCurve.SmoothTangents(num, 0f);
			this.curve.BuildInternalCurveFromAnimationCurve();
		}

		public void Flatten()
		{
			if (this.curve == null)
			{
				return;
			}
			this.inTangent = 0f;
			this.outTangent = 0f;
			this.curve.BuildAnimationCurveFromInternalCurve();
		}

		public void RightTangentLinear()
		{
			if (this.curve == null)
			{
				return;
			}
			NewUSInternalKeyframe nextKeyframe = this.curve.GetNextKeyframe(this);
			if (nextKeyframe == null)
			{
				return;
			}
			this.outTangent = (nextKeyframe.value - this.value) / (nextKeyframe.time - this.time);
			this.brokenTangents = true;
			this.curve.BuildAnimationCurveFromInternalCurve();
		}

		public void RightTangentConstant()
		{
			this.outTangent = float.PositiveInfinity;
			this.curve.BuildAnimationCurveFromInternalCurve();
		}

		public void LeftTangentLinear()
		{
			if (this.curve == null)
			{
				return;
			}
			NewUSInternalKeyframe prevKeyframe = this.curve.GetPrevKeyframe(this);
			if (prevKeyframe == null)
			{
				return;
			}
			this.inTangent = (prevKeyframe.value - this.value) / (prevKeyframe.time - this.time);
			this.brokenTangents = true;
			this.curve.BuildAnimationCurveFromInternalCurve();
		}

		public void LeftTangentConstant()
		{
			this.inTangent = float.PositiveInfinity;
			this.curve.BuildAnimationCurveFromInternalCurve();
		}

		public void BothTangentLinear()
		{
			this.LeftTangentLinear();
			this.RightTangentLinear();
		}

		public void BothTangentConstant()
		{
			this.LeftTangentConstant();
			this.RightTangentConstant();
		}
	}
}
