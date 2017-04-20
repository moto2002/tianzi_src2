using System;
using UnityEngine;

public class Guild_pve_boss_awardDataList : BaseVoList
{
	[SerializeField]
	public Guild_pve_boss_awardDataVo[] list;

	public override void Destroy()
	{
		this.list = new Guild_pve_boss_awardDataVo[0];
	}
}
