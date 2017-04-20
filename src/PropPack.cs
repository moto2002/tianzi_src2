using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PropPack
{
	[SerializeField]
	public string PropPackId = string.Empty;

	[SerializeField]
	private List<string> mKeys;

	[SerializeField]
	private List<float> mValues;

	[SerializeField]
	public List<string> Keys
	{
		get
		{
			return this.mKeys;
		}
	}

	public PropPack()
	{
		this.mKeys = new List<string>();
		this.mValues = new List<float>();
	}

	public PropPack(List<string> key, List<float> value)
	{
		this.mKeys = key;
		this.mValues = value;
	}

	public PropPack Clone()
	{
		List<string> list = new List<string>();
		List<float> list2 = new List<float>();
		for (int i = 0; i < this.mKeys.Count; i++)
		{
			list.Add(this.mKeys[i]);
			list2.Add(this.mValues[i]);
		}
		return new PropPack(list, list2);
	}

	public float GetPropValue(string strPropKey)
	{
		int num = this.IndexOf(strPropKey);
		if (num > -1)
		{
			return this.mValues[num];
		}
		return 0f;
	}

	public void SetPropValue(string strPropKey, float oValue)
	{
		if (!this.Contains(strPropKey) && oValue > 0f)
		{
			this.mKeys.Add(strPropKey);
			this.mValues.Add(oValue);
		}
	}

	public bool Contains(string strPropKey)
	{
		return this.IndexOf(strPropKey) > -1;
	}

	public int IndexOf(string strPropKey)
	{
		return this.mKeys.IndexOf(strPropKey);
	}

	public void Remove(string strPropKey)
	{
		int num = this.IndexOf(strPropKey);
		if (num > -1)
		{
			this.mKeys.RemoveAt(num);
			this.mValues.RemoveAt(num);
		}
	}

	public void CombinePack(PropPack propPack)
	{
		if (propPack == null)
		{
			return;
		}
		foreach (string current in propPack.Keys)
		{
			int num = this.IndexOf(current);
			if (num > -1)
			{
				float num2 = this.mValues[num];
				num2 += propPack.GetPropValue(current);
				this.mValues[num] = num2;
			}
			else
			{
				float propValue = propPack.GetPropValue(current);
				if (propValue > 0f)
				{
					this.mKeys.Add(current);
					this.mValues.Add(propValue);
				}
			}
		}
	}

	public void Sort()
	{
	}
}
