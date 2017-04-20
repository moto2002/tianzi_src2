using System;
using UnityEngine;

public class TaskDialogueList : BaseVoList
{
	[SerializeField]
	public TaskDialogue[] list;

	public override void Destroy()
	{
		this.list = new TaskDialogue[0];
	}
}
