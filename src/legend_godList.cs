using System;
using UnityEngine;

public class legend_godList : BaseVoList
{
	[SerializeField]
	public MonsterInfoVo[] list;

	public override void Destroy()
	{
		this.list = new MonsterInfoVo[0];
	}
}
