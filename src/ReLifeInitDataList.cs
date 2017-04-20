using System;
using UnityEngine;

public class ReLifeInitDataList : BaseVoList
{
	[SerializeField]
	public ReLifeInitData[] list;

	public override void Destroy()
	{
		this.list = new ReLifeInitData[0];
	}
}
