using System;
using System.Collections.Generic;
using UnityEngine;

public class Guild_pve_boss_infoDataList : BaseVoList
{
	[SerializeField]
	public Guild_pve_boss_infoDataVo[] list;

	public Guild_pve_boss_infoDataVo GetBossProgress(List<string> bossName)
	{
		if (this.list == null)
		{
			return null;
		}
		if (bossName == null || bossName.Count < 1)
		{
			return this.list[0];
		}
		List<Guild_pve_boss_infoDataVo> list = new List<Guild_pve_boss_infoDataVo>(this.list);
		for (int i = 0; i < bossName.Count; i++)
		{
			for (int j = 0; j < list.Count; j++)
			{
				int num = AssetFileUtils.IntParse(bossName[i], 0);
				if (list[j].ID == num)
				{
					list.Remove(list[j]);
					break;
				}
			}
		}
		if (list.Count == 0)
		{
			return null;
		}
		return list[0];
	}

	public override void Destroy()
	{
		this.list = new Guild_pve_boss_infoDataVo[0];
	}
}
