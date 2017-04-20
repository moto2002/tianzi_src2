using System;
using UnityEngine;

public class Legend_god_extractDataList : BaseVoList
{
	[SerializeField]
	public Legend_god_extractDataVo[] list;

	public override void Destroy()
	{
		this.list = new Legend_god_extractDataVo[0];
	}
}
