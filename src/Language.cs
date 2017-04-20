using System;
using System.Collections.Generic;
using UnityEngine;

public class Language
{
	public class LangVo
	{
		public Language.LanguageType language;

		public string name;

		public string code;
	}

	public enum LanguageType
	{
		None,
		China,
		Russia,
		NorthAmerica,
		Thailand,
		Taiwan,
		Korea,
		Japan,
		Indonesia
	}

	private const float ORIGIN_UI_HEIGHT = 720f;

	private const float ORIGIN_UI_WIDTH = 1280f;

	public static string LANGUAGECACHE = "LanguageCache";

	private static GameObject uiSelectBox;

	private static UIGrid grid;

	private static UIGridItem lastGridItem;

	private static int lastGridItemIndex;

	private static Action mCallFunc;

	private static string strSelectLangTitle;

	private static string strSelectLangButton;

	private static Language.LanguageType curLanguageType;

	public static Language.LanguageType LanguageVersion
	{
		get;
		private set;
	}

	public static bool IS_Voerseas
	{
		get;
		private set;
	}

	public static string GetShortLanguage(Language.LanguageType type)
	{
		switch (type)
		{
		case Language.LanguageType.China:
			return "cn";
		case Language.LanguageType.Russia:
			return "ru";
		case Language.LanguageType.Thailand:
			return "th";
		case Language.LanguageType.Taiwan:
			return "tw";
		case Language.LanguageType.Korea:
			return "ko";
		case Language.LanguageType.Japan:
			return "ja";
		case Language.LanguageType.Indonesia:
			return "id";
		}
		return "en";
	}

	public static void Init(Action callFunction)
	{
		Language.LanguageVersion = Language.LanguageType.China;
		Language.IS_Voerseas = false;
		Language.InitWordInfos();
		callFunction();
		LogSystem.LogWarning(new object[]
		{
			"current language version : ",
			Language.LanguageVersion.ToString()
		});
	}

	private static void InitOnePackageLang(Action callFunction)
	{
		Language.mCallFunc = callFunction;
		List<object> languages = Language.GetLanguages();
		Language.LanguageType languageType;
		if (PlayerPrefs.GetInt(Language.LANGUAGECACHE) == 0)
		{
			string code = DelegateProxy.U3DReturnAndroid();
			languageType = Language.GetLanguageTypeByCode(ref languages, code);
			LogSystem.LogWarning(new object[]
			{
				"SnailPluginsUtils.getInstance.U3DCallSystemLanguage : " + languageType
			});
		}
		languageType = Language.LanguageType.Taiwan;
		Language.SettingLanguage(languageType);
		Language.InitWordInfos();
	}

	private static Language.LanguageType GetLanguageTypeByCode(ref List<object> list, string code)
	{
		for (int i = 0; i < list.Count; i++)
		{
			Language.LangVo langVo = list[i] as Language.LangVo;
			if (langVo != null && code == langVo.code)
			{
				return langVo.language;
			}
		}
		return Language.LanguageType.None;
	}

	private static void InitWordInfos()
	{
		TextAsset textAsset = Resources.Load("Local/Config/Language") as TextAsset;
		if (textAsset != null)
		{
			Config.SetWordsInfo(textAsset.text);
			Resources.UnloadAsset(textAsset);
		}
		if (Language.mCallFunc != null)
		{
			Language.mCallFunc();
		}
	}

	private static void InitCamera()
	{
		GameObject gameObject = GameObject.FindWithTag("UICamera");
		if (gameObject == null)
		{
			return;
		}
		if (gameObject.camera == null)
		{
			LogSystem.LogWarning(new object[]
			{
				"UICamera is not find"
			});
			return;
		}
		if (gameObject.camera.gameObject.GetComponent<UICamera>() == null)
		{
			gameObject.camera.gameObject.AddComponent<UICamera>();
		}
		if ((float)ResolutionConstrain.Instance.width * 1f / (float)ResolutionConstrain.Instance.height >= 1.77777779f)
		{
			gameObject.camera.orthographicSize = 720f / (float)ResolutionConstrain.Instance.height;
		}
		else
		{
			gameObject.camera.orthographicSize = 1280f / (float)ResolutionConstrain.Instance.width;
		}
	}

