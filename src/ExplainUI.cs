using System;
using UnityEngine;

public class ExplainUI : MonoBehaviour
{
	private const float ORIGIN_UI_HEIGHT = 720f;

	private const float ORIGIN_UI_WIDTH = 1280f;

	private GameObject explainUI;

	private UIPanel panel;

	private Camera uiCamera;

	private Action mCallFunc;

	private float Timer = 3f;

	private void Start()
	{
		string path = AssetFileUtils.StringBuilder(new object[]
		{
			"Local/Prefabs/UI/",
			Language.LanguageVersion.ToString(),
			"/ExplainPanel"
		});
		GameObject original = Resources.Load(path) as GameObject;
		this.explainUI = (UnityEngine.Object.Instantiate(original, Vector3.zero, Quaternion.identity) as GameObject);
		if (this.explainUI != null && this.uiCamera != null)
		{
			this.panel = this.explainUI.GetComponent<UIPanel>();
			this.panel.alpha = 0f;
			this.explainUI.transform.parent = this.uiCamera.transform;
			this.explainUI.transform.localPosition = Vector3.zero;
			this.explainUI.transform.localScale = Vector3.one;
		}
	}

	private void Awake()
	{
		GameObject gameObject = GameObject.FindWithTag("UICamera");
		if (gameObject == null)
		{
			return;
		}
		this.uiCamera = gameObject.GetComponent<Camera>();
		if (this.uiCamera == null)
		{
			LogSystem.LogWarning(new object[]
			{
				"UICamera is not find"
			});
			return;
		}
		if (this.uiCamera.gameObject.GetComponent<UICamera>() == null)
		{
			this.uiCamera.gameObject.AddComponent<UICamera>();
		}
		if ((float)ResolutionConstrain.Instance.width * 1f / (float)ResolutionConstrain.Instance.height >= 1.77777779f)
		{
			this.uiCamera.orthographicSize = 720f / (float)ResolutionConstrain.Instance.height;
		}
		else
		{
			this.uiCamera.orthographicSize = 1280f / (float)ResolutionConstrain.Instance.width;
		}
	}

	public void SetCallBack(Action action)
	{
		this.mCallFunc = action;
	}

	private void Update()
	{
		this.Timer -= Time.deltaTime;
		if (this.panel != null)
		{
			this.panel.alpha += 0.03f;
		}
		if (this.Timer < 0f)
		{
			this.Dispose();
		}
	}

	public void Dispose()
	{
		try
		{
			UnityEngine.Object.Destroy(base.gameObject);
			UnityEngine.Object.Destroy(this.explainUI);
			this.uiCamera = null;
		}
		catch (Exception ex)
		{
			LogSystem.LogError(new object[]
			{
				"ExplainUI : ",
				ex.ToString()
			});
		}
		if (this.mCallFunc != null)
		{
			this.mCallFunc();
		}
	}
}
