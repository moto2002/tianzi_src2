using System;
using UnityEngine;

public class EffectWatcher : MonoBehaviour
{
	private NcDuplicator item;

	private NcDelayActive[] list1;

	private NcEffectAniBehaviour[] list2;

	private ParticleSystem[] list3;

	private Animation[] list4;

	private DestroyForTime[] list6;

	private bool init;

	private void Awake()
	{
		this.init = true;
		NcDuplicator componentInChildren = base.gameObject.GetComponentInChildren<NcDuplicator>();
		if (componentInChildren != null)
		{
			LogSystem.LogWarning(new object[]
			{
				base.gameObject.name,
				" NcDuplicator cannot be replayed."
			});
			return;
		}
		this.list1 = base.gameObject.GetComponentsInChildren<NcDelayActive>(true);
		this.list2 = base.gameObject.GetComponentsInChildren<NcEffectAniBehaviour>(true);
		this.list3 = base.gameObject.GetComponentsInChildren<ParticleSystem>(true);
		this.list4 = base.gameObject.GetComponentsInChildren<Animation>(true);
		this.list6 = base.gameObject.GetComponentsInChildren<DestroyForTime>(true);
	}

	public void ResetEffect()
	{
		if (!this.init)
		{
			this.Awake();
		}
		if (this.item != null)
		{
			LogSystem.LogWarning(new object[]
			{
				base.gameObject.name,
				" NcDuplicator cannot be replayed."
			});
			return;
		}
		if (this.list1 != null)
		{
			try
			{
				for (int i = 0; i < this.list1.Length; i++)
				{
					if (this.list1[i] != null)
					{
						this.list1[i].ResetAnimation();
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.LogError(new object[]
				{
					ex.ToString()
				});
			}
		}
		if (this.list2 != null)
		{
			try
			{
				for (int j = 0; j < this.list2.Length; j++)
				{
					if (this.list2[j] != null)
					{
						this.list2[j].ResetAnimation();
					}
				}
			}
			catch (Exception ex2)
			{
				LogSystem.LogError(new object[]
				{
					ex2.ToString()
				});
			}
		}
		if (this.list3 != null)
		{
			try
			{
				for (int k = 0; k < this.list3.Length; k++)
				{
					ParticleSystem particleSystem = this.list3[k];
					if (particleSystem != null)
					{
						particleSystem.Stop();
						particleSystem.Clear();
						particleSystem.time = 0f;
						particleSystem.Play();
					}
				}
			}
			catch (Exception ex3)
			{
				LogSystem.LogError(new object[]
				{
					ex3.ToString()
				});
			}
		}
		if (this.list4 != null)
		{
			try
			{
				for (int l = 0; l < this.list4.Length; l++)
				{
					Animation animation = this.list4[l];
					if (!(animation == null))
					{
						foreach (AnimationState animationState in animation)
						{
							animationState.time = 0f;
						}
						animation.Play();
					}
				}
			}
			catch (Exception ex4)
			{
				LogSystem.LogError(new object[]
				{
					ex4.ToString()
				});
			}
		}
		if (this.list6 != null)
		{
			try
			{
				for (int m = 0; m < this.list6.Length; m++)
				{
					DestroyForTime destroyForTime = this.list6[m];
					if (!(destroyForTime == null))
					{
						destroyForTime.Reset();
					}
				}
			}
			catch (Exception ex5)
			{
				LogSystem.LogError(new object[]
				{
					ex5.ToString()
				});
			}
		}
	}

	private void OnDestroy()
	{
		this.list1 = null;
		this.list2 = null;
		this.list3 = null;
		this.list4 = null;
		this.list6 = null;
	}
}
