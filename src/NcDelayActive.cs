using System;
using UnityEngine;

public class NcDelayActive : MonoBehaviour
{
	public float m_fDelayTime;

	private NcTimerTool timer = new NcTimerTool();

	private bool usedflag;

	private Renderer[] renderers;

	private NcEffectAniBehaviour[] ncAnimEffects;

	private bool bResetAnimation;

	public float GetParentDelayTime(bool bCheckStarted)
	{
		return 0f;
	}

	private void Update()
	{
		if (this.timer.GetTime() > this.m_fDelayTime && !this.usedflag)
		{
			if (this.renderers != null && this.renderers.Length != 0)
			{
				int i = 0;
				int num = this.renderers.Length;
				while (i < num)
				{
					if (this.renderers[i] != this && this.renderers[i] != null)
					{
						this.renderers[i].enabled = true;
					}
					i++;
				}
			}
			int j = 0;
			int childCount = base.transform.childCount;
			while (j < childCount)
			{
				base.transform.GetChild(j).gameObject.SetActive(true);
				j++;
			}
			this.usedflag = true;
			if (this.bResetAnimation)
			{
				this.SendNcEffectAnimResetAnimation();
			}
		}
	}

	private void OnEnable()
	{
		this.timer.Start();
		if (this.renderers == null)
		{
			this.renderers = base.GetComponents<Renderer>();
		}
		if (this.renderers != null && this.renderers.Length != 0)
		{
			int i = 0;
			int num = this.renderers.Length;
			while (i < num)
			{
				if (this.renderers[i] != null)
				{
					this.renderers[i].enabled = false;
				}
				i++;
			}
		}
		int j = 0;
		int childCount = base.transform.childCount;
		while (j < childCount)
		{
			base.transform.GetChild(j).gameObject.SetActive(false);
			j++;
		}
		this.usedflag = false;
	}

	public void ResetAnimation()
	{
		this.bResetAnimation = true;
		this.ncAnimEffects = base.gameObject.GetComponentsInChildren<NcEffectAniBehaviour>(true);
		this.SetNcEffectAnim();
		this.OnEnable();
	}

	private void SendNcEffectAnimResetAnimation()
	{
		if (this.ncAnimEffects == null)
		{
			return;
		}
		for (int i = 0; i < this.ncAnimEffects.Length; i++)
		{
			if (!(this.ncAnimEffects[i] == null))
			{
				this.ncAnimEffects[i].ResetAnimationForNcDelayActive();
			}
		}
	}

	private void SetNcEffectAnim()
	{
		if (this.ncAnimEffects == null)
		{
			return;
		}
		for (int i = 0; i < this.ncAnimEffects.Length; i++)
		{
			if (!(this.ncAnimEffects[i] == null))
			{
				this.ncAnimEffects[i].m_bNcDelayActive = true;
			}
		}
	}

	private void OnDestroy()
	{
		this.ncAnimEffects = null;
		this.bResetAnimation = false;
	}
}
