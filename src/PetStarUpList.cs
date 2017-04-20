using System;
using UnityEngine;

public class PetStarUpList : BaseVoList
{
	[SerializeField]
	public PetStarUp[] list;

	public override void Destroy()
	{
		this.list = new PetStarUp[0];
	}
}
