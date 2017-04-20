using System;
using System.Collections.Generic;

public class DictionaryEx<TKey, TValue> : Dictionary<TKey, TValue>
{
	public List<TKey> mList = new List<TKey>();

	public new TValue this[TKey tkey]
	{
		get
		{
			return base[tkey];
		}
		set
		{
			if (this.ContainsKey(tkey))
			{
				base[tkey] = value;
			}
			else
			{
				this.Add(tkey, value);
			}
		}
	}

	public DictionaryEx()
	{
	}

	public DictionaryEx(IEqualityComparer<TKey> comparer) : base(comparer)
	{
	}

	public new void Add(TKey tkey, TValue tvalue)
	{
		this.mList.Add(tkey);
		base.Add(tkey, tvalue);
	}

	public new bool Remove(TKey tkey)
	{
		this.mList.Remove(tkey);
		return base.Remove(tkey);
	}

	public bool RemoveAt(int index)
	{
		TKey key = this.GetKey(index);
		return key != null && this.mList.Remove(key) && base.Remove(key);
	}

	public void Sort()
	{
		this.mList.Sort();
	}

	public void Sort(Comparison<TKey> comparison)
	{
		this.mList.Sort(comparison);
	}

	public void Sort(IComparer<TKey> comparer)
	{
		this.mList.Sort(comparer);
	}

	public void Sort(int index, int count, IComparer<TKey> comparer)
	{
		this.mList.Sort(index, count, comparer);
	}

	public TValue GetValue(int index)
	{
		if (this.mList != null && index > -1 && index < this.mList.Count)
		{
			TKey tKey = this.mList[index];
			TValue result;
			if (tKey != null && this.TryGetValue(tKey, out result))
			{
				return result;
			}
		}
		return default(TValue);
	}

	public TKey GetKey(int index)
	{
		if (index < 0)
		{
			return default(TKey);
		}
		if (this.mList == null || index > this.mList.Count - 1)
		{
			return default(TKey);
		}
		return this.mList[index];
	}

	public new bool ContainsKey(TKey tkey)
	{
		return this.mList.Contains(tkey);
	}

	public new void Clear()
	{
		this.mList.Clear();
		base.Clear();
	}
}
