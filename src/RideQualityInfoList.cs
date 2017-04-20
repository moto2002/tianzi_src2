using System;
using UnityEngine;

public class RideQualityInfoList : BaseVoList
{
	[SerializeField]
	public RideQualityInfo[] list;

	public override void Destroy()
	{
		this.list = new RideQualityInfo[0];
	}
}
