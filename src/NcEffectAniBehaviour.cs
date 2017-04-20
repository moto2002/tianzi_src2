using System;
using UnityEngine;

public class NcEffectAniBehaviour : NcEffectBehaviour
{
	protected NcTimerTool m_Timer;

	protected GameObject m_OnEndAniGameObject;

	protected bool m_bEndAnimation;

	public string m_OnEndAniFunction = "OnEndAnimation";

	public bool m_bNcDelayActive;

	public void SetCallBackEndAnimation(GameObject callBackGameObj)
	{
		this.m_OnEndAniGameObject = callBackGameObj;
	}

	public void SetCallBackEndAnimation(GameObject callBackGameObj, string nameFunction)
	{
		this.m_OnEndAniGameObject = callBackGameObj;
		this.m_OnEndAniFunction = nameFunction;
	}

	public bool IsEndAnimation()
	{
		return this.m_bEndAnimation;
	}

	protected void InitAnimationTimer()
	{
		if (this.m_Timer == null)
		{
			this.m_Timer = new NcTimerTool();
		}
		this.m_bEndAnimation = false;
		this.m_Timer.Start();
	}

	public virtual void ResetAnimation()
	{
		if (this.m_bNcDelayActive)
		{
			return;
		}
		this.m_bEndAnimation = false;
		if (this.m_Timer != null)
		{
			this.m_Timer.Reset(0f);
		}
	}

	public virtual void ResetAnimationForNcDelayActive()
	{
		this.ResetAnimation();
	}

	public virtual void PauseAnimation()
	{
		if (this.m_Timer != null)
		{
			this.m_Timer.Pause();
		}
	}

	public virtual void ResumeAnimation()
	{
		if (this.m_Timer != null)
		{
			this.m_Timer.Resume();
		}
	}

	public virtual void MoveAnimation(float fRate)
	{
		if (this.m_Timer != null)
		{
			this.m_Timer.Reset(fRate);
		}
	}

	protected void OnEndAnimation()
	{
		this.m_bEndAnimation = true;
		if (this.m_OnEndAniGameObject != null)
		{
			this.m_OnEndAniGameObject.SendMessage(this.m_OnEndAniFunction, this, SendMessageOptions.DontRequireReceiver);
		}
	}
}
