using System;
using UnityEngine;

public class RegionBoundList : BaseVoList
{
	[SerializeField]
	public RegionBound[] list;

	public override void Destroy()
	{
		this.list = new RegionBound[0];
	}
}
