using System;
using UnityEngine;

public class CastConfigDataList : BaseVoList
{
	[SerializeField]
	public CastConfigData[] list;

	public override void Destroy()
	{
		this.list = new CastConfigData[0];
	}
}
