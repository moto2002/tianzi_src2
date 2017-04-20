using System;
using UnityEngine;

public class GloryPackageList : BaseVoList
{
	[SerializeField]
	public GloryPackage[] list;

	public override void Destroy()
	{
		this.list = new GloryPackage[0];
	}
}
