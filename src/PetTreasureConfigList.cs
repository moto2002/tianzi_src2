using System;
using UnityEngine;

public class PetTreasureConfigList : BaseVoList
{
	[SerializeField]
	public PetTreasureConfig[] list;

	public override void Destroy()
	{
		this.list = new PetTreasureConfig[0];
	}
}
