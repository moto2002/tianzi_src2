using System;
using UnityEngine;

public class NationSceneIdDataList : BaseVoList
{
	[SerializeField]
	public NationSceneIdData[] list;

	public override void Destroy()
	{
		this.list = new NationSceneIdData[0];
	}
}
