using System;
using System.Collections.Generic;
using UnityEngine;

public class CtnpayDataList : BaseVoList
{
	[SerializeField]
	public CtnpayDataVo[] list;

	public List<float> GetHightOffset(int day)
	{
		if (this.list == null || this.list.Length == 0)
		{
			return null;
		}
		for (int i = 0; i < this.list.Length; i++)
		{
			CtnpayDataVo ctnpayDataVo = this.list[i];
			if (ctnpayDataVo.CtnNum == day)
			{
				return ctnpayDataVo.GetHightOffset();
			}
		}
		return null;
	}

	public override void Destroy()
	{
		this.list = new CtnpayDataVo[0];
	}
}
