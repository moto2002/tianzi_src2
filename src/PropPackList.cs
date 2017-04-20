using System;
using System.Collections.Generic;
using UnityEngine;

public class PropPackList : BaseVoList
{
	[SerializeField]
	public PropPack[] list;

	private Dictionary<string, int> PropsDic;

	public void InitDictionary()
	{
		if (this.PropsDic == null)
		{
			this.PropsDic = new Dictionary<string, int>();
			for (int i = 0; i < this.list.Length; i++)
			{
				PropPack propPack = this.list[i];
				this.Add(propPack.PropPackId, i);
			}
		}
	}

	public PropPack GetPropPackByKey(string key)
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
		this.list = new PropPack[0];
		if (this.PropsDic != null)
		{
			this.PropsDic.Clear();
		}
	}
}
