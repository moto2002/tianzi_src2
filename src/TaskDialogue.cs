using System;
using System.Collections.Generic;

[Serializable]
public class TaskDialogue
{
	public string TalkId;

	public int iTaskSound;

	public List<TaskSentence> Sentences;
}
