using System;
using UnityEngine;

public class SkillBookList : BaseVoList
{
	[SerializeField]
	public SkillBookVo[] list;

	public override void Destroy()
	{
		this.list = new SkillBookVo[0];
	}
}
