using System;
using UnityEngine;

[AddComponentMenu("NGUI/Tween/Tween Scale")]
public class TweenScale : UITweener
{
	public Vector3 from = Vector3.one;

	public Vector3 to = Vector3.one;

	public bool updateTable;

	private Transform mTrans;

	private UITable mTable;

	private static Vector3 ZeroVec3 = Vector3.one * 0.001f;

	public Transform cachedTransform
	{
		get
		{
			if (this.mTrans == null)
			{
				this.mTrans = base.transform;
			}
			return this.mTrans;
		}
	}

	public Vector3 value
	{
		get
		{
			return this.cachedTransform.localScale;
		}
		set
		{
			this.cachedTransform.localScale = value;
		}
	}

	[Obsolete("Use 'value' instead")]
	public Vector3 scale
	{
		get
		{
			return this.value;
		}
		set
		{
			this.value = value;
		}
	}

	protected override void OnUpdate(float factor, bool isFinished)
	{
		if (this.cachedTransform != null)
		{
			this.value = this.from * (1f - factor) + this.to * factor;
			if (this.updateTable)
			{
				if (this.mTable == null)
				{
					this.mTable = NGUITools.FindInParents<UITable>(base.gameObject);
					if (this.mTable == null)
					{
						this.updateTable = false;
						return;
					}
				}
				this.mTable.repositionNow = true;
			}
		}
	}

	public static TweenScale Begin(GameObject go, float duration, Vector3 scale)
	{
		TweenScale tweenScale = UITweener.Begin<TweenScale>(go, duration);
		tweenScale.from = tweenScale.value;
		tweenScale.to = scale;
		if (tweenScale.from.Equals(Vector3.zero))
		{
			tweenScale.from = TweenScale.ZeroVec3;
		}
		if (tweenScale.to.Equals(Vector3.zero))
		{
			tweenScale.to = TweenScale.ZeroVec3;
		}
		if (duration <= 0f)
		{
			tweenScale.Sample(1f, true);
			tweenScale.enabled = false;
		}
		return tweenScale;
	}

	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	[ContextMenu("Assume value of 'From'")]
	private void SetCurrentValueToStart()
	{
		this.value = this.from;
	}

	[ContextMenu("Assume value of 'To'")]
	private void SetCurrentValueToEnd()
	{
		this.value = this.to;
	}

	private new void OnDestroy()
	{
		this.mTrans = null;
		this.mTable = null;
	}
}
