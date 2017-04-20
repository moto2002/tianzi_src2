using System;
using UnityEngine;

public class LegendGodStepClassList : BaseVoList
{
	[SerializeField]
	public LegendGodStepClass[] list;

	public override void Destroy()
	{
		this.list = new LegendGodStepClass[0];
	}
}
