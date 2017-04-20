using System;
using System.Collections.Generic;
using UnityEngine;

public class RideSkillInfoList : BaseVoList
{
	[SerializeField]
	public RideSkillInfo[] list;

	[SerializeField]
	public List<string> PassiveSkills = new List<string>();

	public override void Destroy()
	{
		this.list = new RideSkillInfo[0];
		this.PassiveSkills.Clear();
	}
}
