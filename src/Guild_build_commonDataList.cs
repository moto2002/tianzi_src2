using System;
using UnityEngine;

public class Guild_build_commonDataList : BaseVoList
{
	[SerializeField]
	public Guild_build_commonDataVo[] list;

	public override void Destroy()
	{
		this.list = new Guild_build_commonDataVo[0];
	}
}
