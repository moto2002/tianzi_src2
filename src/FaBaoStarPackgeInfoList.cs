using System;
using UnityEngine;

public class FaBaoStarPackgeInfoList : BaseVoList
{
	[SerializeField]
	public FaBaoStarPackgeInfo[] list;

	public override void Destroy()
	{
		this.list = new FaBaoStarPackgeInfo[0];
	}
}
