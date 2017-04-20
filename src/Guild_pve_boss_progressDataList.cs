using System;
using System.Collections.Generic;
using UnityEngine;

public class Guild_pve_boss_progressDataList : BaseVoList
{
	[SerializeField]
	public Guild_pve_boss_progressDataVo[] list;

	public override void Destroy()
	{
		this.list = new Guild_pve_boss_progressDataVo[0];
	}

	public int GetAirWallIndexByBossName(List<string> bossName)
	{
		if (this.list == null)
		{
			return 0;
		}
		if (bossName == null || bossName.Count <= 1)
		{
			return 0;
		}
		int num = 0;
		for (int i = 0; i < this.list.Length; i++)
		{
			num = i;
			for (int j = 0; j < this.list[i].BossIds.Length; j++)
			{
				string item = this.list[i].BossIds[j];
				if (!bossName.Contains(item))
				{
					return num;
				}
				bossName.Remove(item);
			}
		}
		return num + 1;
	}
}
