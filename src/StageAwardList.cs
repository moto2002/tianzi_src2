using System;
using UnityEngine;

public class StageAwardList : BaseVoList
{
	[SerializeField]
	public StageAward[] list;

	public override void Destroy()
	{
		this.list = new StageAward[0];
	}
}
