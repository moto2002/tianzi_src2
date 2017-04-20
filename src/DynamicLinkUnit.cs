using System;
using UnityEngine;

public class DynamicLinkUnit
{
	private float localorient;

	private Vector3 dir = Vector3.zero;

	public DynamicUnit mDynamic;

	private bool isMainUint;

	private bool genRipple;

	private bool needSampleHeight;

	public DynamicLinkUnit(DynamicUnit unit)
	{
		this.mDynamic = unit;
		this.isMainUint = unit.isMainUint;
		this.genRipple = unit.genRipple;
		this.needSampleHeight = unit.needSampleHeight;
		this.mDynamic.isMainUint = false;
		this.mDynamic.genRipple = false;
		this.mDynamic.needSampleHeight = false;
	}

	public Vector3 GetPosition(float y)
	{
		Quaternion rotation = Quaternion.Euler(0f, this.localorient / 0.0174532924f + y, 0f);
		return rotation * this.dir;
	}

	public void SetPositionAndOrient(Vector3 position, float orient)
	{
		this.localorient = orient;
		this.dir = position - Vector3.zero;
	}

	public void Init()
	{
		if (this.mDynamic == null)
		{
			return;
		}
		if (this.mDynamic.destroyed)
		{
			return;
		}
		this.mDynamic.isMainUint = false;
		this.mDynamic.genRipple = false;
		this.mDynamic.needSampleHeight = false;
	}

	public void Update(Vector3 position, Quaternion rotation, bool isMove)
	{
		if (this.mDynamic == null)
		{
			return;
		}
		if (this.mDynamic.destroyed)
		{
			return;
		}
		Vector3 eulerAngles = rotation.eulerAngles;
		this.mDynamic.position = position + this.GetPosition(eulerAngles.y);
		this.mDynamic.rotation = Quaternion.Euler(0f, eulerAngles.y + this.localorient / 0.0174532924f, 0f);
		this.mDynamic.moveListener(isMove);
	}

	public void Remove()
	{
		if (this.mDynamic == null)
		{
			return;
		}
		if (this.mDynamic.destroyed)
		{
			return;
		}
		this.mDynamic.isMainUint = this.isMainUint;
		this.mDynamic.genRipple = this.genRipple;
		this.mDynamic.needSampleHeight = this.needSampleHeight;
		if (this.mDynamic.mDynState == DynamicState.LINK_PARENT_CHILD)
		{
			this.mDynamic.mDynState = DynamicState.LINK_PARENT;
		}
		else
		{
			this.mDynamic.mDynState = DynamicState.NULL;
		}
	}
}
