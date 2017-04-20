using System;

[Serializable]
public class PassiveSkillGradeInfo
{
	public int id;

	public int roleLimitLv;

	public int preSkillId;

	public int preSkillLv;

	public int costSkillPoint;

	public int capitalType;

	public int capitalAmount;

	public string costItemId;

	public int costItemCount;

	public string propList;

	public void SetSkillNeedTerm(string str)
	{
		if (string.IsNullOrEmpty(str))
		{
			return;
		}
		string[] array = str.Split(new char[]
		{
			','
		});
		if (array == null || array.Length != 2)
		{
			return;
		}
		this.preSkillId = AssetFileUtils.IntParse(array[0], 1);
		this.preSkillLv = AssetFileUtils.IntParse(array[1], 1);
	}

	public void SetSkillCostItem(string str)
	{
		if (string.IsNullOrEmpty(str))
		{
			return;
		}
		string[] array = str.Split(new char[]
		{
			','
		});
		if (array == null || array.Length != 2)
		{
			return;
		}
		this.costItemId = array[0];
		this.costItemCount = AssetFileUtils.IntParse(array[1], 1);
	}
}
