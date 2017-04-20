using System;
using UnityEngine;

public class LocalRideList : BaseVoList
{
	[SerializeField]
	public LocalRideVo[] list;

	public override void Destroy()
	{
		this.list = new LocalRideVo[0];
	}
}
