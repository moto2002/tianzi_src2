using System;
using System.Collections.Generic;

[Serializable]
public class CtnpayDataVo
{
	public int CtnNum;

	public string PosList;

	public List<float> GetHightOffset()
	{
		if (string.IsNullOrEmpty(this.PosList))
		{
			return null;
		}
		List<float> list = new List<float>();
		string[] array = this.PosList.Split(new char[]
		{
			';'
		});
		for (int i = 0; i < array.Length; i++)
		{
			float item = 0f;
			float.TryParse(array[i], out item);
			list.Add(item);
		}
		return list;
	}
}
