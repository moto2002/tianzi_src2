using System;
using System.Collections.Generic;

public class AsyncTrigger
{
	private class FrameTriggerInfo
	{
		public int iFrameDelay = 1;

		public AsyncTrigger.OnTrigger onTrigger;

		public VarStore args;
	}

	public delegate void OnTrigger(VarStore args);

	private static bool frameTrigger = false;

	private static List<AsyncTrigger.FrameTriggerInfo> mFrameTrigger = new List<AsyncTrigger.FrameTriggerInfo>();

	public static bool IsTargetValid(object oFuncTarget)
	{
		return oFuncTarget == null || !oFuncTarget.Equals(null);
	}

	public static void CreateFrameTrigger(int frameDelay, AsyncTrigger.OnTrigger onTrigger, VarStore args)
	{
		AsyncTrigger.frameTrigger = true;
		AsyncTrigger.FrameTriggerInfo frameTriggerInfo = new AsyncTrigger.FrameTriggerInfo();
		frameTriggerInfo.iFrameDelay = ((frameDelay >= 1) ? frameDelay : 1);
		frameTriggerInfo.onTrigger = onTrigger;
		frameTriggerInfo.args = args;
		AsyncTrigger.mFrameTrigger.Add(frameTriggerInfo);
	}

	public static void UpdateFrameTrigger()
	{
		if (AsyncTrigger.frameTrigger)
		{
			AsyncTrigger.UpdateFrameTrigger(ref AsyncTrigger.frameTrigger);
		}
	}

	public static void UpdateFrameTrigger(ref bool update)
	{
		try
		{
			update = (AsyncTrigger.mFrameTrigger.Count > 0);
			if (update)
			{
				for (int i = 0; i < AsyncTrigger.mFrameTrigger.Count; i++)
				{
					AsyncTrigger.FrameTriggerInfo frameTriggerInfo = AsyncTrigger.mFrameTrigger[i];
					if (frameTriggerInfo == null)
					{
						AsyncTrigger.mFrameTrigger.RemoveAt(i);
						i--;
					}
					else
					{
						frameTriggerInfo.iFrameDelay--;
						if (frameTriggerInfo.iFrameDelay <= 1)
						{
							AsyncTrigger.mFrameTrigger.RemoveAt(i);
							if (frameTriggerInfo.onTrigger != null)
							{
								try
								{
									frameTriggerInfo.onTrigger(frameTriggerInfo.args);
								}
								catch (Exception ex)
								{
									LogSystem.LogError(new object[]
									{
										"UpdateFrameTrigger : ",
										ex.ToString()
									});
								}
							}
							i--;
						}
					}
				}
			}
		}
		catch (Exception ex2)
		{
			LogSystem.LogError(new object[]
			{
				ex2.ToString()
			});
		}
	}
}
