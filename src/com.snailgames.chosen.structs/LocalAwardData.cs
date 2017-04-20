using System;
using UnityEngine;

namespace com.snailgames.chosen.structs
{
	[Serializable]
	public class LocalAwardData
	{
		[SerializeField]
		public int id;

		[SerializeField]
		public string value = string.Empty;

		[SerializeField]
		public string desc = string.Empty;

		[SerializeField]
		public string strCopper = string.Empty;

		[SerializeField]
		public string strSilver = string.Empty;

		[SerializeField]
		public string itemList = string.Empty;

		[SerializeField]
		public string effectList = string.Empty;
	}
}
