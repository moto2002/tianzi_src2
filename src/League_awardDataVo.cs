using System;
using System.Collections.Generic;

[Serializable]
public class League_awardDataVo
{
	public int mMatchType;

	public int mAwardRange;

	public int mRank;

	public string mTile;

	public List<LeagueAwardItem> mAwardItemList = new List<LeagueAwardItem>();
}
