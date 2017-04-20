using System;
using UnityEngine;

public class Legend_god_quenching_basepackDataList : BaseVoList
{
	[SerializeField]
	public Legend_god_quenching_basepackDataVo[] list;

	public override void Destroy()
	{
		this.list = new Legend_god_quenching_basepackDataVo[0];
	}
}
