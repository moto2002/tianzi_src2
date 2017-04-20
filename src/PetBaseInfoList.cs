using System;
using UnityEngine;

public class PetBaseInfoList : BaseVoList
{
	[SerializeField]
	public PetBaseInfo[] list;

	public override void Destroy()
	{
		this.list = new PetBaseInfo[0];
	}
}
