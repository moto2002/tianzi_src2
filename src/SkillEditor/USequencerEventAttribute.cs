using System;

namespace SkillEditor
{
	public class USequencerEventAttribute : Attribute
	{
		private string eventPath;

		public string EventPath
		{
			get
			{
				return this.eventPath;
			}
		}

		public USequencerEventAttribute(string myEventPath)
		{
			this.eventPath = myEventPath;
		}
	}
}
