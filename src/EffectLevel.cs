using System;
using UnityEngine;

[ExecuteInEditMode]
public class EffectLevel : MonoBehaviour
{
	public static GameQuality iEffectLevel = GameQuality.LOW;

	public static GameQuality iImageLevel = GameQuality.LOW;

	public bool mbHigh;

	public bool mbMiddle;

	public bool mbLow;

	public bool mbMinimalism;

	protected bool mbCurActive;

	protected static bool effectLevelLocked;

	public static void SetEffectLevel(GameQuality level)
	{
		if (!EffectLevel.effectLevelLocked && EffectLevel.iEffectLevel != level)
		{
			EffectLevel.iEffectLevel = level;
		}
	}

	public static void SetImageLevel(GameQuality level)
	{
		if (EffectLevel.iImageLevel != level)
		{
			EffectLevel.iImageLevel = level;
		}
	}

	private void Start()
	{
		this.StartELevel();
		this.LevelChange(EffectLevel.iEffectLevel);
	}

	private void Destroy()
	{
		this.DestoryELevel();
	}

	protected virtual void StartELevel()
	{
	}

	protected virtual void LevelChange(GameQuality level)
	{
		GameQuality gameQuality = level;
		if (base.gameObject.layer == LayerMask.NameToLayer("UIEffect") || base.gameObject.layer == LayerMask.NameToLayer("UI"))
		{
			gameQuality = EffectLevel.iImageLevel;
		}
		bool flag = false;
		switch (gameQuality)
		{
		case GameQuality.MIN:
			if (this.mbMinimalism)
			{
				flag = true;
			}
			break;
		case GameQuality.LOW:
			if (this.mbLow)
			{
				flag = true;
			}
			break;
		case GameQuality.MIDDLE:
			if (this.mbMiddle)
			{
				flag = true;
			}
			break;
		case GameQuality.HIGH:
			if (this.mbHigh)
			{
				flag = true;
			}
			break;
		}
		this.mbCurActive = flag;
		this.SetActive(this.mbCurActive);
	}

	protected virtual void SetActive(bool bShow)
	{
	}

	protected virtual void DestoryELevel()
	{
	}

	public virtual void LockEffectLevel()
	{
	}
}
