using System;
using UnityEngine;

public class SceneConfigInfoList : BaseVoList
{
	[SerializeField]
	public SceneConfigInfo[] list;

	public override void Destroy()
	{
		this.list = new SceneConfigInfo[0];
	}
}
