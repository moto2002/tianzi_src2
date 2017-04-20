using System;
using UnityEngine;

[Serializable]
public class TaskVo
{
	[SerializeField]
	public int ID;

	[SerializeField]
	public string Title;

	[SerializeField]
	public int AcceptLevel;

	[SerializeField]
	public int Type;

	[SerializeField]
	public int CriteriaType;

	[SerializeField]
	public int FormatType;

	[SerializeField]
	public string NpcList;

	[SerializeField]
	public int RepeatNum;

	[SerializeField]
	public string AwardID;

	[SerializeField]
	public string preQuestList;

	[SerializeField]
	public string postQuestList;

	[SerializeField]
	public int Count;

	[SerializeField]
	public string Desc;

	[SerializeField]
	public string SimpleDesc;

	[SerializeField]
	public int IsUseStar;

	[SerializeField]
	public string TalkId;

	[SerializeField]
	public string QuestItem;

	[SerializeField]
	public int GroupId;

	[SerializeField]
	public string CGMovie;

	[SerializeField]
	public bool bStartEffect;

	[SerializeField]
	public bool bEndEffect;

	[SerializeField]
	public int nation;

	[SerializeField]
	public string scene;

	[SerializeField]
	public bool isShow;

	[SerializeField]
	public string TransformResource;

	[SerializeField]
	public float CDTime;

	public override bool Equals(object obj)
	{
		TaskVo taskVo = obj as TaskVo;
		return taskVo.ID.Equals(this.ID);
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
