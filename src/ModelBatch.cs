using System;
using System.Collections.Generic;

[Serializable]
public class ModelBatch
{
	public string modelPath;

	public int ID;

	public ModelAssetType assetType;

	public int batch;

	public int noShadowBatch;

	public List<int> effectList;

	public static ModelBatch NullModel = new ModelBatch(-1);

	public ModelBatch(int id)
	{
		this.modelPath = string.Empty;
		this.ID = id;
		this.assetType = ModelAssetType.NONE;
		this.batch = 0;
		this.noShadowBatch = 0;
		this.effectList = new List<int>();
	}
}
