using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CastModifyConfigData
{
	[SerializeField]
	public string PropPackId = string.Empty;

	[SerializeField]
	private List<string> mKeys;

	[SerializeField]
	private List<string> mValues;

	[SerializeField]
	public List<string> Keys
	{
		get
		{
			return this.mKeys;
		}
	}

	public CastModifyConfigData()
	{
		this.mKeys = new List<string>();
		this.mValues = new List<string>();
	}

	public CastModifyConfigData(List<string> key, List<string> value)
	{
		this.mKeys = key;
		this.mValues = value;
	}

	public string GetPropValue(string strPropKey)
	{
		int num = this.IndexOf(strPropKey);
		if (num > -1)
		{
			return this.mValues[num];
		}
		return string.Empty;
	}

	public void SetPropValue(string strPropKey, string oValue)
	{
		if (!this.Contains(strPropKey) && !string.IsNullOrEmpty(oValue))
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
}
