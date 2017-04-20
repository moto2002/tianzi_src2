using System;
using UnityEngine;

public class PetSkillUpList : BaseVoList
{
	[SerializeField]
	public PetSkillUp[] list;

	public override void Destroy()
	{
		this.list = new PetSkillUp[0];
	}
}
