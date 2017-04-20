using System;

namespace SkillEditor
{
	public class NewModification
	{
		public NewUSInternalCurve curve;

		public float newTime;

		public float newValue;

		public NewModification(NewUSInternalCurve curve, float newTime, float newValue)
		{
			this.curve = curve;
			this.newTime = newTime;
			this.newValue = newValue;
		}
	}
}
