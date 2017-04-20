using System;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
	private class TimerList
	{
		private int count;

		private List<string> Keys = new List<string>();

		private List<TimerManager.Timer> Values = new List<TimerManager.Timer>();

		public int Count
		{
			get
			{
				return this.count;
			}
		}

		public void Add(string key, TimerManager.Timer value)
		{
			if (!this.Contains(key))
			{
				this.Keys.Add(key);
				this.Values.Add(value);
				this.count = this.Keys.Count;
			}
		}

		public void Remove(string key)
		{
			int num = this.Keys.IndexOf(key);
			if (num > -1)
			{
				this.Keys.RemoveAt(num);
				this.Values.RemoveAt(num);
				this.count = this.Keys.Count;
			}
		}

		public void GetKeyPairValue(ref string key, ref TimerManager.Timer time, int index)
		{
			if (this.Keys.Count > index)
			{
				key = this.Keys[index];
				time = this.Values[index];
			}
		}

		public void Replace(string key, TimerManager.Timer time)
		{
			int num = this.Keys.IndexOf(key);
			if (num > -1)
			{
				this.Values[num] = time;
			}
		}

		public string GetKey(int index)
		{
			if (this.Keys.Count > index)
			{
				return this.Keys[index];
			}
			return string.Empty;
		}

		public TimerManager.Timer GetValue(int index)
		{
			if (this.Values.Count > index)
			{
				return this.Values[index];
			}
			return null;
		}

		public bool Contains(string key)
		{
			return this.Keys.Contains(key);
		}

		public void Clear()
		{
			this.Keys.Clear();
			this.Values.Clear();
			this.count = 0;
		}
	}

	private enum TIMER_MODE
	{
		NORMAL,
		REPEAT,
		COUNTTIME,
		DELAYTIME
	}

	private class Timer
	{
		private string m_Name;

		private TimerManager.TIMER_MODE m_Mode;

		private float m_StartTime;

		private float m_duration;

		private float m_time;

		private TimerManager.TimerManagerHandler m_TimerEvent;

		private TimerManager.TimerManagerHandlerArgs m_TimerArgsEvent;

		private TimerManager.TimerManagerCountHandlerArgs m_TimerCountArgsEvent;

		private object[] m_Args;

		public float StartTime
		{
			get
			{
				return this.m_StartTime;
			}
			set
			{
				this.m_StartTime = value;
			}
		}

		public float TimeLeft
		{
			get
			{
				return Mathf.Max(0f, this.m_duration - (Time.realtimeSinceStartup - this.m_StartTime));
			}
		}

		public Timer(string name, TimerManager.TIMER_MODE mode, float startTime, float duration, TimerManager.TimerManagerHandler handler)
		{
			this.m_Name = name;
			this.m_Mode = mode;
			this.m_StartTime = startTime;
			this.m_duration = duration;
			this.m_TimerEvent = handler;
		}

		public Timer(string name, TimerManager.TIMER_MODE mode, float startTime, float duration, TimerManager.TimerManagerHandlerArgs handler, params object[] args)
		{
			this.m_Name = name;
			this.m_Mode = mode;
			this.m_StartTime = startTime;
			this.m_duration = duration;
			this.m_TimerArgsEvent = handler;
			this.m_Args = args;
		}

		public Timer(string name, TimerManager.TIMER_MODE mode, float startTime, float duration, TimerManager.TimerManagerCountHandlerArgs handler, params object[] args)
		{
			this.m_Name = name;
			this.m_Mode = mode;
			this.m_StartTime = startTime;
			this.m_duration = duration;
			this.m_TimerCountArgsEvent = handler;
			this.m_Args = args;
		}

		public void Run()
		{
			if (this.m_Mode == TimerManager.TIMER_MODE.DELAYTIME)
			{
				if (Time.realtimeSinceStartup - this.m_StartTime > this.m_duration)
				{
					if (this.m_TimerEvent != null && AsyncTrigger.IsTargetValid(this.m_TimerEvent.Target))
					{
						try
						{
							this.m_TimerEvent();
						}
						catch (Exception ex)
						{
							TimerManager.DestroyTimer(this.m_Name);
							LogSystem.LogError(new object[]
							{
								"Time event catch 1",
								ex.ToString()
							});
						}
					}
					else if (this.m_TimerArgsEvent != null && AsyncTrigger.IsTargetValid(this.m_TimerArgsEvent.Target))
					{
						try
						{
							this.m_TimerArgsEvent(this.m_Args);
						}
						catch (Exception ex2)
						{
							TimerManager.DestroyTimer(this.m_Name);
							LogSystem.LogError(new object[]
							{
								"Time event catch 1",
								ex2.ToString()
							});
						}
					}
					TimerManager.DestroyTimer(this.m_Name);
				}
				return;
			}
			if (this.m_Mode == TimerManager.TIMER_MODE.COUNTTIME)
			{
				float num = Time.realtimeSinceStartup - this.m_time;
				if (num > 1f)
				{
					this.m_time = Time.realtimeSinceStartup;
					try
					{
						if (this.m_TimerCountArgsEvent != null && AsyncTrigger.IsTargetValid(this.m_TimerCountArgsEvent.Target))
						{
							this.m_TimerCountArgsEvent(Mathf.CeilToInt(this.TimeLeft), this.m_Args);
						}
					}
					catch (Exception ex3)
					{
						TimerManager.DestroyTimer(this.m_Name);
						LogSystem.LogError(new object[]
						{
							"Time event catch2 ",
							ex3.ToString()
						});
					}
					if (this.TimeLeft <= 0f)
					{
						TimerManager.DestroyTimer(this.m_Name);
					}
				}
				return;
			}
			if (this.TimeLeft > 0f)
			{
				return;
			}
			try
			{
				if (this.m_TimerEvent != null && AsyncTrigger.IsTargetValid(this.m_TimerEvent.Target))
				{
					this.m_TimerEvent();
				}
				if (this.m_TimerArgsEvent != null && AsyncTrigger.IsTargetValid(this.m_TimerArgsEvent.Target))
				{
					this.m_TimerArgsEvent(this.m_Args);
				}
			}
			catch (Exception ex4)
			{
				TimerManager.DestroyTimer(this.m_Name);
				LogSystem.LogError(new object[]
				{
					"Time event catch3",
					ex4.ToString()
				});
			}
			if (this.m_Mode == TimerManager.TIMER_MODE.NORMAL)
			{
				TimerManager.DestroyTimer(this.m_Name);
			}
			else
			{
				this.m_StartTime = Time.realtimeSinceStartup;
			}
		}
	}

	public delegate void TimerManagerHandler();

	public delegate void TimerManagerHandlerArgs(params object[] args);

	public delegate void TimerManagerCountHandlerArgs(int count, params object[] args);

	private static TimerManager.TimerList m_TimerList = new TimerManager.TimerList();

	private static TimerManager.TimerList m_AddTimerList = new TimerManager.TimerList();

	private static List<string> m_DestroyTimerList = new List<string>();

	public static void ClearTimer()
	{
		TimerManager.m_DestroyTimerList.Clear();
		TimerManager.m_AddTimerList.Clear();
		TimerManager.m_TimerList.Clear();
	}

	private void Update()
	{
		if (TimerManager.m_DestroyTimerList.Count > 0)
		{
			for (int i = 0; i < TimerManager.m_DestroyTimerList.Count; i++)
			{
				TimerManager.m_TimerList.Remove(TimerManager.m_DestroyTimerList[i]);
			}
			TimerManager.m_DestroyTimerList.Clear();
		}
		if (TimerManager.m_AddTimerList.Count > 0)
		{
			string empty = string.Empty;
			TimerManager.Timer timer = null;
			int j = 0;
			int count = TimerManager.m_AddTimerList.Count;
			while (j < count)
			{
				TimerManager.m_AddTimerList.GetKeyPairValue(ref empty, ref timer, j);
				if (timer != null)
				{
					if (TimerManager.m_TimerList.Contains(empty))
					{
						TimerManager.m_TimerList.Replace(empty, timer);
					}
					else
					{
						TimerManager.m_TimerList.Add(empty, timer);
					}
				}
				j++;
			}
			TimerManager.m_AddTimerList.Clear();
		}
		if (TimerManager.m_TimerList.Count > 0)
		{
			int k = 0;
			int count2 = TimerManager.m_TimerList.Count;
			while (k < count2)
			{
				TimerManager.Timer value = TimerManager.m_TimerList.GetValue(k);
				if (value == null)
				{
					return;
				}
				value.Run();
				if (TimerManager.m_TimerList.Count == 0)
				{
					return;
				}
				k++;
			}
		}
	}

	public static bool AddTimer(string key, float duration, TimerManager.TimerManagerHandler handler)
	{
		return TimerManager.Internal_AddTimer(key, TimerManager.TIMER_MODE.NORMAL, duration, handler);
	}

	public static bool AddTimer(string key, float duration, TimerManager.TimerManagerHandlerArgs handler, params object[] args)
	{
		return TimerManager.Internal_AddTimer(key, TimerManager.TIMER_MODE.NORMAL, duration, handler, args);
	}

	public static bool AddTimerRepeat(string key, float duration, TimerManager.TimerManagerHandler handler)
	{
		return TimerManager.Internal_AddTimer(key, TimerManager.TIMER_MODE.REPEAT, duration, handler);
	}

	public static bool AddTimerRepeat(string key, float duration, TimerManager.TimerManagerHandlerArgs handler, params object[] args)
	{
		return TimerManager.Internal_AddTimer(key, TimerManager.TIMER_MODE.REPEAT, duration, handler, args);
	}

	public static bool AddTimerCount(string key, float duration, TimerManager.TimerManagerCountHandlerArgs handler, params object[] args)
	{
		return TimerManager.Internal_AddTimer(key, TimerManager.TIMER_MODE.COUNTTIME, duration, handler, args);
	}

	public static bool AddDelayTimer(string key, float duration, TimerManager.TimerManagerHandler handler)
	{
		if (string.IsNullOrEmpty(key))
		{
			return false;
		}
		if (duration < 0f)
		{
			if (handler != null)
			{
				handler();
			}
			return true;
		}
		TimerManager.Timer timer = new TimerManager.Timer(key, TimerManager.TIMER_MODE.DELAYTIME, Time.realtimeSinceStartup, duration, handler);
		if (TimerManager.m_AddTimerList.Contains(key))
		{
			TimerManager.m_AddTimerList.Replace(key, timer);
		}
		else
		{
			TimerManager.m_AddTimerList.Add(key, timer);
		}
		return true;
	}

	public static bool AddDelayTimer(string key, float duration, TimerManager.TimerManagerHandlerArgs handler, params object[] args)
	{
		if (string.IsNullOrEmpty(key))
		{
			return false;
		}
		if (duration < 0f)
		{
			if (handler != null)
			{
				handler(new object[0]);
			}
			return true;
		}
		TimerManager.Timer timer = new TimerManager.Timer(key, TimerManager.TIMER_MODE.DELAYTIME, Time.realtimeSinceStartup, duration, handler, args);
		if (TimerManager.m_AddTimerList.Contains(key))
		{
			TimerManager.m_AddTimerList.Replace(key, timer);
		}
		else
		{
			TimerManager.m_AddTimerList.Add(key, timer);
		}
		return true;
	}

	public static void ClearTimerWithPrefix(string prefix)
	{
		if (TimerManager.m_TimerList != null && TimerManager.m_TimerList.Count > 0)
		{
			for (int i = 0; i < TimerManager.m_TimerList.Count; i++)
			{
				string key = TimerManager.m_TimerList.GetKey(i);
				if (key.StartsWith(prefix))
				{
					TimerManager.DestroyTimer(key);
				}
			}
		}
	}

	public static bool DestroyTimer(string key)
	{
		if (!TimerManager.m_TimerList.Contains(key))
		{
			return false;
		}
		if (!TimerManager.m_DestroyTimerList.Contains(key))
		{
			TimerManager.m_DestroyTimerList.Add(key);
		}
		return true;
	}

	public static bool IsHaveTimer(string key)
	{
		return TimerManager.m_TimerList.Contains(key) || TimerManager.m_AddTimerList.Contains(key);
	}

	private static bool Internal_AddTimer(string key, TimerManager.TIMER_MODE mode, float duration, TimerManager.TimerManagerHandler handler)
	{
		if (string.IsNullOrEmpty(key))
		{
			return false;
		}
		if (duration < 0f)
		{
			return false;
		}
		TimerManager.Timer timer = new TimerManager.Timer(key, mode, Time.realtimeSinceStartup, duration, handler);
		if (TimerManager.m_AddTimerList.Contains(key))
		{
			TimerManager.m_AddTimerList.Replace(key, timer);
		}
		else
		{
			TimerManager.m_AddTimerList.Add(key, timer);
		}
		return true;
	}

	private static bool Internal_AddTimer(string key, TimerManager.TIMER_MODE mode, float duration, TimerManager.TimerManagerHandlerArgs handler, params object[] args)
	{
		if (string.IsNullOrEmpty(key))
		{
			return false;
		}
		if (duration < 0f)
		{
			return false;
		}
		TimerManager.Timer timer = new TimerManager.Timer(key, mode, Time.realtimeSinceStartup, duration, handler, args);
		if (TimerManager.m_AddTimerList.Contains(key))
		{
			TimerManager.m_AddTimerList.Replace(key, timer);
		}
		else
		{
			TimerManager.m_AddTimerList.Add(key, timer);
		}
		return true;
	}

	private static bool Internal_AddTimer(string key, TimerManager.TIMER_MODE mode, float duration, TimerManager.TimerManagerCountHandlerArgs handler, params object[] args)
	{
		if (string.IsNullOrEmpty(key))
		{
			return false;
		}
		if (duration < 0f)
		{
			return false;
		}
		TimerManager.Timer timer = new TimerManager.Timer(key, mode, Time.realtimeSinceStartup, duration, handler, args);
		if (TimerManager.m_AddTimerList.Contains(key))
		{
			TimerManager.m_AddTimerList.Replace(key, timer);
		}
		else
		{
			TimerManager.m_AddTimerList.Add(key, timer);
		}
		return true;
	}

	public static bool IsRunning(string key)
	{
		return TimerManager.m_TimerList.Contains(key);
	}
}
