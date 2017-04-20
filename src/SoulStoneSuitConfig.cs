using System;

[Serializable]
public class SoulStoneSuitConfig
{
	public int nStoneLevel;

	public string strSkillId = string.Empty;

	public int nSkillLevel;

	public string strSkillDes = string.Empty;

	public string strSkillName = string.Empty;

	public SoulStoneSuitConfig(int _nStoneLevel, string _strSkillId, int _nSkillLevel, string _strSkillDes, string _strSkillName = "")
	{
		this.nStoneLevel = _nStoneLevel;
		this.strSkillId = _strSkillId;
		this.nSkillLevel = _nSkillLevel;
		this.strSkillDes = _strSkillDes;
		this.strSkillName = _strSkillName;
	}
}
