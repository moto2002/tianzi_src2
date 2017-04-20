using System;
using System.Collections.Generic;

[Serializable]
public struct EffectBatch
{
	public int ID;

	public List<int> levelBatch;

	public static EffectBatch Null = new EffectBatch(-1);

	public EffectBatch(int id)
	{
		this.ID = id;
		this.levelBatch = new List<int>();
	}
}
