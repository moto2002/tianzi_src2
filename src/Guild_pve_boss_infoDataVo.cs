using System;
using System.Collections.Generic;

[Serializable]
public class Guild_pve_boss_infoDataVo
{
	[Serializable]
	public class XmlItem
	{
		public string ItemID;

		public string ItemDes;
	}

	public int ID;

	public string BossID;

	public string Icon;

	public float Scale;

	public float PosX;

	public float PosY;

	public float PosZ;

	public float RoateY;

	public string Strategy;

	public bool bFlag;

	public List<Guild_pve_boss_infoDataVo.XmlItem> mSkillItemlist = new List<Guild_pve_boss_infoDataVo.XmlItem>();
}
