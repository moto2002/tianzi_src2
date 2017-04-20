using System;
using UnityEngine;

public class RuneLevelConfigList : BaseVoList
{
	[SerializeField]
	public RuneLevelConfig[] list;

	public override void Destroy()
	{
		this.list = new RuneLevelConfig[0];
	}
}
