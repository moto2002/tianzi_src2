using System;
using UnityEngine;

public class UIGridSubItem : MonoBehaviour
{
	public UIGridItem oEventReciever;

	public UIGrid mGrid;

	public int mIndex;

	private void OnClick()
	{
		if (this.mGrid != null && this.oEventReciever != null)
		{
			this.mGrid.OnClickItem(this.oEventReciever, base.gameObject);
		}
	}

	private void OnDoubleClick()
	{
		if (this.mGrid != null && this.oEventReciever != null)
		{
			this.mGrid.OnDoubleClickItem(this.oEventReciever, base.gameObject);
		}
	}

	private void OnPress(bool isdown)
	{
		if (this.mGrid != null && this.oEventReciever != null)
		{
			this.mGrid.OnPressItem(this.oEventReciever, base.gameObject, isdown);
		}
	}

	private void OnDestroy()
	{
		this.oEventReciever = null;
		this.mGrid = null;
	}
}
