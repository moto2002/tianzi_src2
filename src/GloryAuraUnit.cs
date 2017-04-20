using System;
using System.Collections.Generic;

[Serializable]
public class GloryAuraUnit : IComparer<GloryAuraUnit>
{
	public string id;

	public string group;

	public int col;

	public string title;

	public int row;

	public int lv;

	public string name;

	public string desc;

	public string path;

	public int miCooldownType;

	public int Compare(GloryAuraUnit data1, GloryAuraUnit data2)
	{
		if (data1.row == data2.row)
		{
			return data1.col.CompareTo(data2.col);
		}
		return data1.row.CompareTo(data2.row);
	}
}
