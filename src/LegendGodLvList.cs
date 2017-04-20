using System;
using UnityEngine;

public class LegendGodLvList : BaseVoList
{
	[SerializeField]
	public LegendGodLvClass[] list;

	public override void Destroy()
	{
		this.list = new LegendGodLvClass[0];
	}
}
