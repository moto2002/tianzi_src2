using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectEnable : MonoBehaviour
{
	private string mstrObj = string.Empty;

	private string mstrInfo = string.Empty;

	private List<GameObject> moCurHide = new List<GameObject>();

	private void Start()
	{
	}

	private bool SetObjHide(string name)
	{
		GameObject gameObject = GameObject.Find(name);
		if (gameObject != null)
		{
			this.moCurHide.Add(gameObject);
			gameObject.SetActive(false);
			return true;
		}
		return false;
	}

	private void SetObjShow(string name)
	{
		GameObject gameObject = this.GetHideobjByName(name);
		if (gameObject == null)
		{
			gameObject = GameObject.Find(name);
		}
		if (gameObject != null)
		{
			gameObject.SetActive(true);
			this.RemoveHideObjByName(name);
		}
	}

	private GameObject GetHideobjByName(string strName)
	{
		for (int i = 0; i < this.moCurHide.Count; i++)
		{
			if (strName == this.moCurHide[i].name)
			{
				return this.moCurHide[i];
			}
		}
		return null;
	}

	private bool RemoveHideObjByName(string strName)
	{
		for (int i = 0; i < this.moCurHide.Count; i++)
		{
			if (strName == this.moCurHide[i].name)
			{
				this.moCurHide.RemoveAt(i);
				return true;
			}
		}
		return false;
	}
}
