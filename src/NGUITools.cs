using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public static class NGUITools
{
	private struct ParticleInfo
	{
		public float fStartSize;

		public float fStartSpeed;

		public int iMaxParticles;

		public float fMaxParticleSize;

		public float fGravityModifier;

		public static NGUITools.ParticleInfo Zero = new NGUITools.ParticleInfo(0f, 0f, 0, 0f, 1f, 1f);

		public float fScaleRate;

		public ParticleInfo(float fStartSize, float fStartSpeed, int iMaxParticles, float fMaxParticleSize, float fScaleRate, float fGravityModifier)
		{
			this.fStartSize = fStartSize;
			this.fStartSpeed = fStartSpeed;
			this.iMaxParticles = iMaxParticles;
			this.fMaxParticleSize = fMaxParticleSize;
			this.fScaleRate = fScaleRate;
			this.fGravityModifier = fGravityModifier;
		}
	}

	public delegate void OnPlaySoundEvent(int index);

	private static AudioListener mListener;

	private static bool mLoaded = false;

	private static float mGlobalVolume = 1f;

	public static NGUITools.OnPlaySoundEvent moSoundLisnter = null;

	private static Dictionary<int, NGUITools.ParticleInfo> mAdjustTransDict = new Dictionary<int, NGUITools.ParticleInfo>();

	private static Vector3[] mSides = new Vector3[4];

	public static float soundVolume
	{
		get
		{
			if (!NGUITools.mLoaded)
			{
				NGUITools.mLoaded = true;
				NGUITools.mGlobalVolume = PlayerPrefs.GetFloat("Sound", 1f);
			}
			return NGUITools.mGlobalVolume;
		}
		set
		{
			if (NGUITools.mGlobalVolume != value)
			{
				NGUITools.mLoaded = true;
				NGUITools.mGlobalVolume = value;
				PlayerPrefs.SetFloat("Sound", value);
			}
		}
	}

	public static bool fileAccess
	{
		get
		{
			return Application.platform != RuntimePlatform.WindowsWebPlayer && Application.platform != RuntimePlatform.OSXWebPlayer;
		}
	}

	public static string clipboard
	{
		get
		{
			TextEditor textEditor = new TextEditor();
			textEditor.Paste();
			return textEditor.content.text;
		}
		set
		{
			TextEditor textEditor = new TextEditor();
			textEditor.content = new GUIContent(value);
			textEditor.OnFocus();
			textEditor.Copy();
		}
	}

	public static void SetPlaySoundEvent(NGUITools.OnPlaySoundEvent onSoundEvent)
	{
		NGUITools.moSoundLisnter = onSoundEvent;
	}

	public static void PlaySound(int index)
	{
		if (NGUITools.moSoundLisnter != null)
		{
			NGUITools.moSoundLisnter(index);
		}
	}

	public static AudioSource PlaySound(AudioClip clip)
	{
		return NGUITools.PlaySound(clip, 1f, 1f);
	}

	public static AudioSource PlaySound(AudioClip clip, float volume)
	{
		return NGUITools.PlaySound(clip, volume, 1f);
	}

	public static AudioSource PlaySound(AudioClip clip, float volume, float pitch)
	{
		volume *= NGUITools.soundVolume;
		if (clip != null && volume > 0.01f)
		{
			if (NGUITools.mListener == null || !NGUITools.GetActive(NGUITools.mListener))
			{
				NGUITools.mListener = (UnityEngine.Object.FindObjectOfType(typeof(AudioListener)) as AudioListener);
				if (NGUITools.mListener == null)
				{
					Camera camera = Camera.main;
					if (camera == null)
					{
						camera = (UnityEngine.Object.FindObjectOfType(typeof(Camera)) as Camera);
					}
					if (camera != null)
					{
						NGUITools.mListener = camera.gameObject.AddComponent<AudioListener>();
					}
				}
			}
			if (NGUITools.mListener != null && NGUITools.mListener.enabled && NGUITools.GetActive(NGUITools.mListener.gameObject))
			{
				AudioSource audioSource = NGUITools.mListener.audio;
				if (audioSource == null)
				{
					audioSource = NGUITools.mListener.gameObject.AddComponent<AudioSource>();
				}
				audioSource.pitch = pitch;
				audioSource.PlayOneShot(clip, volume);
				return audioSource;
			}
		}
		return null;
	}

	public static WWW OpenURL(string url)
	{
		WWW result = null;
		try
		{
			result = new WWW(url);
		}
		catch (Exception ex)
		{
			LogSystem.LogError(new object[]
			{
				ex.ToString()
			});
		}
		return result;
	}

	public static WWW OpenURL(string url, WWWForm form)
	{
		if (form == null)
		{
			return NGUITools.OpenURL(url);
		}
		WWW result = null;
		try
		{
			result = new WWW(url, form);
		}
		catch (Exception ex)
		{
			LogSystem.LogError(new object[]
			{
				(ex == null) ? "<null>" : ex.ToString()
			});
		}
		return result;
	}

	public static int RandomRange(int min, int max)
	{
		if (min == max)
		{
			return min;
		}
		return UnityEngine.Random.Range(min, max + 1);
	}

	public static string GetHierarchy(GameObject obj)
	{
		string text = obj.name;
		while (obj.transform.parent != null)
		{
			obj = obj.transform.parent.gameObject;
			text = obj.name + "\\" + text;
		}
		return text;
	}

	public static T[] FindActive<T>() where T : Component
	{
		return UnityEngine.Object.FindObjectsOfType(typeof(T)) as T[];
	}

	public static Camera FindCameraForLayer(int layer)
	{
		int num = 1 << layer;
		Camera camera;
		for (int i = 0; i < UICamera.list.size; i++)
		{
			camera = UICamera.list.buffer[i].cachedCamera;
			if (camera != null && (camera.cullingMask & num) != 0)
			{
				return camera;
			}
		}
		camera = Camera.main;
		if (camera != null && (camera.cullingMask & num) != 0)
		{
			return camera;
		}
		Camera[] array = NGUITools.FindActive<Camera>();
		int j = 0;
		int num2 = array.Length;
		while (j < num2)
		{
			camera = array[j];
			if ((camera.cullingMask & num) != 0)
			{
				return camera;
			}
			j++;
		}
		return null;
	}

	public static BoxCollider AddWidgetCollider(GameObject go)
	{
		return NGUITools.AddWidgetCollider(go, false);
	}

	public static BoxCollider AddWidgetCollider(GameObject go, bool considerInactive)
	{
		if (go != null)
		{
			Collider component = go.GetComponent<Collider>();
			BoxCollider boxCollider = component as BoxCollider;
			UIWidget uIWidget = null;
			if (boxCollider == null)
			{
				if (component != null)
				{
					if (Application.isPlaying)
					{
						UnityEngine.Object.Destroy(component);
					}
					else
					{
						UnityEngine.Object.DestroyImmediate(component);
					}
				}
				boxCollider = go.AddComponent<BoxCollider>();
				boxCollider.isTrigger = true;
				uIWidget = go.GetComponent<UIWidget>();
				if (uIWidget != null)
				{
					uIWidget.SetBoxCollider(boxCollider);
					uIWidget.autoResizeBoxCollider = true;
				}
			}
			NGUITools.UpdateWidgetCollider(uIWidget, boxCollider, considerInactive);
			return boxCollider;
		}
		return null;
	}

	public static void UpdateWidgetCollider(UIWidget widget, BoxCollider box, bool considerInactive)
	{
		if (box != null)
		{
			GameObject gameObject = box.gameObject;
			if (widget != null)
			{
				Vector4 drawingDimensions = widget.drawingDimensions;
				Vector3 zero = Vector3.zero;
				zero.x = (drawingDimensions.x + drawingDimensions.z) * 0.5f;
				zero.y = (drawingDimensions.y + drawingDimensions.w) * 0.5f;
				box.center = zero;
				zero.x = drawingDimensions.z - drawingDimensions.x;
				zero.y = drawingDimensions.w - drawingDimensions.y;
				box.size = zero;
			}
			else
			{
				Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(gameObject.transform, considerInactive);
				box.center = bounds.center;
				Vector3 zero2 = Vector3.zero;
				zero2.x = bounds.size.x;
				zero2.y = bounds.size.y;
				box.size = zero2;
			}
		}
	}

	public static string GetTypeName<T>()
	{
		string text = typeof(T).ToString();
		if (text.StartsWith("UI"))
		{
			text = text.Substring(2);
		}
		else if (text.StartsWith("UnityEngine."))
		{
			text = text.Substring(12);
		}
		return text;
	}

	public static string GetTypeName(UnityEngine.Object obj)
	{
		if (obj == null)
		{
			return "Null";
		}
		string text = obj.GetType().ToString();
		if (text.StartsWith("UI"))
		{
			text = text.Substring(2);
		}
		else if (text.StartsWith("UnityEngine."))
		{
			text = text.Substring(12);
		}
		return text;
	}

	public static void RegisterUndo(UnityEngine.Object obj, string name)
	{
	}

	public static void SetDirty(UnityEngine.Object obj)
	{
	}

	public static GameObject AddChild(GameObject parent)
	{
		return NGUITools.AddChild(parent, true);
	}

	public static GameObject AddChild(GameObject parent, bool undo)
	{
		GameObject gameObject = new GameObject();
		if (parent != null)
		{
			Transform transform = gameObject.transform;
			transform.parent = parent.transform;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
			gameObject.layer = parent.layer;
		}
		return gameObject;
	}

	public static GameObject AddChild(GameObject parent, GameObject prefab)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(prefab) as GameObject;
		if (gameObject != null && parent != null)
		{
			Transform transform = gameObject.transform;
			transform.parent = parent.transform;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
			gameObject.layer = parent.layer;
		}
		return gameObject;
	}

	public static int CalculateRaycastDepth(GameObject go)
	{
		UIWidget component = go.GetComponent<UIWidget>();
		if (component != null)
		{
			return component.raycastDepth;
		}
		UIWidget[] componentsInChildren = go.GetComponentsInChildren<UIWidget>();
		if (componentsInChildren.Length == 0)
		{
			return 0;
		}
		int num = 2147483647;
		int i = 0;
		int num2 = componentsInChildren.Length;
		while (i < num2)
		{
			if (componentsInChildren[i].enabled)
			{
				num = Mathf.Min(num, componentsInChildren[i].raycastDepth);
			}
			i++;
		}
		return num;
	}

	public static int CalculateNextDepth(GameObject go)
	{
		int num = -1;
		UIWidget[] componentsInChildren = go.GetComponentsInChildren<UIWidget>();
		int i = 0;
		int num2 = componentsInChildren.Length;
		while (i < num2)
		{
			num = Mathf.Max(num, componentsInChildren[i].depth);
			i++;
		}
		return num + 1;
	}

	public static int CalculateNextDepth(GameObject go, bool ignoreChildrenWithColliders)
	{
		if (ignoreChildrenWithColliders)
		{
			int num = -1;
			UIWidget[] componentsInChildren = go.GetComponentsInChildren<UIWidget>();
			int i = 0;
			int num2 = componentsInChildren.Length;
			while (i < num2)
			{
				UIWidget uIWidget = componentsInChildren[i];
				if (!(uIWidget.cachedGameObject != go) || !(uIWidget.collider != null))
				{
					num = Mathf.Max(num, uIWidget.depth);
				}
				i++;
			}
			return num + 1;
		}
		return NGUITools.CalculateNextDepth(go);
	}

	public static int AdjustDepth(GameObject go, int adjustment)
	{
		if (!(go != null))
		{
			return 0;
		}
		UIPanel component = go.GetComponent<UIPanel>();
		if (component != null)
		{
			UIPanel[] componentsInChildren = go.GetComponentsInChildren<UIPanel>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				UIPanel uIPanel = componentsInChildren[i];
				uIPanel.depth += adjustment;
			}
			return 1;
		}
		UIWidget[] componentsInChildren2 = go.GetComponentsInChildren<UIWidget>(true);
		int j = 0;
		int num = componentsInChildren2.Length;
		while (j < num)
		{
			UIWidget uIWidget = componentsInChildren2[j];
			uIWidget.depth += adjustment;
			j++;
		}
		return 2;
	}

	public static void BringForward(GameObject go)
	{
		int num = NGUITools.AdjustDepth(go, 1000);
		if (num == 1)
		{
			NGUITools.NormalizePanelDepths();
		}
		else if (num == 2)
		{
			NGUITools.NormalizeWidgetDepths();
		}
	}

	public static void PushBack(GameObject go)
	{
		int num = NGUITools.AdjustDepth(go, -1000);
		if (num == 1)
		{
			NGUITools.NormalizePanelDepths();
		}
		else if (num == 2)
		{
			NGUITools.NormalizeWidgetDepths();
		}
	}

	public static void NormalizeDepths()
	{
		NGUITools.NormalizeWidgetDepths();
		NGUITools.NormalizePanelDepths();
	}

	public static void NormalizeWidgetDepths()
	{
		UIWidget[] array = NGUITools.FindActive<UIWidget>();
		int num = array.Length;
		if (num > 0)
		{
			Array.Sort<UIWidget>(array, new Comparison<UIWidget>(UIWidget.FullCompareFunc));
			int num2 = 0;
			int depth = array[0].depth;
			for (int i = 0; i < num; i++)
			{
				UIWidget uIWidget = array[i];
				if (uIWidget.depth == depth)
				{
					uIWidget.depth = num2;
				}
				else
				{
					depth = uIWidget.depth;
					num2 = (uIWidget.depth = num2 + 1);
				}
			}
		}
	}

	public static void NormalizePanelDepths()
	{
		UIPanel[] array = NGUITools.FindActive<UIPanel>();
		int num = array.Length;
		if (num > 0)
		{
			Array.Sort<UIPanel>(array, new Comparison<UIPanel>(UIPanel.CompareFunc));
			int num2 = 0;
			int depth = array[0].depth;
			for (int i = 0; i < num; i++)
			{
				UIPanel uIPanel = array[i];
				if (uIPanel.depth == depth)
				{
					uIPanel.depth = num2;
				}
				else
				{
					depth = uIPanel.depth;
					num2 = (uIPanel.depth = num2 + 1);
				}
			}
		}
	}

	public static UIPanel CreateUI(bool advanced3D)
	{
		return NGUITools.CreateUI(null, advanced3D, -1);
	}

	public static UIPanel CreateUI(bool advanced3D, int layer)
	{
		return NGUITools.CreateUI(null, advanced3D, layer);
	}

	public static UIPanel CreateUI(Transform trans, bool advanced3D, int layer)
	{
		UIRoot uIRoot = (!(trans != null)) ? null : NGUITools.FindInParents<UIRoot>(trans.gameObject);
		if (uIRoot == null && UIRoot.list.Count > 0)
		{
			uIRoot = UIRoot.list[0];
		}
		if (uIRoot == null)
		{
			GameObject gameObject = NGUITools.AddChild(null, false);
			uIRoot = gameObject.AddComponent<UIRoot>();
			if (layer == -1)
			{
				layer = LayerMask.NameToLayer("UI");
			}
			if (layer == -1)
			{
				layer = LayerMask.NameToLayer("2D UI");
			}
			gameObject.layer = layer;
			if (advanced3D)
			{
				gameObject.name = "UI Root (3D)";
				uIRoot.scalingStyle = UIRoot.Scaling.FixedSize;
			}
			else
			{
				gameObject.name = "UI Root";
				uIRoot.scalingStyle = UIRoot.Scaling.PixelPerfect;
			}
		}
		UIPanel uIPanel = uIRoot.GetComponentInChildren<UIPanel>();
		if (uIPanel == null)
		{
			Camera[] array = NGUITools.FindActive<Camera>();
			float num = -1f;
			bool flag = false;
			int num2 = 1 << uIRoot.gameObject.layer;
			for (int i = 0; i < array.Length; i++)
			{
				Camera camera = array[i];
				if (camera.clearFlags == CameraClearFlags.Color || camera.clearFlags == CameraClearFlags.Skybox)
				{
					flag = true;
				}
				num = Mathf.Max(num, camera.depth);
				camera.cullingMask &= ~num2;
			}
			Camera camera2 = NGUITools.AddChild<Camera>(uIRoot.gameObject, false);
			camera2.gameObject.AddComponent<UICamera>();
			camera2.clearFlags = ((!flag) ? CameraClearFlags.Color : CameraClearFlags.Depth);
			camera2.backgroundColor = Color.grey;
			camera2.cullingMask = num2;
			camera2.depth = num + 1f;
			if (advanced3D)
			{
				camera2.nearClipPlane = 0.1f;
				camera2.farClipPlane = 4f;
				Vector3 zero = Vector3.zero;
				zero.z = -700f;
				camera2.transform.localPosition = zero;
			}
			else
			{
				camera2.orthographic = true;
				camera2.orthographicSize = 1f;
				camera2.nearClipPlane = -10f;
				camera2.farClipPlane = 10f;
			}
			AudioListener[] array2 = NGUITools.FindActive<AudioListener>();
			if (array2 == null || array2.Length == 0)
			{
				camera2.gameObject.AddComponent<AudioListener>();
			}
			uIPanel = uIRoot.gameObject.AddComponent<UIPanel>();
		}
		if (trans != null)
		{
			while (trans.parent != null)
			{
				trans = trans.parent;
			}
			if (NGUITools.IsChild(trans, uIPanel.transform))
			{
				uIPanel = trans.gameObject.AddComponent<UIPanel>();
			}
			else
			{
				trans.parent = uIPanel.transform;
				trans.localScale = Vector3.one;
				trans.localPosition = Vector3.zero;
				NGUITools.SetChildLayer(uIPanel.cachedTransform, uIPanel.cachedGameObject.layer);
			}
		}
		return uIPanel;
	}

	public static void SetChildLayer(Transform t, int layer)
	{
		for (int i = 0; i < t.childCount; i++)
		{
			Transform child = t.GetChild(i);
			child.gameObject.layer = layer;
			NGUITools.SetChildLayer(child, layer);
		}
	}

	public static T AddChild<T>(GameObject parent) where T : Component
	{
		GameObject gameObject = NGUITools.AddChild(parent);
		gameObject.name = NGUITools.GetTypeName<T>();
		return gameObject.AddComponent<T>();
	}

	public static T AddChild<T>(GameObject parent, bool undo) where T : Component
	{
		GameObject gameObject = NGUITools.AddChild(parent, undo);
		gameObject.name = NGUITools.GetTypeName<T>();
		return gameObject.AddComponent<T>();
	}

	public static T AddWidget<T>(GameObject go) where T : UIWidget
	{
		int depth = NGUITools.CalculateNextDepth(go);
		T result = NGUITools.AddChild<T>(go);
		result.width = 100;
		result.height = 100;
		result.depth = depth;
		result.gameObject.layer = go.layer;
		return result;
	}

	public static UISprite AddSprite(GameObject go, UIAtlas atlas, string spriteName)
	{
		UISpriteData uISpriteData = (!(atlas != null)) ? null : atlas.GetSprite(spriteName);
		UISprite uISprite = NGUITools.AddWidget<UISprite>(go);
		uISprite.type = ((uISpriteData != null && uISpriteData.hasBorder) ? UISprite.Type.Sliced : UISprite.Type.Simple);
		uISprite.atlas = atlas;
		uISprite.spriteName = spriteName;
		return uISprite;
	}

	public static GameObject GetRoot(GameObject go)
	{
		Transform transform = go.transform;
		while (true)
		{
			Transform parent = transform.parent;
			if (parent == null)
			{
				break;
			}
			transform = parent;
		}
		return transform.gameObject;
	}

	public static T FindInParents<T>(GameObject go) where T : Component
	{
		if (go == null)
		{
			return (T)((object)null);
		}
		T component = go.GetComponent<T>();
		if (component == null)
		{
			Transform parent = go.transform.parent;
			while (parent != null && component == null)
			{
				component = parent.gameObject.GetComponent<T>();
				parent = parent.parent;
			}
		}
		return component;
	}

	public static T FindInParents<T>(Transform trans) where T : Component
	{
		if (trans == null)
		{
			return (T)((object)null);
		}
		T component = trans.GetComponent<T>();
		if (component == null)
		{
			Transform parent = trans.transform.parent;
			while (parent != null && component == null)
			{
				component = parent.gameObject.GetComponent<T>();
				parent = parent.parent;
			}
		}
		return component;
	}

	public static void Destroy(UnityEngine.Object obj)
	{
		if (obj != null)
		{
			if (Application.isPlaying)
			{
				if (obj is GameObject)
				{
					GameObject gameObject = obj as GameObject;
					gameObject.transform.parent = null;
				}
				UnityEngine.Object.Destroy(obj);
			}
			else
			{
				UnityEngine.Object.DestroyImmediate(obj);
			}
		}
	}

	public static void DestroyImmediate(UnityEngine.Object obj)
	{
		if (obj != null)
		{
			if (Application.isEditor)
			{
				UnityEngine.Object.DestroyImmediate(obj);
			}
			else
			{
				UnityEngine.Object.Destroy(obj);
			}
		}
	}

	public static void Broadcast(string funcName)
	{
		GameObject[] array = UnityEngine.Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		int i = 0;
		int num = array.Length;
		while (i < num)
		{
			array[i].SendMessage(funcName, SendMessageOptions.DontRequireReceiver);
			i++;
		}
	}

	public static void Broadcast(string funcName, object param)
	{
		GameObject[] array = UnityEngine.Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		int i = 0;
		int num = array.Length;
		while (i < num)
		{
			array[i].SendMessage(funcName, param, SendMessageOptions.DontRequireReceiver);
			i++;
		}
	}

	public static bool IsChild(Transform parent, Transform child)
	{
		if (parent == null || child == null)
		{
			return false;
		}
		while (child != null)
		{
			if (child == parent)
			{
				return true;
			}
			child = child.parent;
		}
		return false;
	}

	private static void Activate(Transform t)
	{
		NGUITools.Activate(t, true);
	}

	private static void Activate(Transform t, bool compatibilityMode)
	{
		NGUITools.SetActiveSelf(t.gameObject, true);
		if (compatibilityMode)
		{
			int i = 0;
			int childCount = t.childCount;
			while (i < childCount)
			{
				Transform child = t.GetChild(i);
				if (child.gameObject.activeSelf)
				{
					return;
				}
				i++;
			}
			int j = 0;
			int childCount2 = t.childCount;
			while (j < childCount2)
			{
				Transform child2 = t.GetChild(j);
				NGUITools.Activate(child2, true);
				j++;
			}
		}
	}

	private static void Deactivate(Transform t)
	{
		NGUITools.SetActiveSelf(t.gameObject, false);
	}

	public static void SetActive(GameObject go, bool state)
	{
		NGUITools.SetActive(go, state, true);
	}

	public static void SetActive(GameObject go, bool state, bool compatibilityMode)
	{
		if (go)
		{
			if (state)
			{
				NGUITools.Activate(go.transform, compatibilityMode);
				NGUITools.CallCreatePanel(go.transform);
			}
			else
			{
				NGUITools.Deactivate(go.transform);
			}
		}
	}

	[DebuggerHidden, DebuggerStepThrough]
	private static void CallCreatePanel(Transform t)
	{
		UIWidget component = t.GetComponent<UIWidget>();
		if (component != null)
		{
			component.CreatePanel();
		}
		int i = 0;
		int childCount = t.childCount;
		while (i < childCount)
		{
			NGUITools.CallCreatePanel(t.GetChild(i));
			i++;
		}
	}

	public static void SetActiveChildren(GameObject go, bool state)
	{
		Transform transform = go.transform;
		if (state)
		{
			int i = 0;
			int childCount = transform.childCount;
			while (i < childCount)
			{
				Transform child = transform.GetChild(i);
				NGUITools.Activate(child);
				i++;
			}
		}
		else
		{
			int j = 0;
			int childCount2 = transform.childCount;
			while (j < childCount2)
			{
				Transform child2 = transform.GetChild(j);
				NGUITools.Deactivate(child2);
				j++;
			}
		}
	}

	[Obsolete("Use NGUITools.GetActive instead")]
	public static bool IsActive(Behaviour mb)
	{
		return mb != null && mb.enabled && mb.gameObject.activeInHierarchy;
	}

	public static bool GetActive(Behaviour mb)
	{
		return mb != null && mb.enabled && mb.gameObject.activeInHierarchy;
	}

	public static bool GetActive(GameObject go)
	{
		return go && go.activeInHierarchy;
	}

	public static void SetActiveSelf(GameObject go, bool state)
	{
		go.SetActive(state);
	}

	public static void SetLayer(GameObject go, int layer)
	{
		if (go.layer == layer)
		{
			if (go.transform.childCount > 0)
			{
				int childCount = go.transform.childCount;
				for (int i = 0; i < childCount; i++)
				{
					NGUITools.SetLayer(go.transform.GetChild(i).gameObject, layer);
				}
			}
		}
		else
		{
			go.layer = layer;
			if (go.transform.childCount > 0)
			{
				int childCount2 = go.transform.childCount;
				for (int j = 0; j < childCount2; j++)
				{
					NGUITools.SetLayer(go.transform.GetChild(j).gameObject, layer);
				}
			}
		}
	}

	public static Vector3 GetScaleByParent(Transform trans)
	{
		Vector3 one = Vector3.one;
		if (trans == null)
		{
			return one;
		}
		one.x = trans.localScale.x;
		one.y = trans.localScale.x;
		one.z = trans.localScale.x;
		Transform parent = trans.parent;
		while (parent != null)
		{
			one.x *= parent.localScale.x;
			one.y *= parent.localScale.y;
			one.z *= parent.localScale.z;
			parent = parent.parent;
		}
		return one;
	}

	public static void AdjustParticlesToUI(GameObject oModel, float scaleRate)
	{
		if (oModel == null)
		{
			return;
		}
		ParticleSystem[] componentsInChildren = oModel.GetComponentsInChildren<ParticleSystem>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			ParticleSystem particleSystem = componentsInChildren[i];
			if (particleSystem != null)
			{
				NGUITools.ParticleInfo zero = NGUITools.ParticleInfo.Zero;
				zero.fStartSize = particleSystem.startSize;
				zero.fStartSpeed = particleSystem.startSpeed;
				zero.iMaxParticles = particleSystem.maxParticles;
				zero.fScaleRate = scaleRate;
				zero.fGravityModifier = particleSystem.gravityModifier;
				particleSystem.startSize *= scaleRate;
				particleSystem.startSpeed *= scaleRate;
				particleSystem.gravityModifier *= scaleRate;
				ParticleSystemRenderer component = particleSystem.GetComponent<ParticleSystemRenderer>();
				if (component)
				{
					zero.fMaxParticleSize = component.maxParticleSize;
					component.maxParticleSize *= scaleRate;
				}
				int hashCode = particleSystem.transform.GetHashCode();
				if (!NGUITools.mAdjustTransDict.ContainsKey(hashCode))
				{
					NGUITools.mAdjustTransDict.Add(hashCode, zero);
				}
				particleSystem.transform.gameObject.layer = LayerMask.NameToLayer("UIEffect");
			}
		}
	}

	public static void AdjustParticlesToWorld(GameObject oModel)
	{
		if (oModel == null)
		{
			return;
		}
		ParticleSystem[] componentsInChildren = oModel.GetComponentsInChildren<ParticleSystem>(true);
		if (componentsInChildren != null)
		{
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				ParticleSystem particleSystem = componentsInChildren[i];
				if (particleSystem.transform.gameObject.layer == LayerMask.NameToLayer("UIEffect"))
				{
					int hashCode = particleSystem.transform.GetHashCode();
					if (NGUITools.mAdjustTransDict.ContainsKey(hashCode))
					{
						NGUITools.ParticleInfo particleInfo = NGUITools.mAdjustTransDict[hashCode];
						particleSystem.startSize = particleInfo.fStartSize;
						particleSystem.startSpeed = particleInfo.fStartSpeed;
						particleSystem.maxParticles = particleInfo.iMaxParticles;
						particleSystem.gravityModifier = particleInfo.fGravityModifier;
						ParticleSystemRenderer component = particleSystem.GetComponent<ParticleSystemRenderer>();
						if (component)
						{
							component.maxParticleSize = particleInfo.fMaxParticleSize;
						}
						NGUITools.mAdjustTransDict.Remove(hashCode);
					}
					particleSystem.transform.gameObject.layer = LayerMask.NameToLayer("UI");
				}
			}
		}
	}

	public static Vector3 Round(Vector3 v)
	{
		v.x = Mathf.Round(v.x);
		v.y = Mathf.Round(v.y);
		v.z = Mathf.Round(v.z);
		return v;
	}

	public static void MakePixelPerfect(Transform t)
	{
		UIWidget component = t.GetComponent<UIWidget>();
		if (component != null)
		{
			component.MakePixelPerfect();
		}
		if (t.GetComponent<UIAnchor>() == null && t.GetComponent<UIRoot>() == null)
		{
			t.localPosition = NGUITools.Round(t.localPosition);
			t.localScale = NGUITools.Round(t.localScale);
		}
		int i = 0;
		int childCount = t.childCount;
		while (i < childCount)
		{
			NGUITools.MakePixelPerfect(t.GetChild(i));
			i++;
		}
	}

	public static bool Save(string fileName, byte[] bytes)
	{
		if (!NGUITools.fileAccess)
		{
			return false;
		}
		string path = Application.persistentDataPath + "/" + fileName;
		if (bytes == null)
		{
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			return true;
		}
		FileStream fileStream = null;
		try
		{
			fileStream = File.Create(path);
		}
		catch (Exception ex)
		{
			NGUIDebug.Log(new object[]
			{
				ex.ToString()
			});
			return false;
		}
		fileStream.Write(bytes, 0, bytes.Length);
		fileStream.Close();
		return true;
	}

	public static byte[] Load(string fileName)
	{
		if (!NGUITools.fileAccess)
		{
			return null;
		}
		string path = Application.persistentDataPath + "/" + fileName;
		if (File.Exists(path))
		{
			return File.ReadAllBytes(path);
		}
		return null;
	}

	public static Color ApplyPMA(Color c)
	{
		if (c.a != 1f)
		{
			c.r *= c.a;
			c.g *= c.a;
			c.b *= c.a;
		}
		return c;
	}

	public static void MarkParentAsChanged(GameObject go)
	{
		UIRect[] componentsInChildren = go.GetComponentsInChildren<UIRect>();
		int i = 0;
		int num = componentsInChildren.Length;
		while (i < num)
		{
			componentsInChildren[i].ParentHasChanged();
			i++;
		}
	}

	[Obsolete("Use NGUIText.EncodeColor instead")]
	public static string EncodeColor(Color c)
	{
		return NGUIText.EncodeColor(c);
	}

	[Obsolete("Use NGUIText.ParseColor instead")]
	public static Color ParseColor(string text, int offset)
	{
		return NGUIText.ParseColor(text, offset);
	}

	[Obsolete("Use NGUIText.StripSymbols instead")]
	public static string StripSymbols(string text)
	{
		return NGUIText.StripSymbols(text);
	}

	public static T AddMissingComponent<T>(this GameObject go) where T : Component
	{
		T t = go.GetComponent<T>();
		if (t == null)
		{
			t = go.AddComponent<T>();
		}
		return t;
	}

	public static Vector3[] GetSides(this Camera cam)
	{
		return cam.GetSides(Mathf.Lerp(cam.nearClipPlane, cam.farClipPlane, 0.5f), null);
	}

	public static Vector3[] GetSides(this Camera cam, float depth)
	{
		return cam.GetSides(depth, null);
	}

	public static Vector3[] GetSides(this Camera cam, Transform relativeTo)
	{
		return cam.GetSides(Mathf.Lerp(cam.nearClipPlane, cam.farClipPlane, 0.5f), relativeTo);
	}

	public static Vector3[] GetSides(this Camera cam, float depth, Transform relativeTo)
	{
		Vector3 zero = Vector3.zero;
		zero.x = 0f;
		zero.y = 0.5f;
		zero.z = depth;
		NGUITools.mSides[0] = cam.ViewportToWorldPoint(zero);
		zero.x = 0.5f;
		zero.y = 1f;
		zero.z = depth;
		NGUITools.mSides[1] = cam.ViewportToWorldPoint(zero);
		zero.x = 1f;
		zero.y = 0.5f;
		zero.z = depth;
		NGUITools.mSides[2] = cam.ViewportToWorldPoint(zero);
		zero.x = 0.5f;
		zero.y = 0f;
		zero.z = depth;
		NGUITools.mSides[3] = cam.ViewportToWorldPoint(zero);
		if (relativeTo != null)
		{
			for (int i = 0; i < 4; i++)
			{
				NGUITools.mSides[i] = relativeTo.InverseTransformPoint(NGUITools.mSides[i]);
			}
		}
		return NGUITools.mSides;
	}

	public static Vector3[] GetWorldCorners(this Camera cam)
	{
		return cam.GetWorldCorners(Mathf.Lerp(cam.nearClipPlane, cam.farClipPlane, 0.5f), null);
	}

	public static Vector3[] GetWorldCorners(this Camera cam, float depth)
	{
		return cam.GetWorldCorners(depth, null);
	}

	public static Vector3[] GetWorldCorners(this Camera cam, Transform relativeTo)
	{
		return cam.GetWorldCorners(Mathf.Lerp(cam.nearClipPlane, cam.farClipPlane, 0.5f), relativeTo);
	}

	public static Vector3[] GetWorldCorners(this Camera cam, float depth, Transform relativeTo)
	{
		NGUITools.mSides[0] = cam.ViewportToWorldPoint(new Vector3(0f, 0f, depth));
		NGUITools.mSides[1] = cam.ViewportToWorldPoint(new Vector3(0f, 1f, depth));
		NGUITools.mSides[2] = cam.ViewportToWorldPoint(new Vector3(1f, 1f, depth));
		NGUITools.mSides[3] = cam.ViewportToWorldPoint(new Vector3(1f, 0f, depth));
		if (relativeTo != null)
		{
			for (int i = 0; i < 4; i++)
			{
				NGUITools.mSides[i] = relativeTo.InverseTransformPoint(NGUITools.mSides[i]);
			}
		}
		return NGUITools.mSides;
	}
}
