using System;
using UnityEngine;

public class BloodQualityUpList : BaseVoList
{
	[SerializeField]
	public BloodQualityUp[] list;

	public override void Destroy()
	{
		this.list = new BloodQualityUp[0];
	}
}
