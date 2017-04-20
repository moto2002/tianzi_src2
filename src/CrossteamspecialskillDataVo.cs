using System;
using System.Collections.Generic;

[Serializable]
public class CrossteamspecialskillDataVo
{
	[Serializable]
	public class SpecialskillInfo
	{
		public int Index;

		public int Count;

		public string Skillid;

		public string Name;

		public int Access;

		public string Desc;

		public string Path;
	}

	public int Group;

	public int SelectCount;

	public string SkillTypeIcon;

	public List<CrossteamspecialskillDataVo.SpecialskillInfo> Skills = new List<CrossteamspecialskillDataVo.SpecialskillInfo>();

	public void ResetCount()
	{
		this.SelectCount = 0;
		for (int i = 0; i < this.Skills.Count; i++)
		{
			this.Skills[i].Count = 0;
		}
	}
}
