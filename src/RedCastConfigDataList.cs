using System;
using UnityEngine;

public class RedCastConfigDataList : BaseVoList
{
	[SerializeField]
	public RedCastConfigData[] list;

	public override void Destroy()
	{
		this.list = new RedCastConfigData[0];
	}
}
