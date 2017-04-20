using System;
using UnityEngine;

public class PetStepsUpList : BaseVoList
{
	[SerializeField]
	public PetStepsUp[] list;

	public override void Destroy()
	{
		this.list = new PetStepsUp[0];
	}
}
