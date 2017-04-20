using System;
using System.Collections.Generic;

[Serializable]
public class SoulStoneSuitCfg
{
	public string strSuitId = string.Empty;

	public int nJob;

	public List<SoulStoneSuitConfig> dicExSuit = new List<SoulStoneSuitConfig>();
}
