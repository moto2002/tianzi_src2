using System;

namespace SkillEditor
{
	public class USequencerFriendlyNameAttribute : Attribute
	{
		private string friendlyName;

		public string FriendlyName
		{
			get
			{
				return this.friendlyName;
			}
		}

		public USequencerFriendlyNameAttribute(string myFriendlyName)
		{
			this.friendlyName = myFriendlyName;
		}
	}
}
