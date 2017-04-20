using System;

[Serializable]
public class SoulSuitInfo : IComparable<SoulSuitInfo>
{
	public string Id;

	public int iProfession;

	public int iGender;

	public int iSoulLevel;

	public int iSoulStage;

	public int iSoulClass;

	public string strWeapon;

	public string strCloth;

	public string strBasePackage;

	public int CompareTo(SoulSuitInfo target)
	{
		if (this.iSoulLevel < target.iSoulLevel)
		{
			return -1;
		}
		if (this.iSoulLevel > target.iSoulLevel)
		{
			return 1;
		}
		return 0;
	}
}
