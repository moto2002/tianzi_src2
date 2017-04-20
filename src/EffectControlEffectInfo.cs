using System;

[Serializable]
public class EffectControlEffectInfo
{
	public string strName = string.Empty;

	public int PlayType = 1;

	public bool Loop;

	public string strEffectPath = string.Empty;

	public float fDelayFrame;

	public Slot InParent;

	public int TrackType;

	public int DestroyTime;

	public bool bWatch;

	public float OffsetY;

	public float OffsetX;

	public float OffsetZ;

	public int OffsetRX;

	public int iWeight = 1;

	public bool bRotateAround;

	public float fAngleY;

	public string strAddEffect = string.Empty;

	public int iDynamicRot;

	public bool bNeedRandom;

	public int iSpeed;

	public bool bLookAtCamera;

	public int iBuff;

	public bool bFollow;

	public bool bAutoSize;

	public float fScale;

	public string OwnCampEffectPath = string.Empty;
}
