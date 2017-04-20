using System;
using UnityEngine;

public class NcAttachSound : NcEffectBehaviour
{
	public float m_fDelayTime;

	public float m_fRepeatTime;

	public int m_nRepeatCount;

	public AudioClip m_AudioClip;

	public int m_nPriority = 128;

	public bool m_bLoop;

	public float m_fVolume = 1f;

	public float m_fPitch = 1f;

	protected AudioSource m_AudioSource;

	protected float m_fStartTime;

	protected int m_nCreateCount;

	protected bool m_bStartAttach;

	public override int GetAnimationState()
	{
		if ((base.enabled && NcEffectBehaviour.IsActive(base.gameObject)) || (this.m_AudioSource != null && this.m_AudioSource.isPlaying))
		{
			return 1;
		}
		return 0;
	}

	public void Replay()
	{
		this.m_bStartAttach = false;
	}

	private void Update()
	{
		if (this.m_AudioClip == null)
		{
			base.enabled = false;
			return;
		}
		if (!this.m_bStartAttach)
		{
			this.m_fStartTime = NcEffectBehaviour.GetEngineTime();
			this.m_bStartAttach = true;
		}
		if (this.m_fStartTime + this.m_fDelayTime <= NcEffectBehaviour.GetEngineTime())
		{
			this.CreateAttachSound();
			if (0f < this.m_fRepeatTime && (this.m_nRepeatCount == 0 || this.m_nCreateCount < this.m_nRepeatCount))
			{
				this.m_fStartTime = NcEffectBehaviour.GetEngineTime();
				this.m_fDelayTime = this.m_fRepeatTime;
			}
			else
			{
				base.enabled = false;
			}
		}
	}

	private void CreateAttachSound()
	{
		this.m_AudioSource = base.gameObject.AddComponent<AudioSource>();
		this.m_AudioSource.clip = this.m_AudioClip;
		this.m_AudioSource.priority = this.m_nPriority;
		this.m_AudioSource.loop = this.m_bLoop;
		this.m_AudioSource.volume = this.m_fVolume;
		this.m_AudioSource.pitch = this.m_fPitch;
		this.m_AudioSource.Play();
		this.m_nCreateCount++;
	}

	public override void OnUpdateEffectSpeed(float fSpeedRate, bool bRuntime)
	{
		this.m_fDelayTime /= fSpeedRate;
		this.m_fRepeatTime /= fSpeedRate;
	}
}
