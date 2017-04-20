using System;
using UnityEngine;

public class BufferDataList : BaseVoList
{
	[SerializeField]
	public BufferData[] list;

	public override void Destroy()
	{
		this.list = new BufferData[0];
	}
}
