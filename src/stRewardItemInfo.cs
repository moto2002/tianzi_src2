using System;

[Serializable]
public struct stRewardItemInfo
{
	public string strItemId;

	public int nCount;

	public stRewardItemInfo(string itemid, int count)
	{
		this.strItemId = itemid;
		this.nCount = count;
	}
}
