using System;
using UnityEngine;

public class PetLevelUpList : BaseVoList
{
	[SerializeField]
	public PetLevelUp[] list;

	public override void Destroy()
	{
		this.list = new PetLevelUp[0];
	}
}
