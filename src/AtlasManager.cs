using System;
using System.Collections.Generic;
using UnityEngine;

public class AtlasManager
{
	private class AtlasBase
	{
		private List<GameObject> UseObjects = new List<GameObject>();

		public int Length
		{
			get
			{
				return this.UseObjects.Count;
			}
		}

		public int IndexOf(GameObject gameObject)
		{
			return this.UseObjects.IndexOf(gameObject);
		}

		public void Add(GameObject gameObject)
		{
			this.UseObjects.Add(gameObject);
		}

		public void Remove(GameObject gameObject)
		{
			this.UseObjects.Remove(gameObject);
		}

		public GameObject Get(int index)
		{
			return this.UseObjects[index];
		}

		public virtual void Destroy()
		{
			this.UseObjects.Clear();
		}
	}

	private class AtlasUI : AtlasManager.AtlasBase
	{
		public UIAtlas UseAtlas
		{
			get;
			set;
		}

		public string AtlasName
		{
			get;
			set;
		}

		public override void Destroy()
		{
			base.Destroy();
			this.UseAtlas = null;
		}
	}

	private class AtlasFont : AtlasManager.AtlasBase
	{
		public UIFont UseFont
		{
			get;
			set;
		}

		public string FontName
		{
			get;
			set;
		}

		public override void Destroy()
		{
			base.Destroy();
			this.UseFont = null;
		}
	}

	private static string strAtlasPath = "Prefabs/UIAtlas/";

	private static string strPathSplit = "/";

	private static string strFontPrefix = "Prefabs/UIAtlas/";

	private static string strFontPrefix2 = "Fonts/";

	private static string strFontSplit = "/";

	private static Dictionary<string, AtlasManager.AtlasBase> mDictionary = new Dictionary<string, AtlasManager.AtlasBase>();

	public static int Count
	{
		get
		{
			return AtlasManager.mDictionary.Count;
		}
	}

	public static void GetFont(string fontName, GameObject gameObject, AssetCallBack callBack)
	{
		if (AtlasManager.mDictionary.ContainsKey(fontName))
		{
			AtlasManager.AtlasFont atlasFont = AtlasManager.mDictionary[fontName] as AtlasManager.AtlasFont;
			int num = atlasFont.IndexOf(gameObject);
			if (num > -1)
			{
				LogSystem.LogWarning(new object[]
				{
					"AtlasManager::GetFont gameObject = ",
					gameObject,
					" FontName = ",
					fontName
				});
			}
			else
			{
				atlasFont.Add(gameObject);
			}
			if (atlasFont.UseFont != null)
			{
				VarStore varStore = VarStore.CreateVarStore();
				varStore.PushParams(atlasFont.UseFont);
				callBack(varStore);
			}
			else
			{
				AtlasManager.LoadFont(fontName, callBack);
				LogSystem.LogWarning(new object[]
				{
					"AtlasManager::GetFont atlas.UseFont is null"
				});
			}
		}
		else
		{
			AtlasManager.CreateFontPool(fontName, gameObject);
			AtlasManager.LoadFont(fontName, callBack);
		}
	}

	private static void LoadFont(string fontName, AssetCallBack callBack)
	{
		if (Application.isPlaying)
		{
			string text = AtlasManager.strFontPrefix + Language.LanguageVersion.ToString() + AtlasManager.strFontSplit + fontName;
			if (fontName.Contains("NGUIFont"))
			{
				text = AtlasManager.strFontPrefix2 + Language.LanguageVersion.ToString() + AtlasManager.strFontSplit + fontName;
			}
			if (!DelegateProxy.LoadAsset(text, delegate(VarStore args)
			{
				AtlasManager.OnAtlasFontLoaded(fontName, callBack, AtlasManager.GetComFont(args[0] as UnityEngine.Object));
				args.Collect();
			}, false))
			{
				UnityEngine.Object obj = Resources.Load(text);
				AtlasManager.OnAtlasFontLoaded(fontName, callBack, AtlasManager.GetComFont(obj));
			}
		}
	}

	private static void CreateFontPool(string atlasName, GameObject gameObject)
	{
		if (!AtlasManager.mDictionary.ContainsKey(atlasName))
		{
			AtlasManager.AtlasFont atlasFont = new AtlasManager.AtlasFont();
			atlasFont.FontName = atlasName;
			atlasFont.Add(gameObject);
			AtlasManager.mDictionary.Add(atlasName, atlasFont);
		}
	}

	private static void OnAtlasFontLoaded(string atlasName, AssetCallBack callBack, UIFont atlas)
	{
		if (atlas != null)
		{
			AtlasManager.PushFont(atlasName, atlas);
			if (callBack != null)
			{
				VarStore varStore = VarStore.CreateVarStore();
				varStore.PushParams(atlas);
				callBack(varStore);
			}
		}
		else
		{
			LogSystem.LogWarning(new object[]
			{
				"AtlasManager::OnAtlasFontLoaded ",
				atlasName,
				" Load is failed !"
			});
		}
	}

	private static UIFont GetComFont(UnityEngine.Object obj)
	{
		GameObject gameObject = obj as GameObject;
		if (gameObject != null)
		{
			return gameObject.GetComponent<UIFont>();
		}
		return null;
	}

