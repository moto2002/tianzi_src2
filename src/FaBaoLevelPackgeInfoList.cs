using System;
using UnityEngine;

public class FaBaoLevelPackgeInfoList : BaseVoList
{
	[SerializeField]
	public FaBaoLevelPackgeInfo[] list;

	public override void Destroy()
	{
		this.list = new FaBaoLevelPackgeInfo[0];
	}
}
