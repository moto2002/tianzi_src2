using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace com.snailgames.chosen.poolmanager
{
	[AddComponentMenu("Path-o-logical/PoolManager/SpawnPool")]
	public sealed class SpawnPool : MonoBehaviour, IEnumerable, IList<Transform>, ICollection<Transform>, IEnumerable<Transform>
	{
		public string poolName = string.Empty;

		public bool matchPoolScale;

		public bool matchPoolLayer;

		public bool dontDestroyOnLoad;

		public bool logMessages;

		public List<PrefabPool> _perPrefabPoolOptions = new List<PrefabPool>();

		public Dictionary<object, bool> prefabsFoldOutStates = new Dictionary<object, bool>();

		[HideInInspector]
		public float maxParticleDespawnTime = 60f;

		public PrefabsDict prefabs = new PrefabsDict();

		public Dictionary<object, bool> _editorListItemStates = new Dictionary<object, bool>();

		private List<PrefabPool> _prefabPools = new List<PrefabPool>();

		internal List<Transform> spawned = new List<Transform>();

		private static int miEffectLayer = LayerMask.NameToLayer("Effect");

		private static int miUIEffectLayer = LayerMask.NameToLayer("UIEffect");

		private static int miModelLayer = LayerMask.NameToLayer("UIModel");

		public Transform group
		{
			get;
			private set;
		}

		public Dictionary<string, PrefabPool> prefabPools
		{
			get
			{
				Dictionary<string, PrefabPool> dictionary = new Dictionary<string, PrefabPool>();
				for (int i = 0; i < this._prefabPools.Count; i++)
				{
					PrefabPool prefabPool = this._prefabPools[i];
					dictionary[prefabPool.prefabGO.name] = prefabPool;
				}
				return dictionary;
			}
		}

		public Transform this[int index]
		{
			get
			{
				return this.spawned[index];
			}
			set
			{
				throw new NotImplementedException("Read-only.");
			}
		}

		public int Count
		{
			get
			{
				return this.spawned.Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			SpawnPool.GetEnumerator>c__Iterator8 getEnumerator>c__Iterator = new SpawnPool.GetEnumerator>c__Iterator8();
			getEnumerator>c__Iterator.<>f__this = this;
			return getEnumerator>c__Iterator;
		}

		bool ICollection<Transform>.Remove(Transform item)
		{
			throw new NotImplementedException();
		}

		private void Awake()
		{
			if (this.dontDestroyOnLoad)
			{
				UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			}
			this.group = base.transform;
			if (this.poolName == string.Empty)
			{
				this.poolName = this.group.name.Replace("Pool", string.Empty);
				this.poolName = this.poolName.Replace("(Clone)", string.Empty);
			}
			if (this.logMessages)
			{
				LogSystem.LogWarning(new object[]
				{
					string.Format("SpawnPool {0}: Initializing..", this.poolName)
				});
			}
			for (int i = 0; i < this._perPrefabPoolOptions.Count; i++)
			{
				PrefabPool prefabPool = this._perPrefabPoolOptions[i];
				if (prefabPool.prefab == null)
				{
					if (this.logMessages)
					{
						LogSystem.LogWarning(new object[]
						{
							string.Format("Initialization Warning: Pool '{0}' contains a PrefabPool with no prefab reference. Skipping.", this.poolName)
						});
					}
				}
				else
				{
					prefabPool.inspectorInstanceConstructor();
					this.CreatePrefabPool(prefabPool);
				}
			}
			CachePoolManager.Pools.Add(this);
		}

		public void ClearSpawnCache()
		{
			CachePoolManager.Pools.Remove(this);
			base.StopAllCoroutines();
			this.spawned.Clear();
			for (int i = 0; i < this._prefabPools.Count; i++)
			{
				PrefabPool prefabPool = this._prefabPools[i];
				Transform prefab = prefabPool.prefab;
				prefabPool.SelfDestruct();
			}
			this._prefabPools.Clear();
			Resources.UnloadUnusedAssets();
			GC.Collect();
		}

		public void ForceClearMemory()
		{
			for (int i = 0; i < this._prefabPools.Count; i++)
			{
				PrefabPool prefabPool = this._prefabPools[i];
				prefabPool.ProcessTimeOutInstance();
				if (prefabPool.despawned.Count == 0 && prefabPool.spawned.Count == 0)
				{
					Transform prefab = prefabPool.prefab;
					prefabPool.SelfDestruct();
					this._prefabPools.RemoveAt(i);
					i--;
				}
			}
		}

		public void RemoveEmptyPrefabPools()
		{
			for (int i = 0; i < this._prefabPools.Count; i++)
			{
				PrefabPool prefabPool = this._prefabPools[i];
				if (prefabPool != null)
				{
					prefabPool.ProcessTimeOutInstance();
					if (prefabPool.despawned.Count == 0 && prefabPool.spawned.Count == 0 && prefabPool.fTimeSecondWhenEmpty > 0)
					{
						Transform prefab = prefabPool.prefab;
						prefabPool.SelfDestruct();
						this._prefabPools.RemoveAt(i);
						i--;
					}
				}
			}
		}

		public static void CheckEffectWatcher(GameObject oEffect)
		{
			if (oEffect == null)
			{
				return;
			}
			if (oEffect.layer == SpawnPool.miEffectLayer || oEffect.layer == SpawnPool.miUIEffectLayer || oEffect.layer == SpawnPool.miModelLayer)
			{
				EffectWatcher effectWatcher = oEffect.GetComponent<EffectWatcher>();
				if (effectWatcher == null)
				{
					effectWatcher = oEffect.AddComponent<EffectWatcher>();
				}
				effectWatcher.ResetEffect();
			}
		}

		public void DestroyPool()
		{
			if (this.logMessages)
			{
				LogSystem.LogWarning(new object[]
				{
					string.Format("SpawnPool {0}: Destroying...", this.poolName)
				});
			}
			CachePoolManager.Pools.Remove(this);
			base.StopAllCoroutines();
			this.spawned.Clear();
			for (int i = 0; i < this._prefabPools.Count; i++)
			{
				PrefabPool prefabPool = this._prefabPools[i];
				prefabPool.SelfDestruct();
			}
			this._prefabPools.Clear();
			Resources.UnloadUnusedAssets();
			GC.Collect();
		}

		private void CreatePrefabPool(PrefabPool prefabPool)
		{
			if (this.GetPrefab(prefabPool.prefab) == null)
			{
				prefabPool.spawnPool = this;
				this._prefabPools.Add(prefabPool);
			}
			if (!prefabPool.preloaded)
			{
				if (this.logMessages)
				{
					UnityEngine.Debug.Log(string.Format("SpawnPool {0}: Preloading {1} {2}", this.poolName, prefabPool.preloadAmount, prefabPool.prefab.name));
				}
				prefabPool.PreloadInstances();
			}
		}

		public void Add(Transform item)
		{
			string message = "Use SpawnPool.Spawn() to properly add items to the pool.";
			throw new NotImplementedException(message);
		}

		public void Remove(Transform item)
		{
			string message = "Use Despawn() to properly manage items that should remain in the pool but be deactivated.";
			throw new NotImplementedException(message);
		}

		public Transform Spawn(Transform prefab, Vector3 pos, Quaternion rot)
		{
			int i = 0;
			Transform transform;
			while (i < this._prefabPools.Count)
			{
				PrefabPool prefabPool = this._prefabPools[i];
				if (prefabPool.prefabGO == prefab.gameObject)
				{
					transform = prefabPool.SpawnInstance(pos, rot);
					if (transform == null)
					{
						return null;
					}
					if (transform.parent != this.group)
					{
						transform.parent = this.group;
					}
					this.spawned.Add(transform);
					return transform;
				}
				else
				{
					i++;
				}
			}
			PrefabPool prefabPool2 = new PrefabPool(prefab);
			this.CreatePrefabPool(prefabPool2);
			transform = prefabPool2.SpawnInstance(pos, rot);
			transform.parent = this.group;
			this.spawned.Add(transform);
			return transform;
		}

		public bool IsCacheObject(GameObject obj)
		{
			return this.spawned != null && obj != null && this.spawned.Contains(obj.transform);
		}

		public bool IsCachePrefab(UnityEngine.Object obj)
		{
			GameObject gameObject = obj as GameObject;
			if (gameObject == null)
			{
				return false;
			}
			for (int i = 0; i < this._prefabPools.Count; i++)
			{
				PrefabPool prefabPool = this._prefabPools[i];
				if (prefabPool.prefabGO == gameObject)
				{
					return true;
				}
			}
			return false;
		}

		public bool IsInDespawn(Transform trans)
		{
			for (int i = 0; i < this._prefabPools.Count; i++)
			{
				PrefabPool prefabPool = this._prefabPools[i];
				if (prefabPool.despawned.Contains(trans))
				{
					return true;
				}
			}
			return false;
		}

		private void DespawnTree(Transform xform)
		{
			for (int i = xform.childCount - 1; i >= 0; i--)
			{
				Transform child = xform.GetChild(i);
				if (!(child == null))
				{
					this.DespawnTree(child);
				}
			}
			if (this.IsSpawned(xform))
			{
				Vector3 localEulerAngles = xform.localEulerAngles;
				if (this != null)
				{
					xform.parent = base.transform;
				}
				xform.localEulerAngles = localEulerAngles;
				this.DespawnObject(xform);
			}
		}

		private void DespawnObject(Transform xform)
		{
			bool flag = false;
			for (int i = 0; i < this._prefabPools.Count; i++)
			{
				PrefabPool prefabPool = this._prefabPools[i];
				if (prefabPool.spawned.Contains(xform))
				{
					flag = prefabPool.DespawnInstance(xform);
					break;
				}
				if (prefabPool.despawned.Contains(xform))
				{
					if (this.logMessages)
					{
						LogSystem.LogWarning(new object[]
						{
							string.Format("SpawnPool {0}: {1} has already been despawned. You cannot despawn something more than once!", this.poolName, xform.name)
						});
					}
					return;
				}
			}
			if (!flag)
			{
				if (this.logMessages)
				{
					LogSystem.LogWarning(new object[]
					{
						string.Format("SpawnPool {0}: {1} not found in SpawnPool", this.poolName, xform.name)
					});
				}
				return;
			}
			if (DelegateProxy.DespawnedProxy != null)
			{
				DelegateProxy.DespawnedProxy(xform);
			}
			this.spawned.Remove(xform);
		}

		public void Despawn(Transform xform)
		{
			if (xform == null)
			{
				return;
			}
			this.DespawnTree(xform);
		}

		private bool IsSpawned(Transform instance)
		{
			return !instance.CompareTag("Untagged") && this.spawned.Contains(instance);
		}

		private Transform GetPrefab(Transform prefab)
		{
			for (int i = 0; i < this._prefabPools.Count; i++)
			{
				PrefabPool prefabPool = this._prefabPools[i];
				if (prefabPool.prefabGO == null && this.logMessages)
				{
					LogSystem.LogWarning(new object[]
					{
						string.Format("SpawnPool {0}: PrefabPool.prefabGO is null", this.poolName)
					});
				}
				if (prefabPool.prefabGO == prefab.gameObject)
				{
					return prefabPool.prefab;
				}
			}
			return null;
		}

		public override string ToString()
		{
			List<string> list = new List<string>();
			foreach (Transform current in this.spawned)
			{
				list.Add(current.name);
			}
			return string.Join(", ", list.ToArray());
		}

		public bool Contains(Transform item)
		{
			string message = "Use IsSpawned(Transform instance) instead.";
			throw new NotImplementedException(message);
		}

		public void CopyTo(Transform[] array, int arrayIndex)
		{
			this.spawned.CopyTo(array, arrayIndex);
		}

		[DebuggerHidden]
		public IEnumerator<Transform> GetEnumerator()
		{
			SpawnPool.<GetEnumerator>c__Iterator9 <GetEnumerator>c__Iterator = new SpawnPool.<GetEnumerator>c__Iterator9();
			<GetEnumerator>c__Iterator.<>f__this = this;
			return <GetEnumerator>c__Iterator;
		}

		public int IndexOf(Transform item)
		{
			throw new NotImplementedException();
		}

		public void Insert(int index, Transform item)
		{
			throw new NotImplementedException();
		}

		public void RemoveAt(int index)
		{
			throw new NotImplementedException();
		}

		public void Clear()
		{
			throw new NotImplementedException();
		}
	}
}
