using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config
{
	public class GameAddress
	{
		public string strID = string.Empty;

		public string strName = string.Empty;

		public string strDomainAddress = string.Empty;

		public string strIPAddress = string.Empty;
	}

	public class PushServerInfo
	{
		public string strServerID = string.Empty;

		public string strServerName = string.Empty;

		public string strServerIP = string.Empty;

		public string strServerPort = string.Empty;

		public string strApiKey = string.Empty;
	}

	public class CustomInfo
	{
		public string strCustomInfoID = string.Empty;

		public string strCustomInfoVaule = string.Empty;
	}

	public class NoticeInfo
	{
		public string strText;

		public int iIndex;
	}

	public enum LoadStatus
	{
		LS_INIT,
		LS_FAILED,
		LS_SUCCUSED,
		LS_MAX
	}

	public delegate void CallLoadAsset(string strFileName, AssetCallBack callback);

	public const string ANYSDK = "ANYSDK";

	public const string USER_ACCOUNT = "useraccount";

	public const string ACCOUNT_ID = "accountid";

	public const string USER_NAME = "user_name";

	public const string SERVERID = "serverid";

	public const string GAME_ID = "game_id";

	public const string APPSTORE_URL = "appstore_url";

	public const string GAME_FONT_NAME = "NGUIFont";

	private static UIFont snailFont = null;

	private static UIFont nGuiFont = null;

	public static bool bANYSDK = false;

	public static string LaunchName = "Launch";

	public static string MessageName = "MessageName";

	public static string LanguagePath = "Language/";

	public static bool bGoogleAccount = false;

	public static string strGoogleAccount = "#google_gmid";

	public static bool bAppStore = false;

	public static bool bGameInitFailed = false;

	public static bool bisOpenCommentValue = true;

	public static string Appstore = "appstore";

	public static string BundleIDSuffix = "chosen.";

	public static string Snail = string.Empty;

	public static string DefaultBundleIdentifier = "com.snailgames.chosen.snail";

	public static string DefaulBundleIdMac = "com.snailgames.chosen.mac";

	public static string serverName = string.Empty;

	public static string gameId = string.Empty;

	public static string accessId = string.Empty;

	public static string accessPassword = string.Empty;

	public static string accessType = string.Empty;

	public static string seed = string.Empty;

	public static string serverId = string.Empty;

	public static string pushPhoneTypeName = string.Empty;

	public static string dataCollectionUrl = string.Empty;

	public static bool isPickGuildReward = false;

	public static string appid = string.Empty;

	public static string appkey = string.Empty;

	public static string registerServerUrl = string.Empty;

	public static bool isFirstLogin = false;

	public static bool isServerReconnect = true;

	public static bool mbEnableCatcher = false;

	public static bool ErrorLogPhoneOpen = false;

	public static bool MonthCardOpne = false;

	public static string FrameAddress = string.Empty;

	public static bool functionBtn_ExchangeShop = true;

	public static bool functionBtn_PetShop = true;

	public static bool functionBtn_LoveMain = true;

	public static bool functionBtn_PetMain = true;

	public static bool functionBtn_PropShop = true;

	public static bool functionBtn_MountBoold = true;

	public static bool functionBtn_SmeltEquip = true;

	public static bool functionBtn_ChangeJob = true;

	public static bool functionBtn_CDKey = true;

	public static bool functionBtn_Rechange = true;

	public static bool functionBtn_BlackMarket = true;

	public static bool functionBtn_1vs1 = true;

	public static bool functionBtn_25vs25 = true;

	public static bool functionBtn_SendFlower = true;

	public static bool functionBtn_TeacherAndPupil = true;

	public static bool functionBtn_TheFirstMyth = true;

	public static bool functionBtn_NationalBanquet = true;

	public static bool functionBtn_PetReincarnated = true;

	public static bool functionBtn_NationWar = true;

	public static bool functionBtn_GangWar = true;

	public static bool functionBtn_GangResourcesWar = true;

	public static bool functionBtn_BattleWar = true;

	public static bool functionBtn_EnableChangeNewJob = true;

	public static bool functionBtn_woniu_coin = true;

	public static bool functionBtn_vk_invite = true;

	public static bool debug = false;

	public string versionUpdateUrl = string.Empty;

	public static string channelId = string.Empty;

	public static string operatorId = string.Empty;

	public static string operatorChannelId = string.Empty;

	public static bool IsInstallWechat = false;

	public static string channelName = string.Empty;

	public static string cid = string.Empty;

	public static string idfa = string.Empty;

	public static string share_Gift_1 = string.Empty;

	public static string share_Gift_2 = string.Empty;

	public static int floatMenuShow = 0;

	public static string LifeCycleStr = string.Empty;

	public static string SceneID = string.Empty;

	public static string WeChatShareImgUrl = string.Empty;

	public static string EventPreventWallowUrl = string.Empty;

	public static string u3dErrorCollectUrl = string.Empty;

	public static string errorCollectUrl = string.Empty;

	public static string dataCollectUrl = string.Empty;

	public static string crashCollectUrl = string.Empty;

	public static string strChannelUniqueName = string.Empty;

	public static string strAdressId = string.Empty;

	public static string BundleIdentifier = string.Empty;

	public static Dictionary<string, List<string>> accInfo = new Dictionary<string, List<string>>();

	public static string strShowAppAdUrl = "http;//10.203.11.48/ad/ad.json";

	public static string appStoreURL = string.Empty;

	public static string ClientInstallUrl = string.Empty;

	public static string phoneType = string.Empty;

	public static string version = string.Empty;

	public static string wxAndroidId = string.Empty;

	public static string wxiOSId = string.Empty;

	public static string pushServiceIP = string.Empty;

	public static string pushServicePOST = string.Empty;

	public static string channelAppSecret = string.Empty;

	public static string Weixin_Link_Url = string.Empty;

	public static string act_Url = string.Empty;

	public static string Pc_RechargeUrl = string.Empty;

	public static string Pc_BackendUrl = string.Empty;

	public static string Pc_FontendUrl = string.Empty;

	public static string Pc_DESCryptoKey = string.Empty;

	public static string Pc_DESCryptoIV = string.Empty;

	public static string Pc_Merchantid = string.Empty;

	public static string curreRoleName = string.Empty;

	public static string FaceBook_Like_Url = string.Empty;

	public static string FaceBook_Share_strTitle = string.Empty;

	public static string FaceBook_Share_strContent = string.Empty;

	public static string FaceBook_Share_imageUrl = string.Empty;

	public static string FaceBook_Share_contentUrl = string.Empty;

	public static string VK_Like_Url = string.Empty;

	public static string VK_Share_strTitle = string.Empty;

	public static string VK_Share_strContent = string.Empty;

	public static string VK_Share_imageUrl = string.Empty;

	public static string VK_Share_contentUrl = string.Empty;

	public static string payCallBackURL = string.Empty;

	public static string payChannelId = string.Empty;

	public static string strAppStoreID = string.Empty;

	public static string strNoticeName = string.Empty;

	public static string strChannelName = string.Empty;

	public static string strTwNotionUrl = string.Empty;

	public static string strTwShoubing = string.Empty;

	public static string strKrNotionUrl1 = string.Empty;

	public static string strKrNotionUrl2 = string.Empty;

	public static string strTwNotionConfig = string.Empty;

	public static string strTwFBImgUrl = string.Empty;

	public static string strTwFBAndroidApkUrl = string.Empty;

	private static bool mbUseCacheWWW = false;

	public static int iCacheVersion = 0;

	public static string isOpenFPS = string.Empty;

	public static string isOpenFPSFile = string.Empty;

	public static string isOpenPkLimit = string.Empty;

	public static string IsGMOpen = string.Empty;

	public static bool IsScanCodeOpen = false;

	public static string IsSpecialGemOpen = string.Empty;

	public static string IsOpenVideo = string.Empty;

	public static bool bNeedDataCollect = false;

	public static bool bNeedWechatShare = false;

	public static int mQueueWaitTime = 5;

	public static string accountName = string.Empty;

	public static bool bCanSwithAccount = false;

	public static Dictionary<string, Config.GameAddress> mUpdaterAddress = new Dictionary<string, Config.GameAddress>();

	public static Dictionary<string, Dictionary<string, string>> mDictUpdaterGuide = new Dictionary<string, Dictionary<string, string>>();

	public static List<Dictionary<string, string>> mDictServerList = new List<Dictionary<string, string>>();

	public static List<Dictionary<string, string>> mDictAllServerList = new List<Dictionary<string, string>>();

	public static Dictionary<string, string> mWordsDict = new Dictionary<string, string>();

	public static string mstrShopData = string.Empty;

	public static string mstrServerNotice = string.Empty;

	public static DictionaryEx<int, List<string>> mDeviceQualityConfig = new DictionaryEx<int, List<string>>();

	public static Dictionary<string, Config.CustomInfo> mDictCustomInfoList = new Dictionary<string, Config.CustomInfo>();

	public static List<Config.PushServerInfo> mDictPushServerList = new List<Config.PushServerInfo>();

	public static string mstrPreSuffix = "File://";

	public static string mstrStreamSuffix = "File://";

	public static string mstrAssetBundleRootPath = string.Empty;

	public static string mstrSourceResRootPath = string.Empty;

	public static string mstrStreamResRootPath = string.Empty;

	public static string mstrMacAddress = string.Empty;

	public static string mstrGoogleId = string.Empty;

	public static string mstrVersionUseage = string.Empty;

	public static string mstrInstallationVersion = string.Empty;

	public static string mstrLocalVersion = string.Empty;

	public static int miLocalNumberVersion = 1;

	public static string mstrAssetBundleStreamResRootPath = string.Empty;

	private static bool isInitMenu = false;

	public static Config.CallLoadAsset monLoadAsset = null;

	private static Dictionary<string, UIAtlas> mAtlasTable = new Dictionary<string, UIAtlas>();

	private static Dictionary<string, UIFont> mFontsTable = new Dictionary<string, UIFont>();

	public static string ConfigXMLPath
	{
		get
		{
			return "/Config/" + Language.LanguageVersion.ToString();
		}
	}

	public static UIFont SnailFont
	{
		get
		{
			return Config.snailFont;
		}
		set
		{
			Config.snailFont = value;
		}
	}

	public static UIFont NGUIFont
	{
		get
		{
			return Config.nGuiFont;
		}
		set
		{
			Config.nGuiFont = value;
		}
	}

	public static bool bEditor
	{
		get
		{
			return false;
		}
	}

	public static bool bAndroid
	{
		get
		{
			return true;
		}
	}

	public static bool bIPhone
	{
		get
		{
			return false;
		}
	}

	public static bool bWin
	{
		get
		{
			return false;
		}
	}

	public static bool bMac
	{
		get
		{
			return false;
		}
	}

	public static bool OpenVoice
	{
		get
		{
			return false;
		}
	}

	public static string ResourceVersion
	{
		get;
		private set;
	}

	public static string GameLibVersion
	{
		get;
		set;
	}

	public static string ScreenResolution
	{
		get
		{
			return ResolutionConstrain.Instance.width + "*" + ResolutionConstrain.Instance.height;
		}
	}

	public static bool mbVerifyVersion
	{
		get
		{
			if (string.IsNullOrEmpty(Config.mstrLocalVersion))
			{
				return false;
			}
			string updaterConfig = Config.GetUpdaterConfig("VerifyVerison", "Value");
			return !string.IsNullOrEmpty(updaterConfig) && Config.mstrLocalVersion == updaterConfig;
		}
	}

	public static string Pc_UrlTC
	{
		get;
		private set;
	}

	public static bool Pc_TraceLog
	{
		get;
		private set;
	}

	public static string launchVersion
	{
		get;
		private set;
	}

	public static string ProjectName
	{
		get;
		private set;
	}

	public static string strCollectUrl
	{
		get;
		set;
	}

	public static UIFont LoadUIFont(string strFontName)
	{
		if (Application.isPlaying)
		{
			string path = "Local/Fonts/" + strFontName;
			UnityEngine.Object @object = Resources.Load(path);
			if (@object != null)
			{
				GameObject gameObject = @object as GameObject;
				UIFont component = gameObject.GetComponent<UIFont>();
				if (component != null)
				{
					return component;
				}
			}
		}
		return null;
	}

	public static void SetUseCacheWWW(bool bUseCache)
	{
		Config.mbUseCacheWWW = bUseCache;
	}

	public static bool GetUseCacheWWW()
	{
		return Config.mbUseCacheWWW;
	}

	public static void SetCacheVersion(string strCacheVersion)
	{
		string @string = PlayerPrefs.GetString("CacheVersion", string.Empty);
		if (string.IsNullOrEmpty(@string) || strCacheVersion != @string)
		{
			PlayerPrefs.SetString("CacheVersion", strCacheVersion);
			Caching.CleanCache();
		}
		Config.iCacheVersion = 0;
		if (string.IsNullOrEmpty(strCacheVersion))
		{
			LogSystem.LogWarning(new object[]
			{
				"Version Error ",
				strCacheVersion
			});
			return;
		}
		string text = strCacheVersion.Replace(".", string.Empty);
		if (string.IsNullOrEmpty(text))
		{
			LogSystem.LogWarning(new object[]
			{
				"Version Error ",
				text
			});
			return;
		}
		Config.iCacheVersion = int.Parse(text);
	}

	public static int GetQueueWaitTime()
	{
		return Config.mQueueWaitTime;
	}

	public static void SetResourceVersion(string strResourceVersion)
	{
		Config.ResourceVersion = strResourceVersion;
	}

	public static string GetMacAddress()
	{
		return Config.mstrMacAddress;
	}

	public static void SetMacAddress(string strMacAddress)
	{
		Config.mstrMacAddress = strMacAddress;
	}

	public static void SetGoogleId(string id)
	{
		Config.mstrGoogleId = id;
	}

	public static string GetPreSuffix()
	{
		return Config.mstrPreSuffix;
	}

	public static void SetPreSuffix(string strSuffix)
	{
		Config.mstrPreSuffix = strSuffix;
	}

	public static string GetStreamSuffix()
	{
		return Config.mstrStreamSuffix;
	}

	public static void SetStreamSuffix(string strSuffix)
	{
		Config.mstrStreamSuffix = strSuffix;
	}

	public static Config.GameAddress GetUpdaterAddress()
	{
		if (Config.mUpdaterAddress.Count == 0)
		{
			return null;
		}
		string key;
		if (string.IsNullOrEmpty(Config.strAdressId))
		{
			key = "QaAddr";
		}
		else
		{
			key = Config.strAdressId;
		}
		if (Config.mUpdaterAddress.ContainsKey(key))
		{
			return Config.mUpdaterAddress[key];
		}
		return null;
	}

	public static string GetUpdaterConfig(string strKey, string strName)
	{
		if (Config.mDictUpdaterGuide.ContainsKey(strKey) && Config.mDictUpdaterGuide[strKey].ContainsKey(strName))
		{
			return Config.mDictUpdaterGuide[strKey][strName];
		}
		return string.Empty;
	}

	public static bool GetOpenVideo()
	{
		return Config.IsOpenVideo == "1";
	}

	public static bool GetQRCodeLogin()
	{
		string updaterConfig = Config.GetUpdaterConfig("QRCodeLogin", "Value");
		return updaterConfig == "1";
	}

	public static string GetAssetBundleRootPath()
	{
		return Config.mstrAssetBundleRootPath;
	}

	public static void SetAssetBundleRootPath(string strRootPath)
	{
		Config.mstrAssetBundleRootPath = strRootPath;
	}

	public static string GetVersionUseage()
	{
		return Config.mstrVersionUseage;
	}

	public static void SetVersionUseage(string strValue)
	{
		Config.mstrVersionUseage = strValue;
	}

	public static void SetLaunchVersion(string version)
	{
		Config.launchVersion = version;
	}

	public static string GetInstallationVersion()
	{
		return Config.mstrInstallationVersion;
	}

	public static void SetInstallationVersion(string strValue)
	{
		Config.mstrInstallationVersion = strValue;
	}

	public static string GetLocalVersion()
	{
		return Config.mstrLocalVersion;
	}

	public static void SetLocalVersion(string strLocalVersion)
	{
		Config.mstrLocalVersion = strLocalVersion;
	}

	public static int GetLocalNumberVersion()
	{
		return Config.miLocalNumberVersion;
	}

	public static void SetLocalNumberVersion(string strLocalNumberVersion)
	{
		if (string.IsNullOrEmpty(strLocalNumberVersion))
		{
			Config.miLocalNumberVersion = 0;
		}
		else
		{
			try
			{
				Config.miLocalNumberVersion = int.Parse(strLocalNumberVersion);
			}
			catch (Exception ex)
			{
				Config.miLocalNumberVersion = 0;
				LogSystem.LogError(new object[]
				{
					ex.ToString()
				});
			}
		}
	}

	public static string GetAssetBundleStreamRootPath()
	{
		return Config.mstrAssetBundleStreamResRootPath;
	}

	public static string GetSourceRootPath()
	{
		return Config.mstrSourceResRootPath;
	}

	public static void SetSourceRootPath(string strRootPath)
	{
		Config.mstrSourceResRootPath = strRootPath;
	}

	public static string GetStreamRootPath()
	{
		return Config.mstrStreamResRootPath;
	}

	public static void SetStreamRootPath(string strRootPath)
	{
		Config.mstrStreamResRootPath = strRootPath;
	}

	public static void SetUpdaterAddress(TextAsset text)
	{
		if (text == null)
		{
			return;
		}
		XMLParser xMLParser = new XMLParser();
		XMLNode xMLNode = xMLParser.Parse(text.text);
		XMLNodeList nodeList = xMLNode.GetNodeList("Addresses>0>Address");
		if (nodeList != null)
		{
			foreach (XMLNode xMLNode2 in nodeList)
			{
				Config.GameAddress gameAddress = new Config.GameAddress();
				gameAddress.strID = xMLNode2.GetValue("@ID");
				gameAddress.strName = xMLNode2.GetValue("@Name");
				gameAddress.strDomainAddress = xMLNode2.GetValue("@DomainAddr") + "/" + Config.GetInstallationVersion();
				gameAddress.strIPAddress = xMLNode2.GetValue("@IPAddr") + "/" + Config.GetInstallationVersion();
				if (!Config.mUpdaterAddress.ContainsKey(gameAddress.strID))
				{
					Config.mUpdaterAddress.Add(gameAddress.strID, gameAddress);
				}
				else
				{
					LogSystem.LogError(new object[]
					{
						"login.xml contains : " + gameAddress.strID
					});
				}
			}
		}
	}

	public static void SetUpdateGuideInfo(string xmlString)
	{
		if (string.IsNullOrEmpty(xmlString))
		{
			return;
		}
		int startIndex = xmlString.IndexOf('<');
		xmlString = xmlString.Substring(startIndex);
		xmlString.Trim();
		XMLParser xMLParser = new XMLParser();
		XMLNode xMLNode = xMLParser.Parse(xmlString);
		XMLNodeList xMLNodeList = (XMLNodeList)xMLNode["Resources"];
		if (xMLNodeList == null)
		{
			return;
		}
		for (int i = 0; i < xMLNodeList.Count; i++)
		{
			XMLNode xMLNode2 = xMLNodeList[i] as XMLNode;
			XMLNodeList nodeList = xMLNode2.GetNodeList("Resource");
			if (nodeList != null)
			{
				for (int j = 0; j < nodeList.Count; j++)
				{
					XMLNode xMLNode3 = nodeList[j] as XMLNode;
					Dictionary<string, string> dictionary = new Dictionary<string, string>();
					string key = string.Empty;
					foreach (DictionaryEntry dictionaryEntry in xMLNode3)
					{
						if (dictionaryEntry.Value != null)
						{
							string text = dictionaryEntry.Key as string;
							if (text[0] == '@')
							{
								text = text.Substring(1);
								if (text == "ID")
								{
									key = (string)dictionaryEntry.Value;
								}
								else if (dictionary.ContainsKey(text))
								{
									dictionary[text] = (string)dictionaryEntry.Value;
								}
								else
								{
									dictionary.Add(text, (string)dictionaryEntry.Value);
								}
							}
						}
					}
					if (Config.mDictUpdaterGuide.ContainsKey(key))
					{
						Config.mDictUpdaterGuide[key] = dictionary;
					}
					else
					{
						Config.mDictUpdaterGuide.Add(key, dictionary);
					}
				}
			}
		}
	}

	public static void SetServerNotice(string xmlString)
	{
		Config.mstrServerNotice = xmlString;
	}

	public static int Compare_Index(Config.NoticeInfo aInfo, Config.NoticeInfo bInfo)
	{
		if (aInfo.iIndex > bInfo.iIndex)
		{
			return 1;
		}
		if (aInfo.iIndex < bInfo.iIndex)
		{
			return -1;
		}
		return 0;
	}

	public static void SetCustomInfoList(string xmlString)
	{
		int startIndex = xmlString.IndexOf('<');
		xmlString = xmlString.Substring(startIndex);
		xmlString.Trim();
		XMLParser xMLParser = new XMLParser();
		XMLNode xMLNode = xMLParser.Parse(xmlString);
		Config.bAppStore = (Config.channelName == Config.Appstore);
		Config.SetCustomInfo((XMLNodeList)xMLNode["Resources"]);
		Config.InitResouceInfo();
		Config.SetChannelInfo(xMLNode);
		Config.SetShareInfo(xMLNode);
		Config.SetRemoteLogInfo(xMLNode);
		Config.SetWechatInfo(xMLNode);
		Config.SetAccInfo(xMLNode);
	}

	private static void SetShareInfo(XMLNode rootnode)
	{
		if (rootnode == null)
		{
			return;
		}
		XMLNodeList nodeList = rootnode.GetNodeList("Resources>0>Share>0>Property");
		if (nodeList != null)
		{
			string shortLanguage = Language.GetShortLanguage(Language.LanguageVersion);
			if (shortLanguage != string.Empty)
			{
				if (Language.LanguageVersion == Language.LanguageType.Russia)
				{
					foreach (XMLNode xMLNode in nodeList)
					{
						string value = xMLNode.GetValue("@ID");
						if (value == shortLanguage)
						{
							Config.VK_Like_Url = xMLNode.GetValue("@LikeUrl");
							Config.VK_Share_strTitle = xMLNode.GetValue("@Title");
							Config.VK_Share_strContent = xMLNode.GetValue("@Content");
							Config.VK_Share_imageUrl = xMLNode.GetValue("@ImageUrl");
							Config.VK_Share_contentUrl = xMLNode.GetValue("@ContentUrl");
							break;
						}
					}
				}
				else
				{
					foreach (XMLNode xMLNode2 in nodeList)
					{
						string value2 = xMLNode2.GetValue("@ID");
						if (value2 == shortLanguage)
						{
							Config.FaceBook_Like_Url = xMLNode2.GetValue("@LikeUrl");
							Config.FaceBook_Share_strTitle = xMLNode2.GetValue("@Title");
							Config.FaceBook_Share_strContent = xMLNode2.GetValue("@Content");
							Config.FaceBook_Share_imageUrl = xMLNode2.GetValue("@ImageUrl");
							Config.FaceBook_Share_contentUrl = xMLNode2.GetValue("@ContentUrl");
							break;
						}
					}
				}
			}
		}
	}

	private static void SetWechatInfo(XMLNode rootnode)
	{
		if (rootnode == null)
		{
			return;
		}
		XMLNodeList nodeList = rootnode.GetNodeList("Resources>0>Wechat>0>Property");
		if (nodeList != null)
		{
			foreach (XMLNode xMLNode in nodeList)
			{
				string value = xMLNode.GetValue("@ID");
				if (value == Config.BundleIdentifier)
				{
					Config.bNeedWechatShare = "1".Equals(xMLNode.GetValue("@WechatShare"));
					Config.wxAndroidId = xMLNode.GetValue("@wxAndroidId");
					Config.WeChatShareImgUrl = xMLNode.GetValue("@WeChatShareImgUrl");
					break;
				}
			}
		}
	}

	private static void SetChannelInfo(XMLNode rootnode)
	{
		if (rootnode == null)
		{
			return;
		}
		XMLNodeList nodeList = rootnode.GetNodeList("Resources>0>Channel>0>Property");
		LogSystem.LogWarning(new object[]
		{
			"----------strChannelUniqueName--------" + Config.strChannelUniqueName
		});
		if (string.IsNullOrEmpty(Config.strChannelUniqueName))
		{
			Config.strChannelUniqueName = "android_snail";
		}
		if (nodeList != null)
		{
			foreach (XMLNode xMLNode in nodeList)
			{
				string value = xMLNode.GetValue("@ID");
				if (value == Config.strChannelUniqueName)
				{
					Config.payCallBackURL = xMLNode.GetValue("@PayCallBackUrl");
					Config.ClientInstallUrl = xMLNode.GetValue("@ClientInstallUrl");
					Config.strNoticeName = xMLNode.GetValue("@NoticeName");
					Config.strChannelName = xMLNode.GetValue("@ChannelName");
					Config.bNeedDataCollect = "1".Equals(xMLNode.GetValue("@DataCollect"));
					Config.operatorId = xMLNode.GetValue("@OperatorId");
					Config.operatorChannelId = xMLNode.GetValue("@ChannelId");
					string value2 = xMLNode.GetValue("@SwithAccount");
					if (!string.IsNullOrEmpty(value2))
					{
						Config.bCanSwithAccount = "1".Equals(xMLNode.GetValue("@SwithAccount"));
					}
					break;
				}
			}
		}
	}

	private static void SetRemoteLogInfo(XMLNode rootnode)
	{
		if (rootnode == null)
		{
			LogSystem.LogWarning(new object[]
			{
				"Config::SetRemoteLogInfo rootnode is null"
			});
			return;
		}
		XMLNode node = rootnode.GetNode("Resources>0>LogSocket>0");
		if (node != null)
		{
			int num = AssetFileUtils.IntParse(node.GetValue("@Status"), 0);
			LogSocket.SetRemoteLogStatus(num);
			if (num == 1)
			{
				XMLNodeList nodeList = rootnode.GetNodeList("Resources>0>LogSocket>0>Host");
				if (nodeList == null)
				{
					LogSystem.LogWarning(new object[]
					{
						"Config::SetRemoteLogInfo xmlNodeList is null"
					});
					return;
				}
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				foreach (XMLNode xMLNode in nodeList)
				{
					string value = xMLNode.GetValue("@Client");
					string value2 = xMLNode.GetValue("@Server");
					if (!dictionary.ContainsKey(value))
					{
						dictionary.Add(value, value2);
					}
				}
				LogSocket.AddConncetInfo(dictionary);
			}
		}
	}

	private static void SetAccInfo(XMLNode rootnode)
	{
		if (rootnode == null)
		{
			LogSystem.LogWarning(new object[]
			{
				"SetAccInfo is null"
			});
			return;
		}
		XMLNodeList nodeList = rootnode.GetNodeList("Resources>0>Acc>0>Property");
		if (nodeList == null)
		{
			LogSystem.LogWarning(new object[]
			{
				"SetAccInfo is null"
			});
			return;
		}
		foreach (XMLNode xMLNode in nodeList)
		{
			string value = xMLNode.GetValue("@No");
			if (Config.accInfo.ContainsKey(value))
			{
				Config.accInfo[value].Add(xMLNode.GetValue("@Name"));
			}
			else
			{
				List<string> list = new List<string>();
				list.Add(xMLNode.GetValue("@Name"));
				Config.accInfo.Add(value, list);
			}
		}
	}

	private static void SetCustomInfo(XMLNodeList xmlNodeList)
	{
		if (xmlNodeList == null)
		{
			LogSystem.LogWarning(new object[]
			{
				"SetCustomInfo is null"
			});
			return;
		}
		for (int i = 0; i < xmlNodeList.Count; i++)
		{
			XMLNode xMLNode = xmlNodeList[i] as XMLNode;
			XMLNodeList nodeList = xMLNode.GetNodeList("Resource");
			if (nodeList != null)
			{
				for (int j = 0; j < nodeList.Count; j++)
				{
					XMLNode xMLNode2 = nodeList[j] as XMLNode;
					Config.CustomInfo customInfo = new Config.CustomInfo();
					foreach (DictionaryEntry dictionaryEntry in xMLNode2)
					{
						if (dictionaryEntry.Value != null)
						{
							string text = dictionaryEntry.Key as string;
							if (text[0] == '@')
							{
								text = text.Substring(1);
								if (text == "ID")
								{
									customInfo.strCustomInfoID = (dictionaryEntry.Value as string);
								}
								else if (text == "Value")
								{
									customInfo.strCustomInfoVaule = (dictionaryEntry.Value as string);
								}
							}
						}
					}
					if (Config.mDictCustomInfoList.ContainsKey(customInfo.strCustomInfoID))
					{
						Config.mDictCustomInfoList[customInfo.strCustomInfoID] = customInfo;
					}
					else
					{
						Config.mDictCustomInfoList.Add(customInfo.strCustomInfoID, customInfo);
					}
				}
			}
		}
	}

	public static string GetCustomValue(string strKey)
	{
		Config.CustomInfo customInfo;
		if (Config.mDictCustomInfoList.TryGetValue(strKey, out customInfo))
		{
			return customInfo.strCustomInfoVaule;
		}
		return string.Empty;
	}

	public static void InitResouceInfo()
	{
		if (Config.mDictCustomInfoList.ContainsKey("accessID"))
		{
			Config.accessId = Config.mDictCustomInfoList["accessID"].strCustomInfoVaule;
		}
		if (Config.mDictCustomInfoList.ContainsKey("accessPwd"))
		{
			Config.accessPassword = Config.mDictCustomInfoList["accessPwd"].strCustomInfoVaule;
		}
		if (Config.mDictCustomInfoList.ContainsKey("seed"))
		{
			Config.seed = Config.mDictCustomInfoList["seed"].strCustomInfoVaule;
		}
		if (Config.mDictCustomInfoList.ContainsKey("ErrorCollectUrl"))
		{
			Config.errorCollectUrl = Config.mDictCustomInfoList["ErrorCollectUrl"].strCustomInfoVaule;
		}
		if (Config.mDictCustomInfoList.ContainsKey("CrashCollectUrl"))
		{
			Config.crashCollectUrl = Config.mDictCustomInfoList["CrashCollectUrl"].strCustomInfoVaule;
		}
		if (Config.mDictCustomInfoList.ContainsKey("U3DErrorCollectUrl"))
		{
			Config.u3dErrorCollectUrl = Config.mDictCustomInfoList["U3DErrorCollectUrl"].strCustomInfoVaule;
		}
		if (Config.mDictCustomInfoList.ContainsKey("DataCollectUrl"))
		{
			Config.dataCollectUrl = Config.mDictCustomInfoList["DataCollectUrl"].strCustomInfoVaule;
		}
		if (Config.mDictCustomInfoList.ContainsKey("ProjectName"))
		{
			Config.ProjectName = Config.mDictCustomInfoList["ProjectName"].strCustomInfoVaule;
		}
		if (Config.mDictCustomInfoList.ContainsKey("appid"))
		{
			Config.appid = Config.mDictCustomInfoList["appid"].strCustomInfoVaule;
		}
		if (Config.mDictCustomInfoList.ContainsKey("Weixin_Link_Url"))
		{
			Config.Weixin_Link_Url = Config.mDictCustomInfoList["Weixin_Link_Url"].strCustomInfoVaule;
		}
		if (Config.mDictCustomInfoList.ContainsKey("ACT_Url"))
		{
			Config.act_Url = Config.mDictCustomInfoList["ACT_Url"].strCustomInfoVaule;
		}
		if (Config.mDictCustomInfoList.ContainsKey("TwNotionUrl"))
		{
			Config.strTwNotionUrl = Config.mDictCustomInfoList["TwNotionUrl"].strCustomInfoVaule;
		}
		if (Config.mDictCustomInfoList.ContainsKey("KrNotionUrl1"))
		{
			Config.strKrNotionUrl1 = Config.mDictCustomInfoList["KrNotionUrl1"].strCustomInfoVaule;
		}
		if (Config.mDictCustomInfoList.ContainsKey("KrNotionUrl2"))
		{
			Config.strKrNotionUrl2 = Config.mDictCustomInfoList["KrNotionUrl2"].strCustomInfoVaule;
		}
		if (Config.mDictCustomInfoList.ContainsKey("StrTwShoubing"))
		{
			Config.strTwShoubing = Config.mDictCustomInfoList["StrTwShoubing"].strCustomInfoVaule;
		}
		if (Config.mDictCustomInfoList.ContainsKey("TwNotionConfig"))
		{
			Config.strTwNotionConfig = Config.mDictCustomInfoList["TwNotionConfig"].strCustomInfoVaule;
		}
		if (Language.LanguageVersion == Language.LanguageType.Russia)
		{
			if (Config.mDictCustomInfoList.ContainsKey("vk_share_Gift_1"))
			{
				Config.share_Gift_1 = Config.mDictCustomInfoList["vk_share_Gift_1"].strCustomInfoVaule;
			}
			if (Config.mDictCustomInfoList.ContainsKey("vk_share_Gift_2"))
			{
				Config.share_Gift_2 = Config.mDictCustomInfoList["vk_share_Gift_2"].strCustomInfoVaule;
			}
		}
		else
		{
			if (Config.mDictCustomInfoList.ContainsKey("fb_share_Gift_1"))
			{
				Config.share_Gift_1 = Config.mDictCustomInfoList["fb_share_Gift_1"].strCustomInfoVaule;
			}
			if (Config.mDictCustomInfoList.ContainsKey("fb_share_Gift_2"))
			{
				Config.share_Gift_2 = Config.mDictCustomInfoList["fb_share_Gift_2"].strCustomInfoVaule;
			}
		}
		if (Config.mDictCustomInfoList.ContainsKey("appkey"))
		{
			Config.appkey = Config.mDictCustomInfoList["appkey"].strCustomInfoVaule;
		}
		if (Config.mDictCustomInfoList.ContainsKey("wxAndroidId"))
		{
			Config.wxAndroidId = Config.mDictCustomInfoList["wxAndroidId"].strCustomInfoVaule;
		}
		if (Config.mDictCustomInfoList.ContainsKey("wxiOSId"))
		{
			Config.wxiOSId = Config.mDictCustomInfoList["wxiOSId"].strCustomInfoVaule;
		}
		if (Config.mDictCustomInfoList.ContainsKey("dataCollectionUrl"))
		{
			Config.dataCollectionUrl = Config.mDictCustomInfoList["dataCollectionUrl"].strCustomInfoVaule;
		}
		if (Config.mDictCustomInfoList.ContainsKey("ErrorLogPhoneOpen"))
		{
			string strCustomInfoVaule = Config.mDictCustomInfoList["ErrorLogPhoneOpen"].strCustomInfoVaule;
			if (strCustomInfoVaule == "true")
			{
				Config.ErrorLogPhoneOpen = true;
			}
			else
			{
				Config.ErrorLogPhoneOpen = false;
			}
		}
		if (Config.mDictCustomInfoList.ContainsKey("MonthCardOpne"))
		{
			string strCustomInfoVaule2 = Config.mDictCustomInfoList["MonthCardOpne"].strCustomInfoVaule;
			if (strCustomInfoVaule2 == "true")
			{
				Config.MonthCardOpne = true;
			}
			else
			{
				Config.MonthCardOpne = false;
			}
		}
		if (Config.mDictCustomInfoList.ContainsKey("floatMenuShow"))
		{
			string strCustomInfoVaule3 = Config.mDictCustomInfoList["floatMenuShow"].strCustomInfoVaule;
			if (strCustomInfoVaule3 == "true")
			{
				Config.floatMenuShow = 0;
			}
			else
			{
				Config.floatMenuShow = 1;
			}
		}
		if (Config.mDictCustomInfoList.ContainsKey("EventPreventWallow"))
		{
			Config.EventPreventWallowUrl = Config.mDictCustomInfoList["EventPreventWallow"].strCustomInfoVaule;
		}
		if (Config.mDictCustomInfoList.ContainsKey("regUrl"))
		{
			Config.registerServerUrl = Config.mDictCustomInfoList["regUrl"].strCustomInfoVaule;
		}
		if (Config.mDictCustomInfoList.ContainsKey("FrameAddress"))
		{
			Config.FrameAddress = Config.mDictCustomInfoList["FrameAddress"].strCustomInfoVaule;
		}
		if (Config.mDictCustomInfoList.ContainsKey("accessType"))
		{
			Config.accessType = Config.mDictCustomInfoList["accessType"].strCustomInfoVaule;
		}
		if (Config.mDictCustomInfoList.ContainsKey("gameID"))
		{
			Config.gameId = Config.mDictCustomInfoList["gameID"].strCustomInfoVaule;
		}
		if (Config.mDictCustomInfoList.ContainsKey("TwFBImgUrl"))
		{
			Config.strTwFBImgUrl = Config.mDictCustomInfoList["TwFBImgUrl"].strCustomInfoVaule;
		}
		if (Config.mDictCustomInfoList.ContainsKey("TwFBAndroidApkUrl"))
		{
			Config.strTwFBAndroidApkUrl = Config.mDictCustomInfoList["TwFBAndroidApkUrl"].strCustomInfoVaule;
		}
		if (Config.mDictCustomInfoList.ContainsKey("FPSOpen"))
		{
			Config.isOpenFPS = Config.mDictCustomInfoList["FPSOpen"].strCustomInfoVaule;
		}
		if (Config.mDictCustomInfoList.ContainsKey("FPSOpenFile"))
		{
			Config.isOpenFPSFile = Config.mDictCustomInfoList["FPSOpenFile"].strCustomInfoVaule;
		}
		if (Config.mDictCustomInfoList.ContainsKey("PKLimitOpen"))
		{
			Config.isOpenPkLimit = Config.mDictCustomInfoList["PKLimitOpen"].strCustomInfoVaule;
		}
		if (Config.mDictCustomInfoList.ContainsKey("GMOpen"))
		{
			Config.IsGMOpen = Config.mDictCustomInfoList["GMOpen"].strCustomInfoVaule;
		}
		if (Config.mDictCustomInfoList.ContainsKey("ScanCodeOpen"))
		{
			Config.IsScanCodeOpen = Config.mDictCustomInfoList["ScanCodeOpen"].strCustomInfoVaule.Equals("1");
		}
		if (Config.mDictCustomInfoList.ContainsKey("specialGem"))
		{
			Config.IsSpecialGemOpen = Config.mDictCustomInfoList["specialGem"].strCustomInfoVaule;
		}
		if (Config.mDictCustomInfoList.ContainsKey("OpenVideo"))
		{
			Config.IsOpenVideo = Config.mDictCustomInfoList["OpenVideo"].strCustomInfoVaule;
		}
		if (Config.mDictCustomInfoList.ContainsKey("channeltype"))
		{
			Config.channelId = Config.mDictCustomInfoList["channeltype"].strCustomInfoVaule;
		}
		if (Config.mDictCustomInfoList.ContainsKey("PushAndroidName"))
		{
			Config.pushPhoneTypeName = Config.mDictCustomInfoList["PushAndroidName"].strCustomInfoVaule;
		}
		if (Config.mDictCustomInfoList.ContainsKey("version"))
		{
			Config.version = Config.mDictCustomInfoList["version"].strCustomInfoVaule;
		}
		if (Config.mDictCustomInfoList.ContainsKey("pushServiceIP"))
		{
			Config.pushServiceIP = Config.mDictCustomInfoList["pushServiceIP"].strCustomInfoVaule;
		}
		if (Config.mDictCustomInfoList.ContainsKey("pushServicePOST"))
		{
			Config.pushServicePOST = Config.mDictCustomInfoList["pushServicePOST"].strCustomInfoVaule;
		}
		if (Config.mDictCustomInfoList.ContainsKey("channelAppSecret"))
		{
			Config.channelAppSecret = Config.mDictCustomInfoList["channelAppSecret"].strCustomInfoVaule;
		}
		if (Config.mDictCustomInfoList.ContainsKey("TwGoogleAccount"))
		{
			Config.strGoogleAccount = Config.mDictCustomInfoList["TwGoogleAccount"].strCustomInfoVaule;
		}
		if (Config.mDictCustomInfoList.ContainsKey("Quality_SuperLow"))
		{
			Config.qualityHandle("Quality_SuperLow");
		}
		if (Config.mDictCustomInfoList.ContainsKey("Quality_Low"))
		{
			Config.qualityHandle("Quality_Low");
		}
		if (Config.mDictCustomInfoList.ContainsKey("Quality_Middle"))
		{
			Config.qualityHandle("Quality_Middle");
		}
		if (Config.mDictCustomInfoList.ContainsKey("Quality_High"))
		{
			Config.qualityHandle("Quality_High");
		}
		if (Config.mDictCustomInfoList.ContainsKey("Pc_RechargeUrl"))
		{
			Config.Pc_RechargeUrl = Config.mDictCustomInfoList["Pc_RechargeUrl"].strCustomInfoVaule;
		}
		if (Config.mDictCustomInfoList.ContainsKey("Pc_BackendUrl"))
		{
			Config.Pc_BackendUrl = Config.mDictCustomInfoList["Pc_BackendUrl"].strCustomInfoVaule;
		}
		if (Config.mDictCustomInfoList.ContainsKey("Pc_FontendUrl"))
		{
			Config.Pc_FontendUrl = Config.mDictCustomInfoList["Pc_FontendUrl"].strCustomInfoVaule;
		}
		if (Config.mDictCustomInfoList.ContainsKey("Pc_SignKey"))
		{
			string strCustomInfoVaule4 = Config.mDictCustomInfoList["Pc_SignKey"].strCustomInfoVaule;
			string[] array = strCustomInfoVaule4.Split(new char[]
			{
				','
			});
			if (array.Length == 2)
			{
				Config.Pc_DESCryptoKey = array[0];
				Config.Pc_DESCryptoIV = array[1];
			}
		}
		if (Config.mDictCustomInfoList.ContainsKey("Pc_Merchantid"))
		{
			Config.Pc_Merchantid = Config.mDictCustomInfoList["Pc_Merchantid"].strCustomInfoVaule;
		}
		if (Config.mDictCustomInfoList.ContainsKey("Pc_UrlTC"))
		{
			Config.Pc_UrlTC = Config.mDictCustomInfoList["Pc_UrlTC"].strCustomInfoVaule;
		}
		if (Config.mDictCustomInfoList.ContainsKey("Pc_TraceLog"))
		{
			string strCustomInfoVaule5 = Config.mDictCustomInfoList["Pc_TraceLog"].strCustomInfoVaule;
			Config.Pc_TraceLog = (strCustomInfoVaule5 == "1");
		}
	}

	private static void qualityHandle(string id)
	{
		string strCustomInfoVaule = Config.mDictCustomInfoList[id].strCustomInfoVaule;
		if (string.IsNullOrEmpty(strCustomInfoVaule))
		{
			return;
		}
		int qualityNum = Config.getQualityNum(id);
		if (qualityNum == -1)
		{
			return;
		}
		string[] array = strCustomInfoVaule.Split(new char[]
		{
			','
		});
		if (array == null || array.Length == 0)
		{
			return;
		}
		List<string> list = new List<string>();
		for (int i = 0; i < array.Length; i++)
		{
			list.Add(array[i]);
		}
		if (Config.mDeviceQualityConfig.ContainsKey(qualityNum))
		{
			Config.mDeviceQualityConfig[qualityNum] = list;
		}
		else
		{
			Config.mDeviceQualityConfig.Add(qualityNum, list);
		}
	}

	private static int getQualityNum(string quality)
	{
		if (quality.Equals("Quality_SuperLow"))
		{
			return 0;
		}
		if (quality.Equals("Quality_Low"))
		{
			return 1;
		}
		if (quality.Equals("Quality_Middle"))
		{
			return 2;
		}
		if (quality.Equals("Quality_High"))
		{
			return 3;
		}
		return -1;
	}

	public static void SetServerList(string xmlString)
	{
		int startIndex = xmlString.IndexOf('<');
		xmlString = xmlString.Substring(startIndex);
		xmlString.Trim();
		XMLParser xMLParser = new XMLParser();
		XMLNode xMLNode = xMLParser.Parse(xmlString);
		XMLNodeList xMLNodeList = (XMLNodeList)xMLNode["Servers"];
		if (xMLNodeList == null)
		{
			return;
		}
		for (int i = 0; i < xMLNodeList.Count; i++)
		{
			XMLNode xMLNode2 = xMLNodeList[i] as XMLNode;
			XMLNodeList nodeList = xMLNode2.GetNodeList("Server");
			if (nodeList != null)
			{
				for (int j = 0; j < nodeList.Count; j++)
				{
					XMLNode xMLNode3 = nodeList[j] as XMLNode;
					Dictionary<string, string> dictionary = new Dictionary<string, string>();
					foreach (DictionaryEntry dictionaryEntry in xMLNode3)
					{
						if (dictionaryEntry.Value != null)
						{
							string text = dictionaryEntry.Key as string;
							if (text[0] == '@')
							{
								text = text.Substring(1);
								if (!dictionary.ContainsKey(text))
								{
									dictionary.Add(text, (string)dictionaryEntry.Value);
								}
								else
								{
									dictionary[text] = (string)dictionaryEntry.Value;
								}
							}
						}
					}
					Config.mDictServerList.Add(dictionary);
				}
			}
		}
	}

	public static void SetAllServerList(string xmlString)
	{
		int startIndex = xmlString.IndexOf('<');
		xmlString = xmlString.Substring(startIndex);
		xmlString.Trim();
		XMLParser xMLParser = new XMLParser();
		XMLNode xMLNode = xMLParser.Parse(xmlString);
		XMLNodeList xMLNodeList = (XMLNodeList)xMLNode["Servers"];
		if (xMLNodeList == null)
		{
			return;
		}
		for (int i = 0; i < xMLNodeList.Count; i++)
		{
			XMLNode xMLNode2 = xMLNodeList[i] as XMLNode;
			XMLNodeList nodeList = xMLNode2.GetNodeList("Server");
			if (nodeList != null)
			{
				for (int j = 0; j < nodeList.Count; j++)
				{
					XMLNode xMLNode3 = nodeList[j] as XMLNode;
					Dictionary<string, string> dictionary = new Dictionary<string, string>();
					foreach (DictionaryEntry dictionaryEntry in xMLNode3)
					{
						if (dictionaryEntry.Value != null)
						{
							string text = dictionaryEntry.Key as string;
							if (text[0] == '@')
							{
								text = text.Substring(1);
								if (!dictionary.ContainsKey(text))
								{
									dictionary.Add(text, (string)dictionaryEntry.Value);
								}
								else
								{
									dictionary[text] = (string)dictionaryEntry.Value;
								}
							}
						}
					}
					Config.mDictAllServerList.Add(dictionary);
				}
			}
		}
	}

	public static void SetShopList(string xmlString)
	{
		Config.mstrShopData = xmlString;
	}

	public static void SetPushServerList(string xmlString)
	{
		int startIndex = xmlString.IndexOf('<');
		xmlString = xmlString.Substring(startIndex);
		xmlString.Trim();
		XMLParser xMLParser = new XMLParser();
		XMLNode xMLNode = xMLParser.Parse(xmlString);
		XMLNodeList xMLNodeList = (XMLNodeList)xMLNode["PushServers"];
		if (xMLNodeList == null)
		{
			return;
		}
		for (int i = 0; i < xMLNodeList.Count; i++)
		{
			XMLNode xMLNode2 = xMLNodeList[i] as XMLNode;
			XMLNodeList nodeList = xMLNode2.GetNodeList("Server");
			if (nodeList != null)
			{
				for (int j = 0; j < nodeList.Count; j++)
				{
					XMLNode xMLNode3 = nodeList[j] as XMLNode;
					Config.PushServerInfo pushServerInfo = new Config.PushServerInfo();
					foreach (DictionaryEntry dictionaryEntry in xMLNode3)
					{
						if (dictionaryEntry.Value != null)
						{
							string text = dictionaryEntry.Key as string;
							if (text[0] == '@')
							{
								text = text.Substring(1);
								if (text == "ID")
								{
									pushServerInfo.strServerID = (dictionaryEntry.Value as string);
								}
								else if (text == "Name")
								{
									pushServerInfo.strServerName = (dictionaryEntry.Value as string);
								}
								else if (text == "IP")
								{
									pushServerInfo.strServerIP = (dictionaryEntry.Value as string);
								}
								else if (text == "Port")
								{
									pushServerInfo.strServerPort = (dictionaryEntry.Value as string);
								}
								else if (text == "ApiKey")
								{
									pushServerInfo.strApiKey = (dictionaryEntry.Value as string);
								}
							}
						}
					}
					Config.mDictPushServerList.Add(pushServerInfo);
				}
			}
		}
	}

	public static Config.PushServerInfo GetPushServer()
	{
		if (Config.mDictPushServerList.Count == 0)
		{
			return null;
		}
		int index = UnityEngine.Random.Range(0, Config.mDictPushServerList.Count);
		return Config.mDictPushServerList[index];
	}

	public static void SetWordsInfo(string strWords)
	{
		if (string.IsNullOrEmpty(strWords))
		{
			return;
		}
		XMLParser xMLParser = new XMLParser();
		XMLNode xMLNode = xMLParser.Parse(strWords);
		XMLNodeList nodeList = xMLNode.GetNodeList("Resources>0>" + Language.LanguageVersion.ToString() + ">0>Text");
		if (nodeList != null)
		{
			foreach (XMLNode xMLNode2 in nodeList)
			{
				string value = xMLNode2.GetValue("@Key");
				string value2 = xMLNode2.GetValue("@Value");
				if (Config.mWordsDict.ContainsKey(value))
				{
					Config.mWordsDict[value] = value2;
				}
				else
				{
					Config.mWordsDict.Add(value, value2);
				}
			}
		}
	}

	public static void SetAssetBundleStreamRootPath(string strRootPath)
	{
		Config.mstrAssetBundleStreamResRootPath = strRootPath;
	}

	public static string GetUdpateLangage(string strKey)
	{
		if (Config.mWordsDict.ContainsKey(strKey))
		{
			return Config.mWordsDict[strKey];
		}
		return strKey;
	}

	public static void SavePlayerPrefs(string useraccount, string username, string serverid, string gameid, string appstoreUrl, string accountid)
	{
		PlayerPrefs.SetString("useraccount", useraccount);
		string value = WWW.EscapeURL(username);
		PlayerPrefs.SetString("user_name", value);
		PlayerPrefs.SetString("serverid", serverid);
		PlayerPrefs.SetString("game_id", gameid);
		PlayerPrefs.SetString("appstore_url", appstoreUrl);
		PlayerPrefs.SetString("accountid", accountid);
	}

	public static void GetPlayerPrefs(ref string useraccount, ref string username, ref string serverid, ref string gameid, ref string appstoreUrl, ref string accountid)
	{
		useraccount = PlayerPrefs.GetString("useraccount", string.Empty);
		string @string = PlayerPrefs.GetString("user_name", string.Empty);
		username = WWW.UnEscapeURL(@string);
		serverid = PlayerPrefs.GetString("serverid", string.Empty);
		gameid = PlayerPrefs.GetString("game_id", string.Empty);
		appstoreUrl = PlayerPrefs.GetString("appstore_url", string.Empty);
		accountid = PlayerPrefs.GetString("accountid", string.Empty);
	}

	public static string GetNetErrorPromp(string strKeywords)
	{
		string strKey = "CustomInfoError";
		if (strKeywords.Contains("connect to host"))
		{
			strKey = "CantConnectToHost";
		}
		else if (strKeywords.Contains("404"))
		{
			strKey = "HTTPError_404";
		}
		else if (strKeywords.Contains("403"))
		{
			strKey = "HTTPError_403";
		}
		else if (strKeywords.Contains("405"))
		{
			strKey = "HTTPError_405";
		}
		else if (strKeywords.Contains("406"))
		{
			strKey = "HTTPError_406";
		}
		else if (strKeywords.Contains("407"))
		{
			strKey = "HTTPError_407";
		}
		else if (strKeywords.Contains("410"))
		{
			strKey = "HTTPError_410";
		}
		else if (strKeywords.Contains("412"))
		{
			strKey = "HTTPError_412";
		}
		else if (strKeywords.Contains("414"))
		{
			strKey = "HTTPError_414";
		}
		else if (strKeywords.Contains("500"))
		{
			strKey = "HTTPError_500";
		}
		else if (strKeywords.Contains("501"))
		{
			strKey = "HTTPError_501";
		}
		else if (strKeywords.Contains("502"))
		{
			strKey = "HTTPError_502";
		}
		return Config.GetUdpateLangage(strKey);
	}

	public static void InitMenuShow(string serverId)
	{
		if (!Config.isInitMenu)
		{
			Config.isInitMenu = true;
		}
	}

	public static bool IsOpenPkLimit()
	{
		return Config.isOpenPkLimit == "1";
	}

	public static void SetLoadAssetCall(Config.CallLoadAsset call)
	{
		Config.monLoadAsset = call;
	}

	public static UIAtlas GetAtlas(string strAtlasName)
	{
		if (Config.mAtlasTable.ContainsKey(strAtlasName))
		{
			return Config.mAtlasTable[strAtlasName];
		}
		return null;
	}

	private static UIAtlas LoadUIAtlas(string strAtlasName)
	{
		if (Application.isPlaying)
		{
			string path = "Prefabs/UIAtlas/" + Language.LanguageVersion.ToString() + "/" + strAtlasName;
			UnityEngine.Object @object = Resources.Load(path);
			if (@object != null)
			{
				GameObject gameObject = @object as GameObject;
				UIAtlas component = gameObject.GetComponent<UIAtlas>();
				if (component != null)
				{
					return component;
				}
			}
		}
		return null;
	}

	public static UIFont GetFont(string strFontName)
	{
		if (Config.mFontsTable.ContainsKey(strFontName))
		{
			return Config.mFontsTable[strFontName];
		}
		return null;
	}
}
