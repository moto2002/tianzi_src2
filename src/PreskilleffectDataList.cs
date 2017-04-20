using System;
using UnityEngine;

public class PreskilleffectDataList : BaseVoList
{
	[SerializeField]
	public PreskilleffectDataVo[] list;

	public override void Destroy()
	{
		this.list = new PreskilleffectDataVo[0];
	}
}
