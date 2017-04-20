using System;
using UnityEngine;

public class FengLuList : BaseVoList
{
	[SerializeField]
	public FengLuVo[] list;

	public override void Destroy()
	{
		this.list = new FengLuVo[0];
	}
}
