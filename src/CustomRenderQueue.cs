using System;
using System.Collections.Generic;
using UnityEngine;

public class CustomRenderQueue : MonoBehaviour
{
	private List<Renderer> rendererList = new List<Renderer>();

	private GameObject mGameObject;

	private UIWidget mTarget;

	private int frontOrBack = -1;

	private int lastRendererQueue;

	private bool needClipWithPanel;

	private UIPanel mClipPanel;

	private bool Open;

	private static List<CustomRenderQueue> mRenderQueues = new List<CustomRenderQueue>();

	private void Awake()
	{
		CustomRenderQueue.mRenderQueues.Add(this);
	}

	public static void UpdateRendererQueue(Transform oTrans)
	{
		if (oTrans == null)
		{
			return;
		}
		for (int i = CustomRenderQueue.mRenderQueues.Count - 1; i >= 0; i--)
		{
			CustomRenderQueue customRenderQueue = CustomRenderQueue.mRenderQueues[i];
			if (customRenderQueue == null)
			{
				CustomRenderQueue.mRenderQueues.RemoveAt(i);
			}
			else if (customRenderQueue.gameObject.activeInHierarchy)
			{
				if (customRenderQueue.transform.IsChildOf(oTrans))
				{
					customRenderQueue.UpdateRendererQueue();
				}
			}
		}
	}

	private void Start()
	{
		this.OnApplicationFocus();
	}

	private void OnEnable()
	{
		if (this.Open)
		{
			this.RefreshTargetRenderer(this.mTarget, this.frontOrBack, true);
		}
	}

	private void LateUpdate()
	{
		this.OnApplicationFocus();
	}

	private void OnValidate()
	{
		this.OnApplicationFocus();
	}

	public void OnApplicationFocus()
	{
		if (null != this.mTarget && null != this.mTarget.panel && this.needClipWithPanel)
		{
			int nameID = Shader.PropertyToID("_WorldToPanel");
			int nameID2 = Shader.PropertyToID("_MinX");
			int nameID3 = Shader.PropertyToID("_MinY");
			int nameID4 = Shader.PropertyToID("_MaxX");
			int nameID5 = Shader.PropertyToID("_MaxY");
			float x = this.mTarget.panel.mMin.x;
			float y = this.mTarget.panel.mMin.y;
			float x2 = this.mTarget.panel.mMax.x;
			float y2 = this.mTarget.panel.mMax.y;
			Matrix4x4 worldToLocal = this.mTarget.panel.worldToLocal;
			for (int i = 0; i < this.rendererList.Count; i++)
			{
				if (this.rendererList[i] != null)
				{
					string text = this.rendererList[i].material.shader.name;
					if (!text.Contains(" ClipWithPanel"))
					{
						text = this.rendererList[i].material.shader.name + " ClipWithPanel";
						Shader shader = Shader.Find(text);
						if (shader != null)
						{
							this.rendererList[i].material.shader = shader;
						}
					}
					this.rendererList[i].material.SetMatrix(nameID, worldToLocal);
					this.rendererList[i].material.SetFloat(nameID2, x);
					this.rendererList[i].material.SetFloat(nameID3, y);
					this.rendererList[i].material.SetFloat(nameID4, x2);
					this.rendererList[i].material.SetFloat(nameID5, y2);
				}
			}
		}
		else if (null != this.mClipPanel && this.needClipWithPanel)
		{
			int nameID6 = Shader.PropertyToID("_WorldToPanel");
			float x3 = this.mClipPanel.mMin.x;
			float y3 = this.mClipPanel.mMin.y;
			float x4 = this.mClipPanel.mMax.x;
			float y4 = this.mClipPanel.mMax.y;
			Matrix4x4 worldToLocal2 = this.mClipPanel.worldToLocal;
			for (int j = 0; j < this.rendererList.Count; j++)
			{
				if (this.rendererList[j] != null && this.rendererList[j].material != null)
				{
					string text2 = this.rendererList[j].material.shader.name;
					if (!text2.Contains(" ClipWithPanel"))
					{
						text2 = this.rendererList[j].material.shader.name + " ClipWithPanel";
						Shader shader2 = Shader.Find(text2);
						if (shader2 != null)
						{
							this.rendererList[j].material.shader = shader2;
						}
					}
					this.rendererList[j].material.SetMatrix(nameID6, worldToLocal2);
					this.rendererList[j].material.SetFloat("_MinX", x3);
					this.rendererList[j].material.SetFloat("_MinY", y3);
					this.rendererList[j].material.SetFloat("_MaxX", x4);
					this.rendererList[j].material.SetFloat("_MaxY", y4);
				}
			}
		}
		else if (this.mTarget != null)
		{
			int nameID7 = Shader.PropertyToID("_WorldToPanel");
			float value = -9999f;
			float value2 = -9999f;
			float value3 = 9999f;
			float value4 = 9999f;
			Matrix4x4 identity = Matrix4x4.identity;
			for (int k = 0; k < this.rendererList.Count; k++)
			{
				if (this.rendererList[k] != null && this.rendererList[k].material != null)
				{
					string text3 = this.rendererList[k].material.shader.name;
					if (!text3.Contains(" ClipWithPanel"))
					{
						text3 = this.rendererList[k].material.shader.name + " ClipWithPanel";
						Shader shader3 = Shader.Find(text3);
						if (shader3 != null)
						{
							this.rendererList[k].material.shader = shader3;
						}
					}
					this.rendererList[k].material.SetMatrix(nameID7, identity);
					this.rendererList[k].material.SetFloat("_MinX", value);
					this.rendererList[k].material.SetFloat("_MinY", value2);
					this.rendererList[k].material.SetFloat("_MaxX", value3);
					this.rendererList[k].material.SetFloat("_MaxY", value4);
				}
			}
		}
	}

