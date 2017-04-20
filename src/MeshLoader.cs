using System;
using UnityEngine;

[ExecuteInEditMode]
[Serializable]
public class MeshLoader : MonoBehaviour
{
	public delegate void CallLoadAsset(string strFileName, AssetCallBack callback, VarStore store = null);

	[SerializeField]
	public string strAssetName = string.Empty;

	public MaterialLoader matchMono;

	private MeshAsset oSelfAsset;

	public static MeshLoader.CallLoadAsset monLoadAsset;

	private void Start()
	{
		if (!string.IsNullOrEmpty(this.strAssetName))
		{
			this.LoadAsset(this.strAssetName);
		}
	}

	private void LoadAsset(string strMeshName)
	{
		if (MeshLoader.monLoadAsset != null)
		{
			MeshLoader.monLoadAsset(strMeshName, new AssetCallBack(this.OnFileLoaded), null);
		}
		else
		{
			UnityEngine.Object oData = Resources.Load(this.strAssetName);
			VarStore varStore = VarStore.CreateVarStore();
			varStore.PushParams(oData);
			this.OnFileLoaded(varStore);
		}
	}

	public void CombineRender(MaterialAsset oOtherAsset)
	{
		if (this.oSelfAsset == null)
		{
			return;
		}
		MeshRenderer component = base.GetComponent<MeshRenderer>();
		if (component != null)
		{
			component.material = oOtherAsset.material;
			GameObjectUnit.ChangeShader(component);
		}
		MeshFilter component2 = base.GetComponent<MeshFilter>();
		if (component2 != null)
		{
			component2.mesh = this.oSelfAsset.mesh;
		}
		if (this.matchMono != null)
		{
			UnityEngine.Object.Destroy(this.matchMono);
		}
		UnityEngine.Object.Destroy(this);
	}

	public static void SetLoadAssetCall(MeshLoader.CallLoadAsset call)
	{
		MeshLoader.monLoadAsset = call;
	}

	private void OnFileLoaded(VarStore args)
	{
		MeshAsset x = args[0] as MeshAsset;
		args.Collect();
		if (x == null)
		{
			return;
		}
		this.oSelfAsset = x;
		if (this.matchMono != null)
		{
			this.matchMono.CombineRender(this.oSelfAsset);
		}
		else
		{
			MeshFilter component = base.GetComponent<MeshFilter>();
			if (component != null)
			{
				component.mesh = this.oSelfAsset.mesh;
			}
		}
	}

	private void OnDestroy()
	{
		this.oSelfAsset = null;
	}
}
