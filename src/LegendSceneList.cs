using System;
using UnityEngine;

public class LegendSceneList : BaseVoList
{
	[SerializeField]
	public LegendScene[] list;

	public override void Destroy()
	{
		this.list = new LegendScene[0];
	}
}
