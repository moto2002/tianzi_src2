using System;
using System.Collections.Generic;

namespace com.snailgames.chosen.poolmanager
{
	public class CachePool
	{
		public Type oType;

		public List<CacheObject> spawned = new List<CacheObject>(16);

		public List<long> despawnedTime = new List<long>(16);

		public List<CacheObject> despawned = new List<CacheObject>(16);

		public T SpawnCache<T>() where T : CacheObject
		{
			if (this.despawned.Count > 0)
			{
				CacheObject cacheObject = this.despawned[0];
				this.despawned.RemoveAt(0);
				this.despawnedTime.RemoveAt(0);
				this.spawned.Add(cacheObject);
				cacheObject.OnSpawn();
				return cacheObject as T;
			}
			CacheObject cacheObject2 = Activator.CreateInstance<T>();
			this.spawned.Add(cacheObject2);
			cacheObject2.OnSpawn();
			return cacheObject2 as T;
		}

		public void DespawnCache(CacheObject oCache)
		{
			this.spawned.Remove(oCache);
			oCache.OnDespawn();
			this.despawned.Add(oCache);
			long ticks = DateTime.Now.Ticks;
			this.despawnedTime.Add(ticks);
		}

		public void SelfDestruct()
		{
			this.spawned.Clear();
			this.despawned.Clear();
			this.despawnedTime.Clear();
			this.oType = null;
		}

		public void OptimizeCachePool(long lTickNow, long lFreeTime)
		{
			for (int i = 0; i < this.despawnedTime.Count; i++)
			{
				long num = this.despawnedTime[i];
				if (lTickNow - num > lFreeTime)
				{
					CacheObject cacheObject = this.despawned[i];
					if (cacheObject != null)
					{
						cacheObject.OnDespawn();
					}
					this.despawnedTime.RemoveAt(i);
					this.despawned.RemoveAt(i);
					i--;
				}
			}
		}
	}
}
