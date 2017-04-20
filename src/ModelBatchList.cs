using System;
using UnityEngine;

public class ModelBatchList : BaseVoList
{
	[SerializeField]
	public ModelBatch[] list;

	public override void Destroy()
	{
		this.list = new ModelBatch[0];
	}
}
