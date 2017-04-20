using System;
using UnityEngine;

public class TaskUseItemList : BaseVoList
{
	[SerializeField]
	public TaskUseItem[] list;

	public override void Destroy()
	{
		this.list = new TaskUseItem[0];
	}
}
