using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ModelLoadEffect : EffectLevel
{
	public delegate void LoadFuncDelegate(string strFileName, AssetCallBack callback, VarStore store = null);

	public static ModelLoadEffect.LoadFuncDelegate LoadFunc = null;

	public Action<GameObject> LoadModelComplete;

	public string mstrPath = string.Empty;

	public Vector3 mvecPos = Vector3.zero;

	public Vector3 mvecRot = Vector3.zero;

	public Vector3 mvecScale = Vector3.one;

	public int miRenderQ = 3050;

	public Transform rootTrans;

	private GameObject mGoEffect;

	private bool bLoaded;

	private GameObject mGo;

	private UnityEngine.Object mPrefab;

	private string timerKey = string.Empty;

	public static List<int> instanceIDList = new List<int>();

	private int instanceID;

	private bool mbValidate;

	private bool mbHasLoaded;

	private bool mbDespawned;

	public Action gameObjectActiveCallBack;

	public GameObject GoEffect
	{
		get
		{
			return this.mGoEffect;
		}
	}

	public GameObject EffectGo
	{
		get
		{
			return this.mGo;
		}
	}

	private void Awake()
	{
		this.mGo = base.gameObject;
		this.instanceID = base.GetInstanceID();
		DelegateProxy.DespawnedProxy = (DelegateProxy.OnDespawnedDelegateProxy)Delegate.Combine(DelegateProxy.DespawnedProxy, new DelegateProxy.OnDespawnedDelegateProxy(this.OnDespawned));
	}

	private void OnEnable()
	{
		if (this.mbDespawned)
		{
			this.LevelChange(EffectLevel.iEffectLevel);
			return;
		}
		if (!this.mbHasLoaded)
		{
			return;
		}
		if (!this.CheckEffectValidate())
		{
			this.mbValidate = true;
			if (!string.IsNullOrEmpty(this.mstrPath) && ModelLoadEffect.LoadFunc != null)
			{
				ModelLoadEffect.LoadFunc(this.mstrPath, new AssetCallBack(this.OnValidateLoadComplete), null);
			}
		}
	}

	private void OnDestroy()
	{
		TimerManager.DestroyTimer(this.timerKey);
		if (this.gameObjectActiveCallBack != null)
		{
			this.gameObjectActiveCallBack = null;
		}
		this.LoadModelComplete = null;
		DelegateProxy.DespawnedProxy = (DelegateProxy.OnDespawnedDelegateProxy)Delegate.Remove(DelegateProxy.DespawnedProxy, new DelegateProxy.OnDespawnedDelegateProxy(this.OnDespawned));
	}

	private void OnDespawned(Transform root)
	{
		if (root == this.rootTrans)
		{
			this.mGoEffect = null;
			this.bLoaded = false;
			if (ModelLoadEffect.instanceIDList.Contains(this.instanceID))
			{
				ModelLoadEffect.instanceIDList.Remove(this.instanceID);
			}
			this.LoadModelComplete = null;
			this.mbDespawned = true;
			EffectLevel.effectLevelLocked = false;
		}
	}

	private bool CheckEffectValidate()
	{
		if (this.mGoEffect == null)
		{
			return this.mbValidate;
		}
		return this.mGoEffect.transform.parent == this.mGo.transform;
	}

	protected override void SetActive(bool bShow)
	{
		if (bShow)
		{
			if (this.mGoEffect != null)
			{
				this.mGoEffect.transform.parent = base.transform;
				this.mGoEffect.transform.localPosition = this.mvecPos;
				this.mGoEffect.transform.localRotation = Quaternion.Euler(this.mvecRot);
				this.mGoEffect.transform.localScale = this.mvecScale;
				GameObjectUtils.SetActive(this.mGoEffect, true);
				if (this.gameObjectActiveCallBack != null)
				{
					this.gameObjectActiveCallBack();
				}
			}
			else if (!string.IsNullOrEmpty(this.mstrPath) && ModelLoadEffect.LoadFunc != null && !this.bLoaded)
			{
				this.bLoaded = true;
				if (!ModelLoadEffect.instanceIDList.Contains(this.instanceID))
				{
					ModelLoadEffect.instanceIDList.Add(this.instanceID);
					ModelLoadEffect.LoadFunc(this.mstrPath, new AssetCallBack(this.ResLoadComplete), null);
				}
			}
		}
		else if (this.mGoEffect != null)
		{
			GameObjectUtils.SetActive(this.mGoEffect, false);
			this.mGoEffect.transform.parent = base.transform;
			this.mGoEffect.transform.localPosition = this.mvecPos;
			this.mGoEffect.transform.localRotation = Quaternion.Euler(this.mvecRot);
			this.mGoEffect.transform.localScale = this.mvecScale;
			if (this.gameObjectActiveCallBack != null)
			{
				this.gameObjectActiveCallBack();
			}
		}
	}

	protected override void DestoryELevel()
	{
		this.mGoEffect = null;
		this.mGo = null;
		this.gameObjectActiveCallBack = null;
		this.LoadModelComplete = null;
		this.mPrefab = null;
	}

	private void OnValidateLoadComplete(VarStore args)
	{
		if (this.mGo == null)
		{
			return;
		}
		this.mPrefab = (args[0] as UnityEngine.Object);
		args.Collect();
		if (this.mPrefab != null)
		{
			GameObject gameObject = DelegateProxy.Instantiate(this.mPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			if (gameObject != null)
			{
				gameObject.transform.parent = base.transform;
				gameObject.transform.localPosition = this.mvecPos;
				gameObject.transform.localRotation = Quaternion.Euler(this.mvecRot);
				gameObject.transform.localScale = this.mvecScale;
				this.mGoEffect = gameObject;
				NGUITools.SetLayer(this.mGoEffect, this.mGo.layer);
				GameObjectUtils.SetActive(this.mGoEffect, this.mbCurActive);
				this.mbValidate = false;
			}
		}
		if (this.LoadModelComplete != null)
		{
			this.LoadModelComplete(this.mGoEffect);
		}
		if (this.gameObjectActiveCallBack != null)
		{
			this.gameObjectActiveCallBack();
		}
	}

	private void ResLoadComplete(VarStore args)
	{
		if (this.mGo == null)
		{
			return;
		}
		if (!base.enabled)
		{
			this.mGoEffect = null;
			this.bLoaded = false;
			return;
		}
		this.mPrefab = (args[0] as UnityEngine.Object);
		args.Collect();
		this.DisPlayModelEffect(this.mPrefab, out this.mGoEffect);
		if (this.LoadModelComplete != null)
		{
			this.LoadModelComplete(this.mGoEffect);
		}
		if (this.gameObjectActiveCallBack != null)
		{
			this.gameObjectActiveCallBack();
		}
	}

	private bool DisPlayModelEffect(UnityEngine.Object prefab, out GameObject oEffect)
	{
		if (prefab == null)
		{
			oEffect = null;
			return false;
		}
		oEffect = (DelegateProxy.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject);
		if (oEffect != null)
		{
			oEffect.transform.parent = base.transform;
			oEffect.transform.localPosition = this.mvecPos;
			oEffect.transform.localRotation = Quaternion.Euler(this.mvecRot);
			oEffect.transform.localScale = this.mvecScale;
			NGUITools.SetLayer(oEffect, this.mGo.layer);
			GameObjectUtils.SetActive(oEffect, this.mbCurActive);
			this.mbHasLoaded = true;
			EffectLevel.effectLevelLocked = false;
			return true;
		}
		return false;
	}

	public override void LockEffectLevel()
	{
		if (!this.mbHasLoaded)
		{
			EffectLevel.effectLevelLocked = true;
		}
	}
}