	public void RefreshTargetRenderer()
	{
		if (this.mTarget == null)
		{
			return;
		}
		this.RefreshTargetRenderer(this.mTarget, this.frontOrBack, true);
	}

	public void RefreshTargetRenderer(UIPanel clipPanel, int customQueue, bool clipWithPanel)
	{
		this.mGameObject = base.gameObject;
		if (this.mGameObject != null)
		{
			this.rendererList.Clear();
			Renderer[] componentsInChildren = this.mGameObject.GetComponentsInChildren<Renderer>(true);
			int num = componentsInChildren.Length;
			for (int i = 0; i < num; i++)
			{
				if (componentsInChildren[i] != null && componentsInChildren[i].material != null && !this.rendererList.Contains(componentsInChildren[i]))
				{
					this.rendererList.Add(componentsInChildren[i]);
				}
			}
			ParticleSystem[] componentsInChildren2 = this.mGameObject.GetComponentsInChildren<ParticleSystem>(true);
			num = componentsInChildren2.Length;
			for (int j = 0; j < num; j++)
			{
				Renderer component = componentsInChildren2[j].GetComponent<Renderer>();
				if (component != null && component.material != null && !this.rendererList.Contains(component))
				{
					this.rendererList.Add(component);
				}
			}
		}
		this.mClipPanel = clipPanel;
		this.frontOrBack = customQueue;
		this.needClipWithPanel = clipWithPanel;
	}

	public void RefreshTargetRenderer(UIWidget tagertWidget, int customQueue, bool clipWithPanel)
	{
		this.mGameObject = base.gameObject;
		if (this.mGameObject != null)
		{
			this.rendererList.Clear();
			Renderer[] componentsInChildren = this.mGameObject.GetComponentsInChildren<Renderer>(true);
			int num = componentsInChildren.Length;
			for (int i = 0; i < num; i++)
			{
				if (componentsInChildren[i] != null && componentsInChildren[i].material != null && !this.rendererList.Contains(componentsInChildren[i]))
				{
					this.rendererList.Add(componentsInChildren[i]);
				}
			}
			ParticleSystem[] componentsInChildren2 = this.mGameObject.GetComponentsInChildren<ParticleSystem>(true);
			num = componentsInChildren2.Length;
			for (int j = 0; j < num; j++)
			{
				Renderer component = componentsInChildren2[j].GetComponent<Renderer>();
				if (component != null && component.material != null && !this.rendererList.Contains(component))
				{
					this.rendererList.Add(component);
				}
			}
		}
		this.mTarget = tagertWidget;
		this.frontOrBack = customQueue;
		this.needClipWithPanel = clipWithPanel;
	}

