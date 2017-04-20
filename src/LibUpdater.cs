using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class LibUpdater : MonoBehaviour
{
	public delegate void LibUpdaterCallBack();

	public delegate void GameLibCheckCallBack();

	public delegate void LibVersionCheckCallBack();

	public const string RVL = "LibVersion.xml";

	private LibUpdater.LibUpdaterCallBack libUpdaterCallBack;

	private LibUpdater.GameLibCheckCallBack gameLibCheckCallBack;

	private LibUpdater.LibVersionCheckCallBack libVersionCheckCallBack;

	private string mStrContent = string.Empty;

	public LibVersionData mLocalVersions;

	public LibVersionData mDestVersions;

	private GuideUI muiGuide;

	private void Awake()
	{
		base.gameObject.AddComponent<WebStreamLoader>();
		this.muiGuide = UnityEngine.Object.FindObjectOfType<GuideUI>();
	}

	public void StartUpdateLib(LibUpdater.LibUpdaterCallBack callback)
	{
		if (callback != null)
		{
			this.libUpdaterCallBack = callback;
		}
		this.LoadConfigFile();
	}

	private void LoadConfigFile()
	{
		Config.GameAddress updaterAddress = Config.GetUpdaterAddress();
		string text = string.Empty;
		if (updaterAddress != null)
		{
			text = updaterAddress.strDomainAddress;
		}
		text += "/Config/Config_Android.xml";
		LogSystem.LogWarning(new object[]
		{
			"LibUpdater Load : ",
			text
		});
		Instance.Get<WebStreamLoader>().ReadStream(text, new WebStreamLoader.OnWebReqCallBack(this.OnConfigLoadCallback));
	}

	private void OnConfigLoadCallback(string strUrl, WebLoadStatus eLoadStatus, string strContent)
	{
		if (eLoadStatus != WebLoadStatus.WLS_SUCCESS)
		{
			if (this.muiGuide != null)
			{
				string udpateLangage = Config.GetUdpateLangage("GameDataAnomalies");
				LogSystem.LogWarning(new object[]
				{
					udpateLangage,
					"OnConfigLoadCallback:",
					strUrl
				});
				this.muiGuide.ShowMBox(3, udpateLangage, new EventDelegate.Callback(this.LoadConfigFile), new EventDelegate.Callback(LaunchUtils.OnErrorQuit), string.Empty);
			}
			return;
		}
		Config.SetUpdateGuideInfo(strContent);
		this.LoadLibVersionFile();
	}

	private void OnLoadLocalLibVersionFile()
	{
		string path = Config.GetAssetBundleRootPath() + "/LibVersion.xml";
		if (File.Exists(path))
		{
			string xmlString = File.ReadAllText(path);
			this.mLocalVersions = this.ParserResVersionList(xmlString);
		}
		this.StartUpdateLibFile();
	}

	private void LoadLocalLibVersionFile(LibUpdater.LibVersionCheckCallBack callback)
	{
		this.libVersionCheckCallBack = callback;
		if (this.mDestVersions == null || string.IsNullOrEmpty(this.mDestVersions.md5Field))
		{
			string filePath = Config.GetStreamSuffix() + Config.GetStreamRootPath() + "/LibVersion.xml";
			base.StartCoroutine(this.OnLoadLocalLibVersionFile(filePath));
		}
		else
		{
			this.libVersionCheckCallBack();
		}
	}

	[DebuggerHidden]
	private IEnumerator OnLoadLocalLibVersionFile(string filePath)
	{
		LibUpdater.<OnLoadLocalLibVersionFile>c__Iterator1 <OnLoadLocalLibVersionFile>c__Iterator = new LibUpdater.<OnLoadLocalLibVersionFile>c__Iterator1();
		<OnLoadLocalLibVersionFile>c__Iterator.filePath = filePath;
		<OnLoadLocalLibVersionFile>c__Iterator.<$>filePath = filePath;
		<OnLoadLocalLibVersionFile>c__Iterator.<>f__this = this;
		return <OnLoadLocalLibVersionFile>c__Iterator;
	}

	private void LoadLibVersionFile()
	{
		string strUrl = string.Empty;
		string text = Config.GetUpdaterConfig("AbsPath", "Value") + "/";
		if (Config.mbVerifyVersion)
		{
			text += Config.GetUpdaterConfig("VerifySubDir", "Value");
			text = text + "/" + Config.GetVersionUseage();
		}
		else
		{
			text += Config.GetUpdaterConfig("StreamingSubDir", "Value");
		}
		Config.SetSourceRootPath(text);
		strUrl = text + "/LibVersion.xml";
		Instance.Get<WebStreamLoader>().ReadStream(strUrl, new WebStreamLoader.OnWebReqCallBack(this.OnLibVersionLoadCallback));
	}

	private void OnLibVersionLoadCallback(string strUrl, WebLoadStatus eLoadStatus, string strContent)
	{
		if (eLoadStatus != WebLoadStatus.WLS_SUCCESS)
		{
			this.LoadLocalLibVersionFile(new LibUpdater.LibVersionCheckCallBack(this.OnLoadLocalLibVersionFile));
			return;
		}
		this.mDestVersions = this.ParserResVersionList(strContent);
		this.mStrContent = strContent;
		this.LoadLocalLibVersionFile(new LibUpdater.LibVersionCheckCallBack(this.OnLoadLocalLibVersionFile));
	}

	private void StartUpdateLibFile()
	{
		if (this.mLocalVersions != null)
		{
			if (this.mDestVersions == null || string.IsNullOrEmpty(this.mDestVersions.md5Field))
			{
				Config.GameLibVersion = this.mLocalVersions.versionField;
				this.CheckLoaclGameLibFile(new LibUpdater.GameLibCheckCallBack(this.OnCopyLoaclGameLibFile));
				return;
			}
			if (string.Compare(this.mDestVersions.md5Field, this.mLocalVersions.md5Field) == 0)
			{
				Config.GameLibVersion = this.mLocalVersions.versionField;
				this.CheckLoaclGameLibFile(new LibUpdater.GameLibCheckCallBack(this.OnCheckLoaclGameLibFile));
				return;
			}
		}
		else if (this.mDestVersions == null || string.IsNullOrEmpty(this.mDestVersions.md5Field))
		{
			if (this.muiGuide != null)
			{
				string udpateLangage = Config.GetUdpateLangage("GameDataAnomalies");
				LogSystem.LogWarning(new object[]
				{
					udpateLangage,
					"StartUpdateLibFile:"
				});
				this.muiGuide.ShowMBox(3, udpateLangage, new EventDelegate.Callback(this.LoadConfigFile), new EventDelegate.Callback(LaunchUtils.OnErrorQuit), string.Empty);
			}
			return;
		}
		Config.GameLibVersion = this.mDestVersions.versionField;
		this.StartDownLoad();
	}

	public void OnCopyLoaclGameLibFile()
	{
		this.libUpdaterCallBack();
	}

	public void OnCheckLoaclGameLibFile()
	{
		string path = Config.GetAssetBundleRootPath() + "/" + this.mLocalVersions.idField + ".unity3d";
		if (File.Exists(path))
		{
			FileStream fileStream = new FileStream(path, FileMode.Open);
			string md = AssetFileUtils.GetMd5(fileStream);
			fileStream.Close();
			if (string.Compare(md, this.mDestVersions.md5Field) == 0)
			{
				this.libUpdaterCallBack();
				return;
			}
		}
		this.StartDownLoad();
	}

	private void CheckLoaclGameLibFile(LibUpdater.GameLibCheckCallBack callback)
	{
		this.gameLibCheckCallBack = callback;
		string text = Config.GetAssetBundleRootPath() + "/" + this.mLocalVersions.idField + ".unity3d";
		if (File.Exists(text))
		{
			FileStream fileStream = new FileStream(text, FileMode.Open);
			string md = AssetFileUtils.GetMd5(fileStream);
			fileStream.Close();
			if (string.Compare(md, this.mLocalVersions.md5Field) == 0)
			{
				this.gameLibCheckCallBack();
				return;
			}
			this.DeleteLocalLibFile(text);
		}
		if (!File.Exists(text))
		{
			string filePath = string.Concat(new string[]
			{
				Config.GetStreamSuffix(),
				Config.GetStreamRootPath(),
				"/",
				this.mLocalVersions.idField,
				".unity3d"
			});
			base.StartCoroutine(this.LoadGameLibFile(filePath));
			return;
		}
		this.gameLibCheckCallBack();
	}

	[DebuggerHidden]
	private IEnumerator LoadGameLibFile(string filePath)
	{
		LibUpdater.<LoadGameLibFile>c__Iterator2 <LoadGameLibFile>c__Iterator = new LibUpdater.<LoadGameLibFile>c__Iterator2();
		<LoadGameLibFile>c__Iterator.filePath = filePath;
		<LoadGameLibFile>c__Iterator.<$>filePath = filePath;
		<LoadGameLibFile>c__Iterator.<>f__this = this;
		return <LoadGameLibFile>c__Iterator;
	}

	private void StartDownLoad()
	{
		if (this.mLocalVersions != null)
		{
			string strPath = Config.GetAssetBundleRootPath() + "/" + this.mLocalVersions.idField + ".unity3d";
			this.DeleteLocalLibFile(strPath);
		}
		base.StartCoroutine(this.DownloadFile());
	}

	[DebuggerHidden]
	private IEnumerator DownloadFile()
	{
		LibUpdater.<DownloadFile>c__Iterator3 <DownloadFile>c__Iterator = new LibUpdater.<DownloadFile>c__Iterator3();
		<DownloadFile>c__Iterator.<>f__this = this;
		return <DownloadFile>c__Iterator;
	}

	private void DeleteLocalLibFile(string strPath)
	{
		try
		{
			if (File.Exists(strPath))
			{
				File.Delete(strPath);
			}
		}
		catch (IOException ex)
		{
			LogSystem.LogError(new object[]
			{
				ex.ToString()
			});
		}
	}

	private LibVersionData ParserResVersionList(string xmlString)
	{
		LibVersionData libVersionData = new LibVersionData();
		if (string.IsNullOrEmpty(xmlString))
		{
			LogSystem.LogWarning(new object[]
			{
				"ParserResVersionList error"
			});
			return null;
		}
		int num = xmlString.IndexOf('<');
		if (num < 0)
		{
			LogSystem.LogWarning(new object[]
			{
				"ParserResVersionList Content error"
			});
			return null;
		}
		xmlString = xmlString.Substring(num);
		xmlString.Trim();
		XMLParser xMLParser = new XMLParser();
		XMLNode xMLNode = xMLParser.Parse(xmlString);
		if (xMLNode == null)
		{
			LogSystem.LogWarning(new object[]
			{
				"ParserResVersionList Parser Content error"
			});
			return null;
		}
		XMLNodeList xMLNodeList = (XMLNodeList)xMLNode["VersionAssets"];
		if (xMLNodeList == null)
		{
			LogSystem.LogWarning(new object[]
			{
				"ParserResVersionList Parser no node error"
			});
			return null;
		}
		for (int i = 0; i < xMLNodeList.Count; i++)
		{
			XMLNode xMLNode2 = xMLNodeList[i] as XMLNode;
			XMLNodeList nodeList = xMLNode2.GetNodeList("VersionAsset");
			if (nodeList != null)
			{
				for (int j = 0; j < nodeList.Count; j++)
				{
					XMLNode xMLNode3 = nodeList[j] as XMLNode;
					libVersionData.idField = xMLNode3.GetValue("@ID");
					libVersionData.md5Field = xMLNode3.GetValue("@Md5");
					libVersionData.versionField = xMLNode3.GetValue("@Version");
				}
			}
		}
		return libVersionData;
	}

	private void WriteLocalRVL(string strContent)
	{
		if (this.mDestVersions == null || string.IsNullOrEmpty(this.mDestVersions.md5Field))
		{
			return;
		}
		try
		{
			string assetBundleRootPath = Config.GetAssetBundleRootPath();
			if (!Directory.Exists(assetBundleRootPath))
			{
				Directory.CreateDirectory(assetBundleRootPath);
			}
			string path = assetBundleRootPath + "/LibVersion.xml";
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			FileStream fileStream = new FileStream(path, FileMode.CreateNew);
			if (fileStream != null)
			{
				StreamWriter streamWriter = new StreamWriter(fileStream);
				if (streamWriter != null)
				{
					streamWriter.Write(strContent);
					streamWriter.Flush();
					streamWriter.Close();
				}
				fileStream.Close();
			}
		}
		catch (Exception ex)
		{
			this.muiGuide.ShowMBox(4, Config.GetUdpateLangage("LocalWriteError"), new EventDelegate.Callback(this.LoadConfigFile), new EventDelegate.Callback(LaunchUtils.OnErrorQuit), string.Empty);
			LogSystem.LogError(new object[]
			{
				ex.ToString()
			});
		}
	}

	private string GetAssetName(string id, string path)
	{
		string text = path + "/" + id;
		text.ToLower();
		text.Replace(" ", string.Empty);
		return text;
	}

	private string GetAssetBundlePath(string id, string path)
	{
		string text = Config.GetAssetBundleRootPath();
		string text2 = text;
		text = string.Concat(new string[]
		{
			text2,
			"/",
			path,
			"/",
			id,
			".unity3d"
		});
		text.ToLower();
		text.Replace(" ", string.Empty);
		return text;
	}
}
