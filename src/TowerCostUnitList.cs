using System;
using UnityEngine;

public class TowerCostUnitList : BaseVoList
{
	[SerializeField]
	public TowerCostUnit[] list;

	public override void Destroy()
	{
		this.list = new TowerCostUnit[0];
	}
}
