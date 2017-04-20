using System;
using UnityEngine;

namespace com.snailgames.chosen.structs
{
	public class LocalAwardDataList : BaseVoList
	{
		[SerializeField]
		public LocalAwardData[] list;

		public override void Destroy()
		{
			this.list = new LocalAwardData[0];
		}
	}
}
