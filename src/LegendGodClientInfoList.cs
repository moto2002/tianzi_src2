using System;
using UnityEngine;

public class LegendGodClientInfoList : BaseVoList
{
	[SerializeField]
	public LegendGodClientInfoClass[] list;

	public override void Destroy()
	{
		this.list = new LegendGodClientInfoClass[0];
	}
}
