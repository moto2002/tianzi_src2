using System;

[Serializable]
public class SkillInfo
{
	public string ID = string.Empty;

	[NonSerialized]
	public string Name = string.Empty;

	public string Type = string.Empty;

	public string HitSkill = string.Empty;

	public float HitTimeMin;

	public float HitTimeMax;

	public bool Single;

	public bool DisplaceMent;

	public string Effect = string.Empty;

	public string AddEffect = string.Empty;

	public bool IsLevel;

	public float AtkDistance;

	public float SprintDis;

	public float SprintSpacing;

	public float SkillCastMode;

	public bool AtkMove;

	public string[] DamageEff;

	public EnumUseType UseType;

	public int IsSendTargetPos;

	public EnumTargetType TargetType;

	public EnumDisplaceType DisplaceType;

	public float RepelDist;

	public bool NeedTarget;

	public int iHitTime;

	public string[] sHitRangeListID;

	public int iHitCount;

	public int iLeadSepTime;

	public int iLeadTime;

	public bool bIsLead;

	public int[] iExtraHitTimes;

	public bool bIsClientHit;

	public bool bCaton;

	public bool bCanStopCase;

	public int iBtnEffect;

	public int iBuffTime;

	public int iPassiveSkill;

	public string strPSkillReset;

	public string strEndSkill;

	public int iChargeTime;

	public int iCoolDownCategory;

	public bool IsUnSilent;

	public bool IsUnVertigo;

	public string GetDamageEff(int index)
	{
		if (this.DamageEff == null || this.DamageEff.Length == 0)
		{
			return string.Empty;
		}
		if (this.DamageEff.Length > index && index >= 0)
		{
			return this.DamageEff[index];
		}
		return this.DamageEff[0];
	}
}
