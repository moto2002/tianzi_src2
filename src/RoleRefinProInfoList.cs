using System;
using UnityEngine;

public class RoleRefinProInfoList : BaseVoList
{
	[SerializeField]
	public RoleRefinProInfo[] list;

	public override void Destroy()
	{
		this.list = new RoleRefinProInfo[0];
	}
}