	public void ChangeClipWithPanelShader(GameObject role, UIPanel clipPanel, bool clipWithPanel = true)
	{
		if (role == null || clipPanel == null)
		{
			return;
		}
		this.mClipPanel = clipPanel;
		this.needClipWithPanel = clipWithPanel;
		Shader shader = Shader.Find("Effect/ModelClipWithPanel");
		if (shader == null)
		{
			LogSystem.LogWarning(new object[]
			{
				"shader not found"
			});
			return;
		}
		this.rendererList.Clear();
		Renderer[] componentsInChildren = role.GetComponentsInChildren<Renderer>(true);
		int num = componentsInChildren.Length;
		for (int i = 0; i < num; i++)
		{
			if (componentsInChildren[i] != null && componentsInChildren[i].material != null && !this.rendererList.Contains(componentsInChildren[i]))
			{
				this.rendererList.Add(componentsInChildren[i]);
			}
		}
		int nameID = Shader.PropertyToID("_WorldToPanel");
		float x = clipPanel.mMin.x;
		float y = clipPanel.mMin.y;
		float x2 = clipPanel.mMax.x;
		float y2 = clipPanel.mMax.y;
		Matrix4x4 worldToLocal = clipPanel.worldToLocal;
		for (int j = 0; j < this.rendererList.Count; j++)
		{
			if (this.rendererList[j] != null && this.rendererList[j].material != null)
			{
				this.rendererList[j].material.shader = shader;
				this.rendererList[j].material.SetMatrix(nameID, worldToLocal);
				this.rendererList[j].material.SetFloat("_MinX", x);
				this.rendererList[j].material.SetFloat("_MinY", y);
				this.rendererList[j].material.SetFloat("_MaxX", x2);
				this.rendererList[j].material.SetFloat("_MaxY", y2);
			}
		}
	}

	public void UpdateRendererQueue()
	{
		if (this.mTarget == null || this.mTarget.drawCall == null)
		{
			return;
		}
		int num = this.mTarget.drawCall.renderQueue;
		num += this.frontOrBack;
		if (this.lastRendererQueue != num)
		{
			this.lastRendererQueue = num;
			if (this.rendererList == null || this.rendererList.Count == 0)
			{
				return;
			}
			if (this.mTarget.panel != null && this.needClipWithPanel)
			{
				int nameID = Shader.PropertyToID("_WorldToPanel");
				float x = this.mTarget.panel.mMin.x;
				float y = this.mTarget.panel.mMin.y;
				float x2 = this.mTarget.panel.mMax.x;
				float y2 = this.mTarget.panel.mMax.y;
				Matrix4x4 worldToLocal = this.mTarget.panel.worldToLocal;
				for (int i = 0; i < this.rendererList.Count; i++)
				{
					if (this.rendererList[i] != null)
					{
						this.rendererList[i].material.renderQueue = this.lastRendererQueue;
						this.rendererList[i].material.SetMatrix(nameID, worldToLocal);
						this.rendererList[i].material.SetFloat("_MinX", x);
						this.rendererList[i].material.SetFloat("_MinY", y);
						this.rendererList[i].material.SetFloat("_MaxX", x2);
						this.rendererList[i].material.SetFloat("_MaxY", y2);
					}
				}
			}
			else
			{
				for (int j = 0; j < this.rendererList.Count; j++)
				{
					if (this.rendererList[j] != null)
					{
						this.rendererList[j].material.renderQueue = this.lastRendererQueue;
					}
				}
			}
		}
	}

	private void OnDestroy()
	{
		CustomRenderQueue.mRenderQueues.Remove(this);
		this.mGameObject = null;
		this.mTarget = null;
		this.mClipPanel = null;
		this.rendererList.Clear();
	}
}
