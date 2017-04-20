using com.snailgames.chosen.structs;
using System;
using UnityEngine;

public class AssetInfoVoList : ScriptableObject
{
	[SerializeField]
	public AssetInfo[] assetInfos;

	[SerializeField]
	public CommonInfo[] commonInfos;
}
