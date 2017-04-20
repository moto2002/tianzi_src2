using System;
using System.Collections.Generic;
using UnityEngine;

public class CastModifyConfigDataList : BaseVoList
{
	[SerializeField]
	public CastModifyConfigData[] list;

	private Dictionary<string, int> PropsDic;

	public void InitDictionary()
	{
		if (this.PropsDic == null)
		{
			this.PropsDic = new Dictionary<string, int>();
			for (int i = 0; i < this.list.Length; i++)
			{
				CastModifyConfigData castModifyConfigData = this.list[i];
				this.Add(castModifyConfigData.PropPackId, i);
			}
		}
	}

	public CastModifyConfigData GetPropPackByKey(string key)
	{
		if (this.PropsDic.ContainsKey(key))
		{
			return this.list[this.PropsDic[key]];
		}
		return null;
	}

	private void Add(string key, int vaule)
	{
		if (!this.PropsDic.ContainsKey(key))
		{
			this.PropsDic.Add(key, vaule);
		}
	}

	public override void Destroy()
	{
		this.list = new CastModifyConfigData[0];
		if (this.PropsDic != null)
		{
			this.PropsDic.Clear();
		}
	}
}
