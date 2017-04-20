using System;
using UnityEngine;

public class RideSkillQualityList : BaseVoList
{
	[SerializeField]
	public RideSkillQuality[] list;

	public override void Destroy()
	{
		this.list = new RideSkillQuality[0];
	}
}
