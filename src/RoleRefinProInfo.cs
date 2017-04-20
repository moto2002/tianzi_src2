using System;

[Serializable]
public class RoleRefinProInfo
{
	public enum Job
	{
		MIN = 1,
		BaHuang = 1,
		YiJian,
		TaiQing,
		GongJianShou,
		MAX = 4
	}

	public enum Sex
	{
		Nan = 1,
		Nv
	}

	public static int lineSums = 4;

	public string mEquProID;

	public string[] mPro = new string[RoleRefinProInfo.lineSums];

	public int Contains(string strItem)
	{
		for (int i = 0; i < this.mPro.Length; i++)
		{
			if (strItem.Equals(this.mPro[i]))
			{
				return i;
			}
		}
		return -1;
	}
}
