using System;
using UnityEngine;

public class GameObjectUtils
{
	public static void SetActive(GameObject go, bool state)
	{
		if (go == null)
		{
			LogSystem.LogWarning(new object[]
			{
				"Gameobject is null,can't do SetActive!"
			});
		}
		go.SetActive(state);
	}
}
