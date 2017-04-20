using System;
using UnityEngine;

public class DataList : BaseVoList
{
	[SerializeField]
	public Data[] list;

	public override void Destroy()
	{
		this.list = new Data[0];
	}
}
