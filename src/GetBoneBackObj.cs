using System;
using UnityEngine;

[Serializable]
public class GetBoneBackObj : MonoBehaviour
{
	public Transform mBoneBack;

	private void Start()
	{
		if (this.mBoneBack == null)
		{
			LogSystem.LogWarning(new object[]
			{
				"no boneNode in this Horse!!!" + base.transform.name
			});
		}
	}
}
