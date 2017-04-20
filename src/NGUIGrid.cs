using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/NGUIGrid")]
public class NGUIGrid : UIWidgetContainer
{
	public enum Arrangement
	{
		Horizontal,
		Vertical
	}

	public enum Sorting
	{
		None,
		Alphabetic,
		Horizontal,
		Vertical,
		Custom
	}

	public delegate void OnReposition();

	public NGUIGrid.Arrangement arrangement;

	public NGUIGrid.Sorting sorting;

	public UIWidget.Pivot pivot;

	public int maxPerLine;

	public float cellWidth = 200f;

	public float cellHeight = 200f;

	public bool animateSmoothly;

	public bool hideInactive = true;

	public bool keepWithinPanel;

	public NGUIGrid.OnReposition onReposition;

	[HideInInspector, SerializeField]
	private bool sorted;

	protected bool mReposition;

	protected UIPanel mPanel;

	protected bool mInitDone;

	public bool repositionNow
	{
		set
		{
			if (value)
			{
				this.mReposition = true;
				base.enabled = true;
			}
		}
	}

	protected virtual void Init()
	{
		this.mInitDone = true;
		this.mPanel = NGUITools.FindInParents<UIPanel>(base.gameObject);
	}

	protected virtual void Start()
	{
		if (!this.mInitDone)
		{
			this.Init();
		}
		bool flag = this.animateSmoothly;
		this.animateSmoothly = false;
		this.Reposition();
		this.animateSmoothly = flag;
		base.enabled = false;
	}

	protected virtual void Update()
	{
		if (this.mReposition)
		{
			this.Reposition();
		}
		base.enabled = false;
	}

	public static int SortByName(Transform a, Transform b)
	{
		return string.Compare(a.name, b.name);
	}

	public static int SortHorizontal(Transform a, Transform b)
	{
		return a.localPosition.x.CompareTo(b.localPosition.x);
	}

	public static int SortVertical(Transform a, Transform b)
	{
		return b.localPosition.y.CompareTo(a.localPosition.y);
	}

	protected virtual void Sort(BetterList<Transform> list)
	{
		list.Sort(new BetterList<Transform>.CompareFunc(NGUIGrid.SortByName));
	}

	[ContextMenu("Execute")]
	public virtual void Reposition()
	{
		if (Application.isPlaying && !this.mInitDone && NGUITools.GetActive(this))
		{
			this.mReposition = true;
			return;
		}
		if (!this.mInitDone)
		{
			this.Init();
		}
		this.mReposition = false;
		Transform transform = base.transform;
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		if (this.sorting != NGUIGrid.Sorting.None || this.sorted)
		{
			BetterList<Transform> betterList = new BetterList<Transform>();
			for (int i = 0; i < transform.childCount; i++)
			{
				Transform child = transform.GetChild(i);
				if (child && (!this.hideInactive || NGUITools.GetActive(child.gameObject)))
				{
					betterList.Add(child);
				}
			}
			if (this.sorting == NGUIGrid.Sorting.Alphabetic)
			{
				betterList.Sort(new BetterList<Transform>.CompareFunc(NGUIGrid.SortByName));
			}
			else if (this.sorting == NGUIGrid.Sorting.Horizontal)
			{
				betterList.Sort(new BetterList<Transform>.CompareFunc(NGUIGrid.SortHorizontal));
			}
			else if (this.sorting == NGUIGrid.Sorting.Vertical)
			{
				betterList.Sort(new BetterList<Transform>.CompareFunc(NGUIGrid.SortVertical));
			}
			else
			{
				this.Sort(betterList);
			}
			int j = 0;
			int size = betterList.size;
			while (j < size)
			{
				Transform transform2 = betterList[j];
				if (NGUITools.GetActive(transform2.gameObject) || !this.hideInactive)
				{
					float z = transform2.localPosition.z;
					Vector3 vector = (this.arrangement != NGUIGrid.Arrangement.Horizontal) ? new Vector3(this.cellWidth * (float)num2, -this.cellHeight * (float)num, z) : new Vector3(this.cellWidth * (float)num, -this.cellHeight * (float)num2, z);
					if (this.animateSmoothly && Application.isPlaying)
					{
						SpringPosition.Begin(transform2.gameObject, vector, 15f).updateScrollView = true;
					}
					else
					{
						transform2.localPosition = vector;
					}
					num3 = Mathf.Max(num3, num);
					num4 = Mathf.Max(num4, num2);
					if (++num >= this.maxPerLine && this.maxPerLine > 0)
					{
						num = 0;
						num2++;
					}
				}
				j++;
			}
		}
		else
		{
			for (int k = 0; k < transform.childCount; k++)
			{
				Transform child2 = transform.GetChild(k);
				if (NGUITools.GetActive(child2.gameObject) || !this.hideInactive)
				{
					float z2 = child2.localPosition.z;
					Vector3 vector2 = (this.arrangement != NGUIGrid.Arrangement.Horizontal) ? new Vector3(this.cellWidth * (float)num2, -this.cellHeight * (float)num, z2) : new Vector3(this.cellWidth * (float)num, -this.cellHeight * (float)num2, z2);
					if (this.animateSmoothly && Application.isPlaying)
					{
						SpringPosition.Begin(child2.gameObject, vector2, 15f).updateScrollView = true;
					}
					else
					{
						child2.localPosition = vector2;
					}
					num3 = Mathf.Max(num3, num);
					num4 = Mathf.Max(num4, num2);
					if (++num >= this.maxPerLine && this.maxPerLine > 0)
					{
						num = 0;
						num2++;
					}
				}
			}
		}
		if (this.pivot != UIWidget.Pivot.TopLeft)
		{
			Vector2 pivotOffset = NGUIMath.GetPivotOffset(this.pivot);
			float num5;
			float num6;
			if (this.arrangement == NGUIGrid.Arrangement.Horizontal)
			{
				num5 = Mathf.Lerp(0f, (float)num3 * this.cellWidth, pivotOffset.x);
				num6 = Mathf.Lerp((float)(-(float)num4) * this.cellHeight, 0f, pivotOffset.y);
			}
			else
			{
				num5 = Mathf.Lerp(0f, (float)num4 * this.cellWidth, pivotOffset.x);
				num6 = Mathf.Lerp((float)(-(float)num3) * this.cellHeight, 0f, pivotOffset.y);
			}
			for (int l = 0; l < transform.childCount; l++)
			{
				Transform child3 = transform.GetChild(l);
				if (NGUITools.GetActive(child3.gameObject) || !this.hideInactive)
				{
					SpringPosition component = child3.GetComponent<SpringPosition>();
					if (component != null)
					{
						SpringPosition expr_44A_cp_0 = component;
						expr_44A_cp_0.target.x = expr_44A_cp_0.target.x - num5;
						SpringPosition expr_45F_cp_0 = component;
						expr_45F_cp_0.target.y = expr_45F_cp_0.target.y - num6;
					}
					else
					{
						Vector3 localPosition = child3.localPosition;
						localPosition.x -= num5;
						localPosition.y -= num6;
						child3.localPosition = localPosition;
					}
				}
			}
		}
		if (this.keepWithinPanel && this.mPanel != null)
		{
			this.mPanel.ConstrainTargetToBounds(transform, true);
		}
		if (this.onReposition != null)
		{
			this.onReposition();
		}
	}
}
