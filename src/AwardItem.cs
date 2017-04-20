using System;

public class AwardItem
{
	public enum AwardType
	{
		CapitalCopper,
		CapitalSilver,
		Exp,
		CapitalSmelt,
		CapitalHonor,
		CapitalBattleSoul,
		CapitalGuild,
		Item,
		Equip,
		PropertyValue
	}

	public AwardItem.AwardType type;

	public string name;

	public int num;

	public string icon;

	public int quality;

	public string itemId;

	public bool isValue
	{
		get
		{
			return this.type == AwardItem.AwardType.Exp || this.type == AwardItem.AwardType.CapitalCopper || this.type == AwardItem.AwardType.CapitalSilver || this.type == AwardItem.AwardType.CapitalSmelt || this.type == AwardItem.AwardType.CapitalHonor || this.type == AwardItem.AwardType.CapitalBattleSoul || this.type == AwardItem.AwardType.CapitalGuild;
		}
	}

	public AwardItem(AwardItem.AwardType type, string name, int num, string icon, int quality, string itemId = "")
	{
		this.type = type;
		this.name = name;
		this.num = num;
		this.icon = icon;
		this.quality = quality;
		this.itemId = itemId;
	}
}
