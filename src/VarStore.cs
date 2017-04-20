using System;
using System.Collections.Generic;
using UnityEngine;

public class VarStore
{
	private List<object> mParams = new List<object>(8);

	private static int miMaxStores = 20;

	private static List<VarStore> mVarStores = new List<VarStore>();

	public object this[int tkey]
	{
		get
		{
			return this.GetParams(tkey);
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	public int GetParamsCount()
	{
		return this.mParams.Count;
	}

	public object GetParams(int iIndex)
	{
		if (iIndex >= 0 && iIndex < this.mParams.Count)
		{
			return this.mParams[iIndex];
		}
		return null;
	}

	public object PopParams()
	{
		int count = this.mParams.Count;
		if (count > 0)
		{
			object result = this.mParams[count - 1];
			this.mParams.RemoveAt(count - 1);
			return result;
		}
		return null;
	}

	public void PushParams(object oData)
	{
		this.mParams.Add(oData);
	}

	public void PushParams(UnityEngine.Object oData)
	{
		this.mParams.Add(oData);
	}

	public void Collect()
	{
		this.Clear();
		VarStore.CollectStore(this);
	}

	private void Clear()
	{
		this.mParams.Clear();
	}

	public static VarStore CreateVarStore()
	{
		if (VarStore.mVarStores.Count > 0)
		{
			VarStore result = VarStore.mVarStores[0];
			VarStore.mVarStores.RemoveAt(0);
			return result;
		}
		return new VarStore();
	}

	private static void CollectStore(VarStore varStore)
	{
		if (varStore != null)
		{
			return;
		}
		if (VarStore.mVarStores.Count > VarStore.miMaxStores)
		{
			VarStore.mVarStores.RemoveAt(0);
		}
		VarStore.mVarStores.Add(varStore);
	}

	public static VarStore operator +(VarStore lo, bool bValue)
	{
		lo.mParams.Add(bValue);
		return lo;
	}

	public static VarStore operator +(VarStore lo, byte byValue)
	{
		lo.mParams.Add(byValue);
		return lo;
	}

	public static VarStore operator +(VarStore lo, short sValue)
	{
		lo.mParams.Add(sValue);
		return lo;
	}

	public static VarStore operator +(VarStore lo, int iValue)
	{
		lo.mParams.Add(iValue);
		return lo;
	}

	public static VarStore operator +(VarStore lo, long lValue)
	{
		lo.mParams.Add(lValue);
		return lo;
	}

	public static VarStore operator +(VarStore lo, float fValue)
	{
		lo.mParams.Add(fValue);
		return lo;
	}

	public static VarStore operator +(VarStore lo, double dValue)
	{
		lo.mParams.Add(dValue);
		return lo;
	}

	public static VarStore operator +(VarStore lo, string strValue)
	{
		lo.mParams.Add(strValue);
		return lo;
	}

	public static VarStore operator +(VarStore lo, object oValue)
	{
		lo.mParams.Add(oValue);
		return lo;
	}

	public static VarStore operator +(VarStore lo, UnityEngine.Object oValue)
	{
		lo.mParams.Add(oValue);
		return lo;
	}
}
