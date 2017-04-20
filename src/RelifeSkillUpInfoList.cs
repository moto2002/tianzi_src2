using System;
using UnityEngine;

public class RelifeSkillUpInfoList : BaseVoList
{
	[SerializeField]
	public RelifeSkillUpInfo[] list;

	public override void Destroy()
	{
		this.list = new RelifeSkillUpInfo[0];
	}
}
