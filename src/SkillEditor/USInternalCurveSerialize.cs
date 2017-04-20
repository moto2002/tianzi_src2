using System;
using System.Collections.Generic;
using UnityEngine;

namespace SkillEditor
{
	[Serializable]
	public class USInternalCurveSerialize
	{
		public AnimationCurve animationCurve = new AnimationCurve();

		public List<USInternalKeyframeSerialize> internalSerializes = new List<USInternalKeyframeSerialize>();

		public bool useCurrentValue;

		public float duration;

		public void Copy(USInternalCurve curve)
		{
			this.duration = curve.Duration;
			this.animationCurve = curve.UnityAnimationCurve;
			this.useCurrentValue = curve.UseCurrentValue;
			if (curve.Keys.Count > 0)
			{
				for (int i = 0; i < curve.Keys.Count; i++)
				{
					USInternalKeyframe uSInternalKeyframe = curve.Keys[i];
					if (!(uSInternalKeyframe == null))
					{
						USInternalKeyframeSerialize uSInternalKeyframeSerialize = new USInternalKeyframeSerialize();
						uSInternalKeyframeSerialize.Copy(uSInternalKeyframe);
						this.internalSerializes.Add(uSInternalKeyframeSerialize);
					}
				}
			}
		}
	}
}
