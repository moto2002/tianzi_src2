using System;
using UnityEngine;

public class NationBoundList : BaseVoList
{
	[SerializeField]
	public NationBound[] list;

	public override void Destroy()
	{
		this.list = new NationBound[0];
	}
}
