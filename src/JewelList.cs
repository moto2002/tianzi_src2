using System;
using UnityEngine;

public class JewelList : BaseVoList
{
	[SerializeField]
	public JewelItemInfo[] list;

	public override void Destroy()
	{
		this.list = new JewelItemInfo[0];
	}
}
