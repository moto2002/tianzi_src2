using System;
using UnityEngine;

public class PetSkillInfoList : BaseVoList
{
	[SerializeField]
	public PetSkillInfo[] list;

	public override void Destroy()
	{
		this.list = new PetSkillInfo[0];
	}
}
