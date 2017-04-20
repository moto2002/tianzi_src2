using System;
using UnityEngine;

public class CrossRankAwardList : BaseVoList
{
	[SerializeField]
	public CrossRankAward[] list;

	public override void Destroy()
	{
		this.list = new CrossRankAward[0];
	}
}
