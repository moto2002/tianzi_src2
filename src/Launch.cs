using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class Launch : MonoBehaviour
{
	private LibUpdater libUpdater;

	private GuideUI guideui;

	private void SetRootPath()
	{
		string persistentDataPath = Application.persistentDataPath;
		string streamRootPath = Application.dataPath + "/StreamingAssets";
		string streamSuffix = "jar:File://";
		streamRootPath = Application.dataPath + "/!/assets";
		persistentDataPath = Application.persistentDataPath;
		Config.SetStreamSuffix(streamSuffix);
		Config.SetAssetBundleRootPath(persistentDataPath);
		Config.SetStreamRootPath(streamRootPath);
	}

	private void InitLogSystem()
	{
		try
		{
			string strLogFile = string.Empty;
			strLogFile = Application.persistentDataPath;
			Screen.sleepTimeout = -1;
			strLogFile = Application.persistentDataPath + "/System.Log";
			Screen.sleepTimeout = -1;
			LogSystem.Init(strLogFile, true, false, 256);
			LogSocket.Init();
		}
		catch (Exception ex)
		{
			LogSystem.LogError(new object[]
			{
				"RunLogSystem",
				ex.ToString()
			});
		}
	}

	private void InitConfig()
	{
		if (this.guideui != null)
		{
			this.guideui.ShowInfo(Config.GetUdpateLangage("LoadConfig"));
		}
		TextAsset textAsset = Resources.Load("Local/Config/Version") as TextAsset;
		if (textAsset != null)
		{
			if (!string.IsNullOrEmpty(textAsset.text))
			{
				string[] array = textAsset.text.Split(new char[]
				{
					'|'
				});
				if (array.Length >= 5)
				{
					Config.SetLocalVersion(array[0]);
					Config.SetLocalNumberVersion(array[1]);
					Config.SetVersionUseage(array[2]);
					Config.SetInstallationVersion(array[3]);
					Config.SetLaunchVersion(array[4]);
				}
				LogSystem.LogWarning(new object[]
				{
					"Version : ",
					textAsset.text
				});
			}
			Resources.UnloadAsset(textAsset);
		}
		else
		{
			Config.SetLocalVersion(string.Empty);
		}
		TextAsset textAsset2 = Resources.Load("Local/Config/Login") as TextAsset;
		if (textAsset2 != null)
		{
			Config.SetUpdaterAddress(textAsset2);
			Resources.UnloadAsset(textAsset2);
		}
	}

	private void InitGuideUI()
	{
		GameObject gameObject = new GameObject("GuideUI");
		this.guideui = gameObject.AddComponent<GuideUI>();
		this.guideui.CreateBackPanel();
	}

	private void InitPlatform()
	{
		PlatformUtils.Instance.Init(new PlatformUtils.InitFinishedCallBack(this.OnInitFinishedCallBack));
	}

	private void OnInitFinishedCallBack(bool bResult)
	{
		if (bResult)
		{
			this.StartNetCheck();
		}
		else if (this.guideui != null)
		{
			this.guideui.ShowMBox(3, Config.GetUdpateLangage("SDKInitError"), new EventDelegate.Callback(this.InitPlatform), new EventDelegate.Callback(LaunchUtils.OnErrorQuit), string.Empty);
		}
	}

	private void InitLibUpdater()
	{
		this.libUpdater = base.gameObject.AddComponent<LibUpdater>();
		this.libUpdater.StartUpdateLib(new LibUpdater.LibUpdaterCallBack(this.OnUpdateLibCallBack));
	}

	private void OnUpdateLibCallBack()
	{
		Reflecter reflecter = base.gameObject.AddComponent<Reflecter>();
		reflecter.LoadScriptLib();
	}

	private void Awake()
	{
		base.gameObject.name = Config.LaunchName;
	}

	[DebuggerHidden]
	private IEnumerator InitSystem()
	{
		Launch.<InitSystem>c__Iterator0 <InitSystem>c__Iterator = new Launch.<InitSystem>c__Iterator0();
		<InitSystem>c__Iterator.<>f__this = this;
		return <InitSystem>c__Iterator;
	}

	private void OnInitConfigInfos()
	{
		if (!Language.IS_Voerseas)
		{
			this.OnShowExplain();
		}
		else if (Language.LanguageVersion == Language.LanguageType.Japan)
		{
			this.OnShowExplain();
		}
		else
		{
			this.OnShowExplainComplete();
		}
	}

	private void OnShowExplain()
	{
		GameObject gameObject = new GameObject("ExplainUI");
		ExplainUI explainUI = gameObject.AddComponent<ExplainUI>();
		explainUI.SetCallBack(new Action(this.OnShowExplainComplete));
	}

	private void OnShowExplainComplete()
	{
		this.InitGuideUI();
		this.InitConfig();
		this.InitPlatform();
	}

	private void Start()
	{
		base.StartCoroutine("InitSystem");
	}

	private void RunGame()
	{
		if (Application.isPlaying && Config.SnailFont == null)
		{
			Config.NGUIFont = Config.LoadUIFont("NGUIFont");
			Config.SnailFont = Config.LoadUIFont("SnailFont");
		}
		this.InitLibUpdater();
	}

	private void StartNetCheck()
	{
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			if (this.guideui != null)
			{
				this.guideui.ShowMBox(3, Config.GetUdpateLangage("netError"), new EventDelegate.Callback(this.StartNetCheck), new EventDelegate.Callback(LaunchUtils.OnErrorQuit), string.Empty);
			}
		}
		else
		{
			this.RunGame();
		}
	}
}
