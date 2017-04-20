using System;
using UnityEngine;

public class FunctionUnLockDataList : BaseVoList
{
	[SerializeField]
	public FunctionUnLockData[] list;

	public override void Destroy()
	{
		this.list = new FunctionUnLockData[0];
	}
}
