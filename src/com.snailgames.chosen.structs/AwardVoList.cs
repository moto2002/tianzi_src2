using System;
using UnityEngine;

namespace com.snailgames.chosen.structs
{
	public class AwardVoList : BaseVoList
	{
		[SerializeField]
		public AwardVo[] list;

		public override void Destroy()
		{
			this.list = new AwardVo[0];
		}
	}
}
