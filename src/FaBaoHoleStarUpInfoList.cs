using System;
using UnityEngine;

public class FaBaoHoleStarUpInfoList : BaseVoList
{
	[SerializeField]
	public FaBaoHoleStarUpInfo[] list;

	public int MaxStarLv;

	public int MaxHoleNums;

	public override void Destroy()
	{
		this.list = new FaBaoHoleStarUpInfo[0];
	}
}
