using System;
using UnityEngine;

public class AnimationProxyInfo : ScriptableObject
{
	[SerializeField]
	public AnimationProxy.AnimationInfo[] mAnimations;

	[SerializeField]
	public AnimationProxy.AnimationInfo mMainClip;

	public string aniID;

	public AnimationProxy.AnimationInfo GetAnimationInfo(string animationName)
	{
		for (int i = 0; i < this.mAnimations.Length; i++)
		{
			if (animationName == this.mAnimations[i].strName)
			{
				return this.mAnimations[i];
			}
		}
		return null;
	}
}
