using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class IOSInterface : MonoBehaviour
{
	[DllImport("__Internal")]
	public static extern void U3DSetCallBack(string gameObjectName);

	[DllImport("__Internal")]
	public static extern void U3DCheckUpdateForUrl(string _url);

	[DllImport("__Internal")]
	public static extern void U3DSetDebugModel(bool isDebug);

	[DllImport("__Internal")]
	public static extern bool U3DGetDebugModel();

	[DllImport("__Internal")]
	public static extern void U3DGetChannelName();

	[DllImport("__Internal")]
	public static extern void U3DgetCurrentVersion();

	[DllImport("__Internal")]
	public static extern void U3DgetChannelAnySDK();

	[DllImport("__Internal")]
	public static extern void U3DgetChannelAdressId();

	[DllImport("__Internal")]
	public static extern void U3DgetChannelUniqueName();

	public void currentVersionFinished(string str)
	{
		PlatformUtils.Instance.currentVersionFinished(str);
	}

	public void OnGetChannelUniqueNameCallback(string str)
	{
		PlatformUtils.Instance.OnGetChannelUniqueNameCallback(str);
	}

	public void OnGetChannelAdressIdCallback(string str)
	{
		PlatformUtils.Instance.OnGetChannelAdressIdCallback(str);
	}

	public void OnGetChannelAnySDKCallback(string str)
	{
		PlatformUtils.Instance.OnGetChannelAnySDKCallBack(str);
	}

	public void SetChannelName(string str)
	{
		PlatformUtils.Instance.OnGetChannelNameCallBack(str);
	}
}
