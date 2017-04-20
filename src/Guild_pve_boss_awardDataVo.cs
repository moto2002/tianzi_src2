using System;
using System.Collections.Generic;

[Serializable]
public class Guild_pve_boss_awardDataVo
{
	[Serializable]
	public class XmlItem
	{
		public string ItemID;

		public int ItemCount;
	}

	public int ID;

	public string BossID;

	public int Diff;

	public List<Guild_pve_boss_awardDataVo.XmlItem> mAwardItemlist = new List<Guild_pve_boss_awardDataVo.XmlItem>();
}
