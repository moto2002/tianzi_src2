using System;
using UnityEngine;

public class BasePackVoList : BaseVoList
{
	[SerializeField]
	public BasePackInfo[] list;

	public override void Destroy()
	{
		this.list = new BasePackInfo[0];
	}
}
