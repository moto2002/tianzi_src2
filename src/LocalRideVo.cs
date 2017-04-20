using System;
using System.Collections.Generic;

[Serializable]
public class LocalRideVo
{
	public string id;

	public string name;

	public int ColorLevel;

	public string script;

	public string photo;

	public List<string> model = new List<string>();

	public string rideDesc;

	public string speedAdd;

	public string strType;

	public float fRideHeight;

	public float fFabaoOffsetY;

	public float posX;

	public float posY;

	public float posZ;

	public float rotate;

	public float scale;

	public float modelScale;

	public string baseLevelAttrId;

	public string baseStepAttrId;

	public int defaultLevel;

	public int defaultStep;

	public int activateCondition;

	public string getLinkDes;

	public int uiMusicIndex;

	public string useItem;
}
