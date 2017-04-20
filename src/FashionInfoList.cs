using System;
using UnityEngine;

public class FashionInfoList : BaseVoList
{
	[SerializeField]
	public FashionInfo[] list;

	public override void Destroy()
	{
		this.list = new FashionInfo[0];
	}
}
