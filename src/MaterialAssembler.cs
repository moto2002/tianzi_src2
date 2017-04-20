using System;
using UnityEngine;

[ExecuteInEditMode]
public class MaterialAssembler : MonoBehaviour
{
	[Serializable]
	public struct TextureInfo
	{
		public string strProptyName;

		public string strPath;
	}

	public delegate void CallLoadAsset(string strFileName, AssetCallBack callback);

	[HideInInspector]
	public MaterialAssembler.TextureInfo[] mTextures = new MaterialAssembler.TextureInfo[0];

	public static MaterialAssembler.CallLoadAsset monLoadAsset;

	private GameObject go;

	private Material mainMat;

	private int callBackIndex;

	public int TextureCount
	{
		get
		{
			return this.mTextures.Length;
		}
	}

	public static void SetLoadAssetCall(MaterialAssembler.CallLoadAsset call)
	{
		MaterialAssembler.monLoadAsset = call;
	}

	private void Awake()
	{
		this.go = base.gameObject;
		GameObjectUtils.SetActive(this.go, false);
		Renderer component = base.GetComponent<Renderer>();
		if (component == null)
		{
			return;
		}
		this.callBackIndex = 0;
		this.mainMat = component.sharedMaterial;
		if (this.mainMat == null)
		{
			return;
		}
		for (int i = 0; i < this.mTextures.Length; i++)
		{
			if (MaterialAssembler.monLoadAsset != null)
			{
				MaterialAssembler.monLoadAsset(this.mTextures[i].strPath, new AssetCallBack(this.OnFileLoaded));
			}
			else
			{
				UnityEngine.Object oData = Resources.Load(this.mTextures[i].strPath);
				VarStore varStore = VarStore.CreateVarStore();
				varStore.PushParams(oData);
				varStore.PushParams(this.mTextures[i].strPath);
				this.OnFileLoaded(varStore);
			}
		}
	}

	private void OnFileLoaded(VarStore args)
	{
		this.callBackIndex++;
		if (this.callBackIndex == this.TextureCount)
		{
			GameObjectUtils.SetActive(this.go, true);
		}
		Texture texture = args[0] as Texture;
		if (texture == null)
		{
			return;
		}
		string fileName = args[1].ToString();
		string proptyName = this.GetProptyName(fileName);
		if (string.IsNullOrEmpty(proptyName))
		{
			return;
		}
		this.mainMat.SetTexture(proptyName, texture);
	}

	private string GetProptyName(string fileName)
	{
		if (string.IsNullOrEmpty(fileName))
		{
			return string.Empty;
		}
		for (int i = 0; i < this.mTextures.Length; i++)
		{
			if (this.mTextures[i].strPath.Equals(fileName))
			{
				return this.mTextures[i].strProptyName;
			}
		}
		return string.Empty;
	}
}
