using System;
using UnityEngine;

public class LinkList : BaseVoList
{
	[SerializeField]
	public LinkVo[] list;

	public override void Destroy()
	{
		this.list = new LinkVo[0];
	}
}
