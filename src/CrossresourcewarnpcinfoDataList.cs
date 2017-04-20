using System;
using UnityEngine;

public class CrossresourcewarnpcinfoDataList : BaseVoList
{
	[SerializeField]
	public CrossresourcewarnpcinfoDataVo[] list;

	public override void Destroy()
	{
		this.list = new CrossresourcewarnpcinfoDataVo[0];
	}
}
