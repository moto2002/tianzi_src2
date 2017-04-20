using System;

[Serializable]
public class ObjectInfo
{
	public string strID;

	public string strScript;

	public string strResouce;

	public int iNpcType;

	public int iNation;

	public int iLevel;

	public int iSelectState;

	public int iCollideRadius;

	public float fRadius;

	public string strBornEffect;

	public string[] strBossbgSound;

	public string strSound;

	public int iSpringRange;

	public string strDieEffect;

	public string photo;

	public float fNPCLiveTime;

	public string strWeapon;

	public string strActivatedRide;

	public int iRideSurfaceIndex = -1;

	public AppearState aAppear;

	public bool bImmediatelyCreate;

	public bool AttackDontRotation;

	public string mapSpriteName = string.Empty;
}
