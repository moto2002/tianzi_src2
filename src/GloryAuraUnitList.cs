using System;
using UnityEngine;

public class GloryAuraUnitList : BaseVoList
{
	[SerializeField]
	public GloryAuraUnit[] list;

	public override void Destroy()
	{
		this.list = new GloryAuraUnit[0];
	}
}
