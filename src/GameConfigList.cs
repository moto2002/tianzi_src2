using System;
using UnityEngine;

public class GameConfigList : BaseVoList
{
	[SerializeField]
	public GameConfigVo[] list;

	public override void Destroy()
	{
		this.list = new GameConfigVo[0];
	}
}
