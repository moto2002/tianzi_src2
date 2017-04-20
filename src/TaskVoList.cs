using System;
using UnityEngine;

public class TaskVoList : BaseVoList
{
	[SerializeField]
	public TaskVo[] list;

	public override void Destroy()
	{
		this.list = new TaskVo[0];
	}
}
