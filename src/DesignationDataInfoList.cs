using System;
using UnityEngine;

public class DesignationDataInfoList : BaseVoList
{
	[SerializeField]
	public DesignationDataInfo[] list;

	public override void Destroy()
	{
		this.list = new DesignationDataInfo[0];
	}
}
