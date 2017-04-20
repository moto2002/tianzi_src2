using System;
using System.Collections.Generic;

[Serializable]
public class Guild_building_devil_configDataVo
{
	[Serializable]
	public class AwardItem
	{
		public string itemID = string.Empty;

		public int num = -1;
	}

	[Serializable]
	public class KillRankAward
	{
		public int minRank = -1;

		public int maxRank = -1;

		public List<Guild_building_devil_configDataVo.AwardItem> AwardItems = new List<Guild_building_devil_configDataVo.AwardItem>();
	}

	public int Level = -1;

	public int NeedMaterial = -1;

	public List<string> bossIdList = new List<string>();

	public List<Guild_building_devil_configDataVo.AwardItem> devilAward = new List<Guild_building_devil_configDataVo.AwardItem>();

	public int Scale = -1;

	public List<Guild_building_devil_configDataVo.KillRankAward> RankAwards = new List<Guild_building_devil_configDataVo.KillRankAward>();
}
