using System;
using UnityEngine;

public class RankUnitList : BaseVoList
{
	[SerializeField]
	public RankUnit[] list;

	public string RankItemID;

	public int RankMaxCount;

	public override void Destroy()
	{
		this.list = new RankUnit[0];
	}
}
