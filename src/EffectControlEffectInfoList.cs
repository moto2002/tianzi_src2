using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectControlEffectInfoList : BaseVoList
{
	[SerializeField]
	public EffectControlEffectInfo[] list;

	private Dictionary<string, EffectControlEffectInfo> mDictionary;

	public Dictionary<string, EffectControlEffectInfo> GetDictionay
	{
		get
		{
			if (this.mDictionary != null)
			{
				return this.mDictionary;
			}
			this.mDictionary = new Dictionary<string, EffectControlEffectInfo>();
			if (this.list != null)
			{
				for (int i = 0; i < this.list.Length; i++)
				{
					EffectControlEffectInfo effectControlEffectInfo = this.list[i];
					if (!this.mDictionary.ContainsKey(effectControlEffectInfo.strName))
					{
						this.mDictionary.Add(effectControlEffectInfo.strName, effectControlEffectInfo);
					}
				}
			}
			return this.mDictionary;
		}
	}

	public override void Destroy()
	{
		this.list = new EffectControlEffectInfo[0];
		if (this.mDictionary != null)
		{
			this.mDictionary.Clear();
		}
	}
}
