using System;
using UnityEngine;

public class ArmyLifeInfoList : BaseVoList
{
	[SerializeField]
	public ArmyLifeInfo[] list;

	public override void Destroy()
	{
		this.list = new ArmyLifeInfo[0];
	}
}
