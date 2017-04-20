using System;
using UnityEngine;

public class League_conditionDataList : BaseVoList
{
	[SerializeField]
	public League_conditionDataVo[] list;

	public override void Destroy()
	{
		this.list = new League_conditionDataVo[0];
	}
}
