using System;
using UnityEngine;

public class PartnerLevelDataVoList : BaseVoList
{
	[SerializeField]
	public CharacterPartnerLevelData[] list;

	public override void Destroy()
	{
		this.list = new CharacterPartnerLevelData[0];
	}
}
