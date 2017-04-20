using System;

[Serializable]
public class SoulItemInfo : ItemInfo
{
	public string strEquipType = string.Empty;

	public int strSoulType = -1;

	public int iAmount;

	public string strBasePackage = string.Empty;

	public int iLevel = 1;

	public string strUpItem = string.Empty;

	public int iSoulClass = 1;

	public int iSoulPoint = 1;

	public int iUpAmount;

	public int iUpLevel;

	public string strEffectPath = string.Empty;

	public int iUpLevelCost;
}
