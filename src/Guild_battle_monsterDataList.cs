using System;
using System.Collections.Generic;
using UnityEngine;

public class Guild_battle_monsterDataList : BaseVoList
{
	[SerializeField]
	public Guild_battle_monsterDataVo[] list;

	public override void Destroy()
	{
		this.list = new Guild_battle_monsterDataVo[0];
	}

	public Guild_battle_monsterDataVo GetBossProgress(List<string> bossName)
	{
		if (this.list == null)
		{
			return null;
		}
		if (bossName == null || bossName.Count < 1)
		{
			return this.list[0];
		}
		List<Guild_battle_monsterDataVo> list = new List<Guild_battle_monsterDataVo>(this.list);
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
}
