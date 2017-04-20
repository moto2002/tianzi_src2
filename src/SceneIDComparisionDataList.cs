using System;
using UnityEngine;

public class SceneIDComparisionDataList : BaseVoList
{
	[SerializeField]
	public SceneIDComparisionData[] list;

	public override void Destroy()
	{
		this.list = new SceneIDComparisionData[0];
	}
}
