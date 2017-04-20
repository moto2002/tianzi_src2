using System;
using UnityEngine;

public class StandardInfoList : BaseVoList
{
	[SerializeField]
	public StandardInfo[] list;

	public override void Destroy()
	{
		this.list = new StandardInfo[0];
	}
}