	private static void ShowSelectLanguage()
	{
		Language.InitCamera();
		if (Language.uiSelectBox == null)
		{
			string path = string.Format("Local/Prefabs/UI/{0}/SelectLanguage", Language.LanguageVersion.ToString());
			GameObject original = Resources.Load(path) as GameObject;
			GameObject gameObject = GameObject.FindWithTag("UICamera");
			Language.uiSelectBox = (UnityEngine.Object.Instantiate(original, Vector3.zero, Quaternion.identity) as GameObject);
			Language.uiSelectBox.transform.parent = gameObject.camera.transform;
			Language.uiSelectBox.transform.localPosition = Vector3.zero;
			Language.uiSelectBox.transform.localScale = Vector3.one;
			Language.uiSelectBox.SetActive(true);
		}
		if (Language.uiSelectBox != null)
		{
			Transform transform = Language.uiSelectBox.transform.FindChild("title");
			if (transform != null)
			{
				UILabel component = transform.GetComponent<UILabel>();
				component.text = Language.strSelectLangTitle;
			}
			Transform transform2 = Language.uiSelectBox.transform.FindChild("scrollView/grid");
			if (transform2 != null)
			{
				Language.grid = transform2.GetComponent<UIGrid>();
				Language.grid.SelectItem = new UIGrid.OnSelectEvent(Language.OnGridSelected);
				Language.grid.BindCustomCallBack(new UIGrid.OnUpdateDataRow(Language.OnLanguageChange));
				Language.grid.StartCustom();
				Language.grid.AddCustomDataList(Language.GetLanguages());
				Language.grid.SetSelect(0);
			}
			Transform transform3 = Language.uiSelectBox.transform.FindChild("btnSure");
			if (transform3 != null)
			{
				UIEventListener.Get(transform3.gameObject).onClick = new UIEventListener.VoidDelegate(Language.OnButtonSureClick);
			}
			Transform transform4 = transform3.transform.FindChild("Label");
			if (transform4 != null)
			{
				UILabel component2 = transform4.GetComponent<UILabel>();
				component2.text = Language.strSelectLangButton;
			}
		}
	}

	private static void OnGridSelected(int iSelect, bool active, bool select)
	{
		if (Language.lastGridItem != null)
		{
			Language.SetToogleState(Language.lastGridItem, false);
		}
		Language.lastGridItemIndex = iSelect;
		Language.lastGridItem = Language.grid.GetSelectedGridItem();
		Language.SetToogleState(Language.lastGridItem, true);
		Language.LangVo langVo = Language.lastGridItem.oData as Language.LangVo;
		if (langVo != null)
		{
			Language.curLanguageType = langVo.language;
		}
	}

	private static void SetToogleState(UIGridItem griditem, bool value)
	{
		UIToggle uIToggle = griditem.mScripts[0] as UIToggle;
		if (uIToggle != null)
		{
			uIToggle.value = value;
		}
	}

	public static List<object> GetLanguages()
	{
		List<object> list = new List<object>();
		TextAsset textAsset = Resources.Load("Local/Config/SelectLanguage") as TextAsset;
		if (textAsset != null)
		{
			string text = textAsset.text;
			if (string.IsNullOrEmpty(text))
			{
				return list;
			}
			XMLParser xMLParser = new XMLParser();
			XMLNode xMLNode = xMLParser.Parse(text);
			XMLNodeList nodeList = xMLNode.GetNodeList("Language>0>Item");
			XMLNode node = xMLNode.GetNode("Language>0>UI>0");
			Language.strSelectLangTitle = node.GetValue("@Title");
			Language.strSelectLangButton = node.GetValue("@Button");
			if (nodeList != null)
			{
				foreach (XMLNode xMLNode2 in nodeList)
				{
					Language.LangVo langVo = new Language.LangVo();
					langVo.language = (Language.LanguageType)((int)Enum.Parse(typeof(Language.LanguageType), xMLNode2.GetValue("@Type")));
					langVo.name = xMLNode2.GetValue("@Name");
					langVo.code = xMLNode2.GetValue("@Code");
					if (!list.Contains(langVo))
					{
						list.Add(langVo);
					}
				}
			}
		}
		Resources.UnloadAsset(textAsset);
		return list;
	}

	private static void OnButtonSureClick(GameObject go)
	{
		Language.SettingLanguage(Language.curLanguageType);
		Language.InitWordInfos();
	}

	private static void OnLanguageChange(UIGridItem item)
	{
		if (item == null && item.oData != null)
		{
			return;
		}
		Language.LangVo langVo = item.oData as Language.LangVo;
		int num = 0;
		UIToggle uIToggle = item.mScripts[num++] as UIToggle;
		UILabel uILabel = item.mScripts[num++] as UILabel;
		uIToggle.optionCanBeNone = true;
		if (item.GetIndex() == Language.lastGridItemIndex)
		{
			uIToggle.value = true;
		}
		else
		{
			uIToggle.value = false;
		}
		if (langVo != null)
		{
			uILabel.text = langVo.language.ToString();
		}
	}

	public static void Destroy()
	{
		if (Language.uiSelectBox != null)
		{
			UnityEngine.Object.Destroy(Language.uiSelectBox);
			Language.uiSelectBox = null;
		}
	}

	public static void SettingLanguage(Language.LanguageType type)
	{
		Language.LanguageVersion = type;
		PlayerPrefs.SetInt(Language.LANGUAGECACHE, (int)Language.LanguageVersion);
		LogSystem.LogWarning(new object[]
		{
			"Setting Language : ",
			PlayerPrefs.GetInt(Language.LANGUAGECACHE)
		});
	}
}
