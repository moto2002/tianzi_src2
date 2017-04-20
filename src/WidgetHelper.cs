using System;
using UnityEngine;

public class WidgetHelper
{
	private static void OnWidgetLoaded(UIWidget[] spList, OnUIWidgetAtlasAllLoaded onAllLoaded, GameObject oGo, params object[] args)
	{
		if (spList == null || oGo == null)
		{
			return;
		}
		for (int i = 0; i < spList.Length; i++)
		{
			UIWidget uIWidget = spList[i];
			if (!(uIWidget == null))
			{
				if (uIWidget.CheckWaitLoadingAtlas())
				{
					return;
				}
			}
		}
		WidgetHelper.ClearSprite(spList);
		if (onAllLoaded != null)
		{
			onAllLoaded(oGo, args);
		}
	}

	private static void ClearSprite(UIWidget[] spList)
	{
		for (int i = 0; i < spList.Length; i++)
		{
			UIWidget x = spList[i];
			if (x == null)
			{
			}
		}
	}

	public static void LoadPrefabUISprite(GameObject oGo)
	{
		if (oGo == null)
		{
			return;
		}
		UIWidget[] componentsInChildren = oGo.GetComponentsInChildren<UIWidget>(true);
		if (componentsInChildren == null)
		{
			return;
		}
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			UIWidget uIWidget = componentsInChildren[i];
			if (!(uIWidget == null))
			{
				uIWidget.CheckLoadAtlas();
			}
		}
	}
}
