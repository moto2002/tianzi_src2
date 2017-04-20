using System;
using UnityEngine;

public class RefineIDInfoList : BaseVoList
{
	[SerializeField]
	public RefineIDInfo[] list;

	public override void Destroy()
	{
		this.list = new RefineIDInfo[0];
	}
}