	private static void PushFont(string atlasName, UIFont uiatlas)
	{
		if (uiatlas != null && AtlasManager.mDictionary.ContainsKey(atlasName))
		{
			AtlasManager.AtlasFont atlasFont = AtlasManager.mDictionary[atlasName] as AtlasManager.AtlasFont;
			atlasFont.UseFont = uiatlas;
		}
	}

	public static void PopFont(string atlasName, GameObject gameObject)
	{
		if (AtlasManager.mDictionary.ContainsKey(atlasName))
		{
			AtlasManager.AtlasBase atlasBase = AtlasManager.mDictionary[atlasName];
			int num = atlasBase.IndexOf(gameObject);
			if (num > -1)
			{
				atlasBase.Remove(gameObject);
			}
			if (atlasBase.Length <= 0)
			{
				atlasBase.Destroy();
				AtlasManager.mDictionary.Remove(atlasName);
			}
		}
	}

	public static void GetAtlas(string atlasName, GameObject gameObject, AssetCallBack callBack)
	{
		if (AtlasManager.mDictionary.ContainsKey(atlasName))
		{
			AtlasManager.AtlasUI atlasUI = AtlasManager.mDictionary[atlasName] as AtlasManager.AtlasUI;
			int num = atlasUI.IndexOf(gameObject);
			if (num > -1)
			{
				LogSystem.LogWarning(new object[]
				{
					"AtlasManager::GetAtlas gameObject = ",
					gameObject,
					" AtlasName = ",
					atlasName
				});
			}
			else
			{
				atlasUI.Add(gameObject);
			}
			if (atlasUI.UseAtlas != null)
			{
				VarStore varStore = VarStore.CreateVarStore();
				varStore.PushParams(atlasUI.UseAtlas);
				callBack(varStore);
			}
			else
			{
				AtlasManager.LoadAtlas(atlasName, callBack);
				LogSystem.LogWarning(new object[]
				{
					"AtlasManager::GetAtlas atlas.UseAtlas is null"
				});
			}
		}
		else
		{
			AtlasManager.CreateAtlasPool(atlasName, gameObject);
			AtlasManager.LoadAtlas(atlasName, callBack);
		}
	}

	private static void CreateAtlasPool(string atlasName, GameObject gameObject)
	{
		if (!AtlasManager.mDictionary.ContainsKey(atlasName))
		{
			AtlasManager.AtlasUI atlasUI = new AtlasManager.AtlasUI();
			atlasUI.AtlasName = atlasName;
			atlasUI.Add(gameObject);
			AtlasManager.mDictionary.Add(atlasName, atlasUI);
		}
	}

	private static void PushAtlas(string atlasName, UIAtlas uiatlas)
	{
		if (uiatlas != null && AtlasManager.mDictionary.ContainsKey(atlasName))
		{
			AtlasManager.AtlasUI atlasUI = AtlasManager.mDictionary[atlasName] as AtlasManager.AtlasUI;
			atlasUI.UseAtlas = uiatlas;
		}
	}

	public static void PopAtlas(string atlasName, GameObject gameObject)
	{
		if (AtlasManager.mDictionary.ContainsKey(atlasName))
		{
			AtlasManager.AtlasBase atlasBase = AtlasManager.mDictionary[atlasName];
			int num = atlasBase.IndexOf(gameObject);
			if (num > -1)
			{
				atlasBase.Remove(gameObject);
			}
			if (atlasBase.Length <= 0)
			{
				atlasBase.Destroy();
				AtlasManager.mDictionary.Remove(atlasName);
			}
		}
	}

	private static UIAtlas GetComAtlas(UnityEngine.Object obj)
	{
		GameObject gameObject = obj as GameObject;
		if (gameObject != null)
		{
			return gameObject.GetComponent<UIAtlas>();
		}
		return null;
	}

	private static void LoadAtlas(string strAtlasName, AssetCallBack callBack)
	{
		if (Application.isPlaying)
		{
			if (strAtlasName.StartsWith("Local/"))
			{
				if (!DelegateProxy.LoadAsset(strAtlasName, delegate(VarStore args)
				{
					AtlasManager.OnAtlasUILoaded(strAtlasName, callBack, AtlasManager.GetComAtlas(args[0] as UnityEngine.Object));
					args.Collect();
				}, false))
				{
					UnityEngine.Object obj = Resources.Load(strAtlasName);
					AtlasManager.OnAtlasUILoaded(strAtlasName, callBack, AtlasManager.GetComAtlas(obj));
				}
			}
			else
			{
				string text = AtlasManager.strAtlasPath + Language.LanguageVersion.ToString() + AtlasManager.strPathSplit + strAtlasName;
				if (!DelegateProxy.LoadAsset(text, delegate(VarStore args)
				{
					AtlasManager.OnAtlasUILoaded(strAtlasName, callBack, AtlasManager.GetComAtlas(args[0] as UnityEngine.Object));
					args.Collect();
				}, false))
				{
					UnityEngine.Object obj2 = Resources.Load(text);
					AtlasManager.OnAtlasUILoaded(strAtlasName, callBack, AtlasManager.GetComAtlas(obj2));
				}
			}
		}
	}

	private static void OnAtlasUILoaded(string atlasName, AssetCallBack callBack, UIAtlas atlas)
	{
		AtlasManager.PushAtlas(atlasName, atlas);
		if (callBack != null)
		{
			VarStore varStore = VarStore.CreateVarStore();
			varStore.PushParams(atlas);
			callBack(varStore);
		}
	}
}
