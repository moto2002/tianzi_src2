using System;
using UnityEngine;

public class RefineTimesConsumeList : BaseVoList
{
	[SerializeField]
	public RefineTimesConsume[] list;

	public override void Destroy()
	{
		this.list = new RefineTimesConsume[0];
	}
}
