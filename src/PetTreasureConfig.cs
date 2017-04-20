using System;
using System.Collections.Generic;

[Serializable]
public class PetTreasureConfig
{
	public int treasureID;

	public string treasureName;

	public int location;

	public int maxVacancy;

	public int qualityLimit;

	public int starLevelLimit;

	public string friendlyPetID;

	public float frienglyRate;

	public List<int> abilityAddValue = new List<int>();

	public List<int> abilityAddRate = new List<int>();

	public string baseOutput;

	public int baseOutputCount;

	public int baseOutputInterval;

	public int finalOutputTime;

	public string deputyGetRewardID;

	public int deputyGetRewardCount;

	public int treasureQuality;

	public int treasureLifeTime;

	public List<stRewardItemInfo> mListFinalRewards = new List<stRewardItemInfo>();
}
