using System;
using UnityEngine;

public class MergeItemInfoList : BaseVoList
{
	[SerializeField]
	public MergeItemInfo[] list;

	public override void Destroy()
	{
		this.list = new MergeItemInfo[0];
	}
}
