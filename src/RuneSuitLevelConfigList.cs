using System;
using UnityEngine;

public class RuneSuitLevelConfigList : BaseVoList
{
	[SerializeField]
	public RuneSuitLevelConfig[] list;

	public override void Destroy()
	{
		this.list = new RuneSuitLevelConfig[0];
	}
}
