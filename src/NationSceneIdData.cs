using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NationSceneIdData
{
	[Serializable]
	public class Data
	{
		public int sceneId;

		public string transferInfo;

		public bool bMapScene;

		public Vector3 flagPos = default(Vector3);

		public string flagSprite = string.Empty;

		public Vector3 guildNamePos = default(Vector3);

		public bool IsTargetSceneValid(int targetSceneId, out int transferPrice)
		{
			if (string.IsNullOrEmpty(this.transferInfo))
			{
				transferPrice = 0;
				return false;
			}
			int num = 10000;
			bool flag = false;
			string[] array = this.transferInfo.Split(new char[]
			{
				','
			});
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split(new char[]
				{
					'$'
				});
				if (array2.Length != 2)
				{
					LogSystem.LogWarning(new object[]
					{
						"transferInfo member count should be 2"
					});
					break;
				}
				int num2 = AssetFileUtils.IntParse(array2[0], 0);
				if (num2 == 0)
				{
					num = AssetFileUtils.IntParse(array2[1], 0);
					flag = true;
				}
				if (targetSceneId == num2)
				{
					transferPrice = AssetFileUtils.IntParse(array2[1], 0);
					return true;
				}
			}
			if (flag)
			{
				transferPrice = num;
				return true;
			}
			transferPrice = 0;
			return false;
		}
	}

	public class TransferParm
	{
		public int transferPrice;

		public int targetSceneId;
	}

	public int nationId;

	public string nationName;

	public List<NationSceneIdData.Data> dataDic;
}
