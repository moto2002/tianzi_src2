using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SceneIDComparisionData
{
	[Serializable]
	public class SpecialPoint
	{
		public Vector3 pos;

		public string spriteName;

		public string targetPosName;

		public string entryLv;

		public int lbSize;

		public int anchor;
	}

	public int serverRegionId;

	public string regionMapName;

	public string regionId;

	public int rotation;

	public int pickupDistance;

	public List<SceneIDComparisionData.SpecialPoint> listSpecialPoints;
}
