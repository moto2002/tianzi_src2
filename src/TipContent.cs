using System;

public class TipContent
{
	public string ItemId;

	public int value;

	public TipContent(string id, int val)
	{
		this.ItemId = id;
		this.value = val;
	}

	public TipContent()
	{
	}

	public TipContent(AwardItem item)
	{
		if (item.type == AwardItem.AwardType.Equip)
		{
			this.value = item.quality;
		}
		else
		{
			this.value = item.num;
		}
		this.ItemId = item.itemId;
	}
}
