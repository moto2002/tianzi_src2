using System;
using System.Collections.Generic;
using UnityEngine;

public class League_awardDataList : BaseVoList
{
	[SerializeField]
	public League_awardDataVo[] list;

	public Dictionary<int, List<League_awardDataVo>> mAwards;

	public void Init()
	{
		if (this.mAwards == null)
		{
			this.mAwards = new Dictionary<int, List<League_awardDataVo>>();
		}
		for (int i = 0; i < this.list.Length; i++)
		{
			League_awardDataVo league_awardDataVo = this.list[i];
			if (this.mAwards.ContainsKey(league_awardDataVo.mMatchType))
			{
				List<League_awardDataVo> list = this.mAwards[league_awardDataVo.mMatchType];
				list.Add(league_awardDataVo);
			}
			else
			{
				List<League_awardDataVo> list = new List<League_awardDataVo>();
				list.Add(league_awardDataVo);
				this.mAwards.Add(league_awardDataVo.mMatchType, list);
			}
		}
	}

	public List<League_awardDataVo> GetLeague_awards(int type)
	{
		if (this.mAwards.ContainsKey(type))
		{
			return this.mAwards[type];
		}
		return null;
	}

	public List<LeagueAwardItem> GetLeagueAwardItemList(int type, int rank)
	{
		if (this.mAwards.ContainsKey(type))
		{
			return this.mAwards[type][rank].mAwardItemList;
		}
		return null;
	}

	public override void Destroy()
	{
		this.list = new League_awardDataVo[0];
		if (this.mAwards != null)
		{
			foreach (KeyValuePair<int, List<League_awardDataVo>> current in this.mAwards)
			{
				current.Value.Clear();
			}
			this.mAwards.Clear();
		}
	}
}
