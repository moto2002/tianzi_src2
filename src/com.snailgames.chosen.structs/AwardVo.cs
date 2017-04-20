using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.snailgames.chosen.structs
{
	[Serializable]
	public class AwardVo
	{
		[SerializeField]
		public string ID;

		[SerializeField]
		public string Name;

		[SerializeField]
		public int CapitalCopper;

		[SerializeField]
		public int CapitalSilver;

		[SerializeField]
		public int Exp;

		[SerializeField]
		public int CapitalSmelt;

		[SerializeField]
		public int CapitalHonor;

		[SerializeField]
		public int CapitalBattleSoul;

		[SerializeField]
		public int CapitalGuildGongxian;

		[SerializeField]
		public string ItemList;

		[SerializeField]
		public string PropertyList;

		public override bool Equals(object obj)
		{
			AwardVo awardVo = obj as AwardVo;
			return awardVo.ID.Equals(this.ID);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public void SetCapitalValue(string value)
		{
			Dictionary<string, int> capitalValue = this.FormatKeyNumber(value, new char[]
			{
				','
			}, new char[]
			{
				':'
			});
			this.SetCapitalValue(capitalValue);
		}

		public void SetCapitalValue(Dictionary<string, int> value)
		{
			if (value != null)
			{
				if (value.ContainsKey("CapitalCopper"))
				{
					this.CapitalCopper = value["CapitalCopper"];
				}
				if (value.ContainsKey("CapitalSilver"))
				{
					this.CapitalSilver = value["CapitalSilver"];
				}
				if (value.ContainsKey("CapitalSmelt"))
				{
					this.CapitalSmelt = value["CapitalSmelt"];
				}
				if (value.ContainsKey("CapitalHonor"))
				{
					this.CapitalHonor = value["CapitalHonor"];
				}
				if (value.ContainsKey("CapitalBattleSoul"))
				{
					this.CapitalBattleSoul = value["CapitalBattleSoul"];
				}
			}
		}

		public Dictionary<string, int> FormatKeyNumber(string value, char[] first, char[] second)
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			string[] array = value.Split(first, StringSplitOptions.RemoveEmptyEntries);
			string text = string.Empty;
			for (int i = 0; i < array.Length; i++)
			{
				text = array[i];
				string[] array2 = text.Split(second, StringSplitOptions.RemoveEmptyEntries);
				if (array2.Length >= 2)
				{
					if (dictionary.ContainsKey(array2[0]))
					{
						string key = array2[0];
						int num = dictionary[key];
						int num2 = this.IntParse(array2[1], 0);
						dictionary[key] = num + num2;
					}
					else
					{
						dictionary.Add(array2[0], this.IntParse(array2[1], 0));
					}
				}
			}
			return dictionary;
		}

		private int IntParse(string value, int defaultValue = 0)
		{
			if (string.IsNullOrEmpty(value))
			{
				return defaultValue;
			}
			value = value.Trim();
			int result;
			if (int.TryParse(value, out result))
			{
				return result;
			}
			return defaultValue;
		}
	}
}
