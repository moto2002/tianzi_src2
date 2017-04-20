using System;
using UnityEngine;

public class XinFaUpGradeInfoList : BaseVoList
{
	[SerializeField]
	public XinFaUpGradeInfo[] list;

	public override void Destroy()
	{
		this.list = new XinFaUpGradeInfo[0];
	}
}
