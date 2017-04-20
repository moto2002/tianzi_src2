using System;
using UnityEngine;

public class RefineRangeInfoList : BaseVoList
{
	[SerializeField]
	public RefineRangeInfo[] list;

	public override void Destroy()
	{
		this.list = new RefineRangeInfo[0];
	}
}
