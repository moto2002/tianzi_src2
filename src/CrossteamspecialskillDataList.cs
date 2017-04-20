using System;
using System.Collections.Generic;
using UnityEngine;

public class CrossteamspecialskillDataList : BaseVoList
{
	[SerializeField]
	public CrossteamspecialskillDataVo[] list;

	public List<CrossteamspecialskillDataVo> AllSkills = new List<CrossteamspecialskillDataVo>();

	private Dictionary<string, CrossteamspecialskillDataVo.SpecialskillInfo> SkillDic = new Dictionary<string, CrossteamspecialskillDataVo.SpecialskillInfo>();

	public void Init()
	{
		if (this.list != null)
		{
			this.AllSkills.Clear();
			this.SkillDic.Clear();
			for (int i = 0; i < this.list.Length; i++)
			{
				CrossteamspecialskillDataVo crossteamspecialskillDataVo = this.list[i];
				if (crossteamspecialskillDataVo != null)
				{
					this.AllSkills.Add(crossteamspecialskillDataVo);
					for (int j = 0; j < crossteamspecialskillDataVo.Skills.Count; j++)
					{
						CrossteamspecialskillDataVo.SpecialskillInfo specialskillInfo = crossteamspecialskillDataVo.Skills[j];
						if (specialskillInfo != null && !this.SkillDic.ContainsKey(specialskillInfo.Skillid))
						{
							this.SkillDic.Add(specialskillInfo.Skillid, specialskillInfo);
						}
					}
				}
			}
		}
	}

	public string GetSkillTypeIconBySkillid(int groupindex)
	{
		if (this.AllSkills != null && this.AllSkills.Count > 0)
		{
			for (int i = 0; i < this.AllSkills.Count; i++)
			{
				CrossteamspecialskillDataVo crossteamspecialskillDataVo = this.AllSkills[i];
				if (crossteamspecialskillDataVo != null && crossteamspecialskillDataVo.Group == groupindex)
				{
					return crossteamspecialskillDataVo.SkillTypeIcon;
				}
			}
		}
		return string.Empty;
	}

	public List<CrossteamspecialskillDataVo> GetAllSkill()
	{
		if (this.AllSkills != null)
		{
			for (int i = 0; i < this.AllSkills.Count; i++)
			{
				CrossteamspecialskillDataVo crossteamspecialskillDataVo = this.AllSkills[i];
				if (crossteamspecialskillDataVo != null)
				{
					crossteamspecialskillDataVo.ResetCount();
				}
			}
			return this.AllSkills;
		}
		return null;
	}

	public string GetIconPathBySkillId(string skillid)
	{
		if (this.SkillDic != null && this.SkillDic.Count > 0 && this.SkillDic.ContainsKey(skillid))
		{
			return this.SkillDic[skillid].Path;
		}
		return string.Empty;
	}

	public override void Destroy()
	{
		this.list = new CrossteamspecialskillDataVo[0];
		if (this.AllSkills != null)
		{
			this.AllSkills.Clear();
		}
		if (this.SkillDic != null)
		{
			this.SkillDic.Clear();
		}
	}
}
