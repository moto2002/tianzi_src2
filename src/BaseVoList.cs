using System;
using UnityEngine;

public class BaseVoList : ScriptableObject
{
	private float DESTROY_TIME = 120f;

	private float m_time;

	public BaseVoList()
	{
		this.Reset();
	}

	public bool Check(float currentTime)
	{
		return currentTime - this.m_time > this.DESTROY_TIME;
	}

	public void Reset()
	{
		this.m_time = Time.realtimeSinceStartup;
	}

	public virtual void Destroy()
	{
	}
}
