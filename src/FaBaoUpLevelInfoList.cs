using System;
using System.Collections.Generic;
using UnityEngine;

public class FaBaoUpLevelInfoList : BaseVoList
{
	[SerializeField]
	public FaBaoUpLevelInfo[] list;

	private Dictionary<int, FaBaoUpLevelInfo> mDataDic;

	public int MaxCloud;

	public int MaxOrder;

	public int MaxLevel;

	public void InitDictionary()
	{
		if (this.mDataDic == null)
		{
			this.mDataDic = new Dictionary<int, FaBaoUpLevelInfo>();
			for (int i = 0; i < this.list.Length; i++)
			{
				FaBaoUpLevelInfo faBaoUpLevelInfo = this.list[i];
				if (!this.mDataDic.ContainsKey(faBaoUpLevelInfo.key))
				{
					this.mDataDic.Add(faBaoUpLevelInfo.key, faBaoUpLevelInfo);
				}
			}
		}
	}

	public FaBaoUpLevelInfo GetFaBaoUpLevelInfo(int key)
	{
		if (this.mDataDic.ContainsKey(key))
		{
			return this.mDataDic[key];
		}
		return null;
	}

	public override void Destroy()
	{
		this.list = new FaBaoUpLevelInfo[0];
		if (this.mDataDic != null)
		{
			this.mDataDic.Clear();
		}
	}
}
