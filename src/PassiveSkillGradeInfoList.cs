using System;
using UnityEngine;

public class PassiveSkillGradeInfoList : BaseVoList
{
	[SerializeField]
	public PassiveSkillGradeInfo[] list;

	public override void Destroy()
	{
		this.list = new PassiveSkillGradeInfo[0];
	}
}
