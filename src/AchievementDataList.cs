using System;
using System.Collections.Generic;
using UnityEngine;

public class AchievementDataList : BaseVoList
{
	[SerializeField]
	public AchievementData[] list;

	private Dictionary<string, AchievementData> m_dictionary;

	public Dictionary<string, AchievementData> GetDictionary()
	{
		if (this.m_dictionary == null)
		{
			this.m_dictionary = new Dictionary<string, AchievementData>();
			AchievementData[] array = this.list;
			if (array != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					AchievementData achievementData = array[i];
					if (this.m_dictionary.ContainsKey(achievementData.id))
					{
						this.m_dictionary.Add(achievementData.id, achievementData);
					}
				}
			}
		}
		return this.m_dictionary;
	}

	public override void Destroy()
	{
		this.list = new AchievementData[0];
		if (this.m_dictionary != null)
		{
			this.m_dictionary.Clear();
		}
		this.m_dictionary = null;
	}
}
