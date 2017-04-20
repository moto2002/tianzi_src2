using System;
using UnityEngine;

public class GuildTaskList : BaseVoList
{
	[SerializeField]
	public GuildTaskVo[] list;

	public override void Destroy()
	{
		this.list = new GuildTaskVo[0];
	}
}
