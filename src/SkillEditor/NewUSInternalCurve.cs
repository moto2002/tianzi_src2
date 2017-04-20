using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SkillEditor
{
	public class NewUSInternalCurve
	{
		public AnimationCurve animationCurve;

		public List<NewUSInternalKeyframe> internalKeyframes = new List<NewUSInternalKeyframe>();

		public bool useCurrentValue;

		public float duration;

		public float Duration
		{
			get
			{
				return this.duration;
			}
			set
			{
				this.duration = value;
			}
		}

		public float FirstKeyframeTime
		{
			get
			{
				if (this.internalKeyframes.Count == 0)
				{
					return 0f;
				}
				return this.internalKeyframes[0].Time;
			}
		}

		public float LastKeyframeTime
		{
			get
			{
				if (this.internalKeyframes.Count == 0)
				{
					return this.Duration;
				}
				return this.internalKeyframes[this.internalKeyframes.Count - 1].Time;
			}
		}

		public AnimationCurve UnityAnimationCurve
		{
			get
			{
				return this.animationCurve;
			}
			set
			{
				this.animationCurve = new AnimationCurve();
				Keyframe[] keys = value.keys;
				for (int i = 0; i < keys.Length; i++)
				{
					Keyframe key = keys[i];
					this.animationCurve.AddKey(key);
				}
				this.BuildInternalCurveFromAnimationCurve();
			}
		}

		public List<NewUSInternalKeyframe> Keys
		{
			get
			{
				return this.internalKeyframes;
			}
		}

		public bool UseCurrentValue
		{
			get
			{
				return this.useCurrentValue;
			}
			set
			{
				this.useCurrentValue = value;
			}
		}

		public void Init(USInternalCurveSerialize fSerialize)
		{
			this.duration = fSerialize.duration;
			this.animationCurve = fSerialize.animationCurve;
			this.useCurrentValue = fSerialize.useCurrentValue;
			if (fSerialize.internalSerializes.Count > 0)
			{
				for (int i = 0; i < fSerialize.internalSerializes.Count; i++)
				{
					USInternalKeyframeSerialize fSerialize2 = fSerialize.internalSerializes[i];
					NewUSInternalKeyframe newUSInternalKeyframe = new NewUSInternalKeyframe();
					newUSInternalKeyframe.curve = this;
					newUSInternalKeyframe.Init(fSerialize2);
					this.internalKeyframes.Add(newUSInternalKeyframe);
				}
			}
		}

		public static int KeyframeComparer(NewUSInternalKeyframe a, NewUSInternalKeyframe b)
		{
			if (a == null)
			{
				return -1;
			}
			if (b == null)
			{
				return 1;
			}
			return a.Time.CompareTo(b.Time);
		}

		private void OnEnable()
		{
			if (this.internalKeyframes == null)
			{
				this.internalKeyframes = new List<NewUSInternalKeyframe>();
				this.Duration = 10f;
			}
			else if (this.internalKeyframes.Count > 0)
			{
				this.Duration = (from keyframe in this.internalKeyframes
				orderby keyframe.Time
				select keyframe).LastOrDefault<NewUSInternalKeyframe>().Time;
			}
			if (this.UnityAnimationCurve == null)
			{
				this.UnityAnimationCurve = new AnimationCurve();
			}
		}

		public float Evaluate(float time)
		{
			return this.animationCurve.Evaluate(time);
		}

		public void BuildInternalCurveFromAnimationCurve()
		{
			Keyframe[] keys = this.animationCurve.keys;
			for (int i = 0; i < keys.Length; i++)
			{
				Keyframe keyframe = keys[i];
				NewUSInternalKeyframe newUSInternalKeyframe = null;
				foreach (NewUSInternalKeyframe current in this.internalKeyframes)
				{
					if (Mathf.Approximately(keyframe.time, current.Time))
					{
						newUSInternalKeyframe = current;
						break;
					}
				}
				if (newUSInternalKeyframe != null)
				{
					newUSInternalKeyframe.ConvertFrom(keyframe);
				}
				else
				{
					NewUSInternalKeyframe newUSInternalKeyframe2 = new NewUSInternalKeyframe();
					newUSInternalKeyframe2.ConvertFrom(keyframe);
					newUSInternalKeyframe2.curve = this;
					this.internalKeyframes.Add(newUSInternalKeyframe2);
					this.internalKeyframes.Sort(new Comparison<NewUSInternalKeyframe>(NewUSInternalCurve.KeyframeComparer));
				}
			}
		}

		public void BuildAnimationCurveFromInternalCurve()
		{
			while (this.animationCurve.keys.Length > 0)
			{
				this.animationCurve.RemoveKey(0);
			}
			Keyframe key = default(Keyframe);
			foreach (NewUSInternalKeyframe current in this.Keys)
			{
				if (current != null)
				{
					key.value = current.Value;
					key.time = current.Time;
					key.inTangent = current.InTangent;
					key.outTangent = current.OutTangent;
					this.animationCurve.AddKey(key);
				}
			}
			this.internalKeyframes.Sort(new Comparison<NewUSInternalKeyframe>(NewUSInternalCurve.KeyframeComparer));
		}

		public void ValidateKeyframeTimes()
		{
			for (int i = this.Keys.Count - 1; i >= 0; i--)
			{
				if (i != this.Keys.Count - 1)
				{
					if (Mathf.Approximately(this.Keys[i].Time, this.Keys[i + 1].Time))
					{
						this.internalKeyframes.Remove(this.Keys[i]);
					}
				}
			}
		}

		public NewUSInternalKeyframe AddKeyframe(float time, float value)
		{
			NewUSInternalKeyframe newUSInternalKeyframe = null;
			foreach (NewUSInternalKeyframe current in this.internalKeyframes)
			{
				if (Mathf.Approximately(current.Time, time))
				{
					newUSInternalKeyframe = current;
				}
				if (newUSInternalKeyframe != null)
				{
					break;
				}
			}
			if (newUSInternalKeyframe == null)
			{
				newUSInternalKeyframe = new NewUSInternalKeyframe();
				this.internalKeyframes.Add(newUSInternalKeyframe);
			}
			this.Duration = Mathf.Max((from keyframe in this.internalKeyframes
			orderby keyframe.Time
			select keyframe).LastOrDefault<NewUSInternalKeyframe>().Time, time);
			newUSInternalKeyframe.curve = this;
			newUSInternalKeyframe.Time = time;
			newUSInternalKeyframe.Value = value;
			newUSInternalKeyframe.InTangent = 0f;
			newUSInternalKeyframe.OutTangent = 0f;
			this.internalKeyframes.Sort(new Comparison<NewUSInternalKeyframe>(NewUSInternalCurve.KeyframeComparer));
			this.BuildAnimationCurveFromInternalCurve();
			return newUSInternalKeyframe;
		}

		public void RemoveKeyframe(NewUSInternalKeyframe internalKeyframe)
		{
			for (int i = this.internalKeyframes.Count - 1; i >= 0; i--)
			{
				if (this.internalKeyframes[i] == internalKeyframe)
				{
					this.internalKeyframes.RemoveAt(i);
				}
			}
			this.internalKeyframes.Sort(new Comparison<NewUSInternalKeyframe>(NewUSInternalCurve.KeyframeComparer));
			this.Duration = (from keyframe in this.internalKeyframes
			orderby keyframe.Time
			select keyframe).LastOrDefault<NewUSInternalKeyframe>().Time;
			this.BuildAnimationCurveFromInternalCurve();
		}

		public NewUSInternalKeyframe GetNextKeyframe(NewUSInternalKeyframe keyframe)
		{
			if (this.internalKeyframes.Count == 0)
			{
				return null;
			}
			if (this.internalKeyframes[this.internalKeyframes.Count - 1] == keyframe)
			{
				return null;
			}
			int num = -1;
			for (int i = 0; i < this.internalKeyframes.Count; i++)
			{
				if (this.internalKeyframes[i] == keyframe)
				{
					num = i;
				}
				if (num != -1)
				{
					break;
				}
			}
			return this.internalKeyframes[num + 1];
		}

		public NewUSInternalKeyframe GetPrevKeyframe(NewUSInternalKeyframe keyframe)
		{
			if (this.internalKeyframes.Count == 0)
			{
				return null;
			}
			if (this.internalKeyframes[0] == keyframe)
			{
				return null;
			}
			int num = -1;
			for (int i = 0; i < this.internalKeyframes.Count; i++)
			{
				if (this.internalKeyframes[i] == keyframe)
				{
					num = i;
				}
				if (num != -1)
				{
					break;
				}
			}
			return this.internalKeyframes[num - 1];
		}

		public bool Contains(NewUSInternalKeyframe keyframe)
		{
			for (int i = this.Keys.Count - 1; i >= 0; i--)
			{
				if (this.Keys[i] == keyframe)
				{
					return true;
				}
			}
			return false;
		}

		public float FindNextKeyframeTime(float time)
		{
			float time2 = this.Duration;
			for (int i = this.Keys.Count - 1; i >= 0; i--)
			{
				if (this.Keys[i].Time < time2 && this.Keys[i].Time > time)
				{
					time2 = this.Keys[i].Time;
				}
			}
			return time2;
		}

		public float FindPrevKeyframeTime(float time)
		{
			float num = 0f;
			for (int i = 0; i < this.Keys.Count; i++)
			{
				if (this.Keys[i].Time > num && this.Keys[i].Time < time)
				{
					num = this.Keys[i].Time;
				}
			}
			return num;
		}

		public bool CanSetKeyframeToTime(float newTime)
		{
			foreach (NewUSInternalKeyframe current in this.internalKeyframes)
			{
				if (Mathf.Approximately(current.Time, newTime))
				{
					return false;
				}
			}
			return true;
		}
	}
}
