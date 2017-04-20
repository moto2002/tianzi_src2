using System;
using UnityEngine;

public class SkillInfoList : BaseVoList
{
	[SerializeField]
	public SkillInfo[] list;

	public override void Destroy()
	{
		this.list = new SkillInfo[0];
	}
}
