using System;
using UnityEngine;

public class Resourcewar_buff_configDataList : BaseVoList
{
	[SerializeField]
	public Resourcewar_buff_configDataVo[] list;

	public override void Destroy()
	{
		this.list = new Resourcewar_buff_configDataVo[0];
	}
}
