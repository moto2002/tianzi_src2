using System;
using UnityEngine;

public class PetChangeModelInfoList : BaseVoList
{
	[SerializeField]
	public PetChangeModelInfo[] list;

	public override void Destroy()
	{
		this.list = new PetChangeModelInfo[0];
	}
}
