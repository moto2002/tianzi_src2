using System;
using UnityEngine;

public class EffectBatchList : BaseVoList
{
	[SerializeField]
	public EffectBatch[] list;

	public override void Destroy()
	{
		this.list = new EffectBatch[0];
	}
}
