using System;
using System.Collections.Generic;

[Serializable]
public class MonsterInfoVo
{
	public int Index;

	public string Name;

	public string ModelId;

	public string NpcId;

	public string ChangeBuffId;

	public string ItemId;

	public int ActivateCount;

	public int Type;

	public int Quality;

	public bool IsActivate;

	public bool IsChange;

	public bool IsSelect;

	public float Scale;

	public string mFightItemId;

	public int mFightItemNums;

	public string mQingShenFuItemId;

	public int mQingShenFuNums;

	public byte[] mYuanBaoNums;

	public int mRelateSpeed;

	public int mRelateMax;

	public string mChangeModelDesc;

	public string mNormalSkillDes;

	public string mCombSkillDes;

	public string[] mSkillIdArr;

	public string mBreakNeedItem;

	public List<int> mBreakNodes = new List<int>();

	public List<int> mBreakNeedItemNums = new List<int>();

	public int mBaseFightValue;

	public string mPackId;

	public bool mblHasCombineSkill;

	public float posX;

	public float posY;

	public float posZ;

	public float rotateY;

	public float mAttributionRatio;

	public string mRelateUpItemId;

	public int mRelateNeedNums;

	public int mMaxCoolTime;

	public string mEffectName = string.Empty;
}
