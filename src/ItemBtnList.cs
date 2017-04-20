using System;
using UnityEngine;

public class ItemBtnList : BaseVoList
{
	[SerializeField]
	public ItemBtnVo[] list;

	public override void Destroy()
	{
		this.list = new ItemBtnVo[0];
	}
}
