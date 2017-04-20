using System;
using UnityEngine;

public class RefineRangeList : BaseVoList
{
	[SerializeField]
	public RefineRange[] list;

	public override void Destroy()
	{
		this.list = new RefineRange[0];
	}
}
