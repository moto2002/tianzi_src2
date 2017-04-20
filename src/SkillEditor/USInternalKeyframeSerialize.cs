using System;

namespace SkillEditor
{
	[Serializable]
	public class USInternalKeyframeSerialize
	{
		public float value;

		public float time;

		public float inTangent;

		public float outTangent;

		public bool brokenTangents;

		public void Copy(USInternalKeyframe keyFrame)
		{
			this.value = keyFrame.Value;
			this.time = keyFrame.Time;
			this.inTangent = keyFrame.InTangent;
			this.outTangent = keyFrame.OutTangent;
			this.brokenTangents = keyFrame.BrokenTangents;
		}
	}
}
