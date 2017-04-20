using System;

public class ItemInfo
{
	public enum Type
	{
		None,
		Equip,
		Item,
		Jewel,
		Soul,
		Exp,
		CapitalCopper,
		CapitalSilver,
		CapitalSmelt,
		CapitalGuild,
		CapitalBuildingMaterial,
		CapitalHonor,
		CapitalBattleSoul,
		Fuwen,
		CapitalBloodSoul,
		CapitalRose,
		CapitalDouShen,
		CapitalOre
	}

	public ItemInfo.Type type;

	public string strID = string.Empty;

	public string strPhoto = string.Empty;

	public string strScript = string.Empty;

	public string strResource = string.Empty;

	public int iItemType;

	public int iColorLevel;

	public int iLimitLevel = 1;

	public int iValidType;

	public int iValidTime;

	public int iSellPrice;

	public int iMaxAmount;

	public int mCuiLianPrice;

	public string strParamEx = string.Empty;

	public bool IsEquip()
	{
		return this.strID.StartsWith("Equip");
	}

	public bool IsSoul()
	{
		return this.strID.StartsWith("Soul");
	}
}
