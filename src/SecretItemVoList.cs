using System;
using UnityEngine;

public class SecretItemVoList : BaseVoList
{
	[SerializeField]
	public SecretItemVo[] list;

	public override void Destroy()
	{
		this.list = new SecretItemVo[0];
	}
}
