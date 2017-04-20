using com.snailgames.chosen.scene.library;
using com.snailgames.chosen.scene.library.loader;
using com.snailgames.chosen.scene.library.parser;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameScene
{
	public delegate void SceneLoadCompleBack();

	public delegate void SceneLoadingBack();

	public int sceneID;

	public GameScene.SceneLoadCompleBack sceneLoadCompleListener;

	public GameScene.SceneLoadingBack sceneLoadingListener;

	public float loadProgress;

	public bool loadSceneComplate;

	private int sceneLoadUnitTick;

	public static bool isPlaying = false;

	public float randomCode;

	private float updateInterval = 0.2f;

	private float lastInterval = Time.realtimeSinceStartup;

	private int frames;

	public float fps = 30f;

	public float time;

	private int tick;

	private int viewRect = 1;

	public int targetFrameRate = 25;

	private Dictionary<string, Region> regionsMap = new Dictionary<string, Region>();

	public List<Region> regions;

	public Vector3 eyePos = Vector3.zero;

	public int curRegionX;

	public int curRegionY;

	public Dictionary<int, GameObjectUnit> unitsMap;

	public List<GameObjectUnit> units;

	public int unitCount;

	public int unitIdCount;

	private TerrainConfig _terrainConfig;

	private Dictionary<string, Tile> tilesMap;

	public List<Tile> tiles;

	public Tile curTile;

	public static bool dontCullUnit = true;

	public static bool isEditor = false;

	private static MapPath _mapPath;

	private List<Camera> cameras = new List<Camera>();

	public GameObjectUnit mainUnit;

	public Camera mainCamera;

	private static int mainCameraCullingMask = -1;

	private static float[,] _heights;

	public static GameScene mainScene = null;

	public static List<GameScene> sceneStack = new List<GameScene>();

	public static GameObject staticInsRoot;

	public RenderTexture mainRTT;

	public static bool SampleMode = false;

	public bool loadFromAssetBund;

	public global::TerrainData terrainData;

	public bool statisticMode = true;

	public bool cacheMode;

	public List<string> preloadUnitAssetPathList = new List<string>();

	public List<int> preloadUnitAssetTypeList = new List<int>();

	public List<Asset> preloadUnitAssetList = new List<Asset>();

	public List<GameObjectUnit> staticUnitcCache = new List<GameObjectUnit>();

	public List<DynamicUnit> dynamicUnitsCache = new List<DynamicUnit>();

	public Dictionary<int, GameObjectUnit> curSceneUnitsMap = new Dictionary<int, GameObjectUnit>();

	public List<ParserBase> parsers = new List<ParserBase>();

	public List<string> postEffectsList = new List<string>();

	private Dictionary<string, byte[]> peConfig = new Dictionary<string, byte[]>();

	public static bool lightmapCorrectOn = false;

	private static Root _rootIns;

	public long lightDataLength = -1L;

	private bool destroyed;

	private Texture2D screenshots;

	public static bool postEffectEnable = true;

	private bool breset;

	private bool peConfigLoaded;

	private int dynamicUnitStartCount = 500000;

	private List<DynamicUnit> removeDynUnits = new List<DynamicUnit>();

	private GameObject sunLightObj;

	public int lightmapCount;

	private bool loadTerrainTextureComplate;

	public bool readCompalte;

	private Vector4 worldSpaceLightPosition;

	private bool _oldLightmapCorrectOn;

	public bool needPreload = true;

	private bool preloadComplate;

	private int preloadMaxCountPer = 1;

	private int loadIndex;

	private string regKey = string.Empty;

	public Vector3 viewDir = Vector3.zero;

	private int curPreLoadCount;

	private int perTerBakeCount = 1;

	private int terBakeCount;

	private int preloadTick;

	private int preloadInterval = 1;

	private int preloadMaxTick;

	private float preloadMaxProgress = 0.8f;

	private float progressInc = 0.02f;

	private int loadComplateWaitTick;

	private int sceneMaxLoadUnitTick = 80;

	private int oldSceneLoadUnitTick;

	private Vector3 lastPos;

	private int visibleTilePerFrame = 1;

	private int visTileCount;

	public int visibleDynamicUnitCount;

	public int visibleStaticUnitCount;

	private int visibleDynaUnitPerFrame;

	private int visibleStaticUnitPerFrame;

	private int hideStaticUnitPerFrame;

	public static int maxDynaUnit = 50;

	public int visibleStaticTypeCount;

	public List<GameObjectUnit> dynamicUnits = new List<GameObjectUnit>();

	public Dictionary<string, int> staticTypeMap = new Dictionary<string, int>();

	public int useMaterialsCount;

	public Dictionary<string, Material> materials = new Dictionary<string, Material>();

	private float dx;

	private float dz;

	private List<GameObjectUnit> shadowUnits = new List<GameObjectUnit>();

	public int maxCastShadowsUnitCount = 8;

	public bool enableDynamicGrid;

	private string smkey = string.Empty;

	public float waterHeight;

	public static Root root
	{
		get
		{
			if (GameScene._rootIns == null)
			{
				GameScene.staticInsRoot = new GameObject("staticInsRoot");
				GameScene._rootIns = GameScene.staticInsRoot.AddComponent<Root>();
			}
			return GameScene._rootIns;
		}
	}

	public TerrainConfig terrainConfig
	{
		get
		{
			return this._terrainConfig;
		}
	}

	public MapPath mapPath
	{
		get
		{
			return GameScene._mapPath;
		}
		set
		{
			GameScene._mapPath = value;
		}
	}

	public float[,] heights
	{
		get
		{
			return GameScene._heights;
		}
		set
		{
			GameScene._heights = value;
		}
	}

	public GameScene(bool createNew = false)
	{
		this.randomCode = UnityEngine.Random.value;
		this.regions = new List<Region>();
		this.regionsMap = new Dictionary<string, Region>();
		this.tilesMap = new Dictionary<string, Tile>();
		this.tiles = new List<Tile>();
		this.unitsMap = new Dictionary<int, GameObjectUnit>();
		this.units = new List<GameObjectUnit>();
		GameScene.mainScene = this;
		GameScene.isPlaying = Application.isPlaying;
		GameScene.isEditor = Application.isEditor;
		this._terrainConfig = new TerrainConfig();
		if (!GameScene.isEditor)
		{
			Terrainmapping.mapsCount = 4;
		}
		GameScene.sceneStack.Add(this);
		if (createNew)
		{
			this.readCompalte = true;
			this.loadTerrainTextureComplate = true;
		}
		this.postEffectsList.Add("TopGradualColor");
		this.postEffectsList.Add("Vignetting");
		this.postEffectsList.Add("WaterDistortion");
	}

	public void Destroy()
	{
		if (this.mainCamera != null)
		{
			this.mainCamera.cullingMask = GameScene.mainCameraCullingMask;
		}
		this.destroyed = true;
		if (GameScene.staticInsRoot != null)
		{
			UnityEngine.Object.DestroyImmediate(GameScene.staticInsRoot);
		}
		RenderSettings.skybox = null;
		if (this.mapPath != null)
		{
			this.mapPath.Clear();
		}
		this.removeDynUnits.Clear();
		this.shadowUnits.Clear();
		int i = 0;
		if (this.units != null)
		{
			while (this.units.Count > 0)
			{
				if (this.units[0].isStatic)
				{
					if (this.units != null && this.units.Count > 0)
					{
						this.RemoveUnit(this.units[0], true, true);
					}
				}
				else if (this.units != null && this.units.Count > 0)
				{
					this.RemoveDynamicUnit(this.units[0] as DynamicUnit, true, true);
				}
			}
			this.units.Clear();
			this.units = null;
			this.dynamicUnits.Clear();
			this.dynamicUnits = null;
		}
		while (i < this.regions.Count)
		{
			this.regions[i].Destroy();
			i++;
		}
		this.regions.Clear();
		this.regions = null;
		while (this.tiles.Count > 0)
		{
			this.RemoveTile(this.tiles[0], true);
		}
		this.tiles = null;
		this.unitsMap.Clear();
		this.unitsMap = null;
		this.tilesMap.Clear();
		this.tilesMap = null;
		if (this.curSceneUnitsMap != null)
		{
			this.curSceneUnitsMap.Clear();
			this.curSceneUnitsMap = null;
		}
		if (this.sunLightObj != null)
		{
			if (GameScene.isPlaying)
			{
				DelegateProxy.GameDestory(this.sunLightObj);
			}
			else
			{
				DelegateProxy.DestroyObjectImmediate(this.sunLightObj);
			}
		}
		this._terrainConfig = null;
		this.preloadUnitAssetPathList.Clear();
		this.preloadUnitAssetPathList = null;
		this.preloadUnitAssetList.Clear();
		this.preloadUnitAssetList = null;
		this.preloadUnitAssetTypeList.Clear();
		this.preloadUnitAssetTypeList = null;
		this.cameras.Clear();
		this.cameras = null;
		this.mainCamera = null;
		this.lightDataLength = -1L;
		this.sceneLoadCompleListener = null;
		this.sceneLoadingListener = null;
		if (this.screenshots != null)
		{
			this.screenshots = null;
		}
		LightmapSettings.lightmaps = new LightmapData[0];
		if (GameScene.mainScene == this)
		{
			GameScene.mainScene = null;
		}
		GameScene.sceneStack.Remove(this);
		this.postEffectsList.Clear();
		this.postEffectsList = null;
		this.parsers.Clear();
		this.parsers = null;
		this.staticUnitcCache.Clear();
		this.staticUnitcCache = null;
		this.dynamicUnitsCache.Clear();
		this.dynamicUnitsCache = null;
		LightmapSettings.lightmaps = null;
		LightmapCorrection.Clear();
		AssetLibrary.RemoveAllAsset();
		Resources.UnloadUnusedAssets();
		GC.Collect();
	}

	public static GameScene FindScene(int sceneID)
	{
		int count = GameScene.sceneStack.Count;
		for (int i = 0; i < count; i++)
		{
			if (GameScene.sceneStack[i].sceneID == sceneID)
			{
				return GameScene.sceneStack[i];
			}
		}
		return null;
	}

	public Texture2D RenderPostEffectImage(List<string> peList = null)
	{
		return null;
	}

	public void ActivePostEffect(bool value)
	{
		if (!GameScene.isPlaying)
		{
			return;
		}
		if (this.breset && GameScene.postEffectEnable == value)
		{
			return;
		}
		GameScene.postEffectEnable = value;
		if (this.mainCamera == null)
		{
			return;
		}
		this.breset = true;
		this.peConfigLoaded = true;
		for (int i = 0; i < this.postEffectsList.Count; i++)
		{
			string text = this.postEffectsList[i];
			MonoBehaviour monoBehaviour = this.mainCamera.GetComponent(text) as MonoBehaviour;
			if (monoBehaviour == null)
			{
				Type type = Type.GetType(text);
				monoBehaviour = (this.mainCamera.gameObject.AddComponent(type) as MonoBehaviour);
			}
			if (monoBehaviour != null)
			{
				if (value && this.peConfig.ContainsKey(text))
				{
					monoBehaviour.enabled = true;
				}
				else
				{
					monoBehaviour.enabled = false;
				}
			}
		}
	}

	public void LoadPostEffectConfig()
	{
		if (this.peConfigLoaded)
		{
			return;
		}
		for (int i = 0; i < this.postEffectsList.Count; i++)
		{
			string text = this.postEffectsList[i];
			PEBase pEBase = this.mainCamera.GetComponent(text) as PEBase;
			if (pEBase == null)
			{
				Type type = Type.GetType(text);
				pEBase = (this.mainCamera.gameObject.AddComponent(type) as PEBase);
			}
			if (pEBase != null)
			{
				pEBase.enabled = false;
				if (this.peConfig.ContainsKey(text))
				{
					byte[] array = this.peConfig[text];
					if (array != null)
					{
						MemoryStream input = new MemoryStream(array);
						BinaryReader binaryReader = new BinaryReader(input);
						pEBase.enabled = binaryReader.ReadBoolean();
						if (pEBase.enabled)
						{
							int num = binaryReader.ReadInt32();
							for (int j = 0; j < num; j++)
							{
								int num2 = binaryReader.ReadInt32();
								string propertyName = binaryReader.ReadString();
								if (num2 == 0)
								{
									pEBase.material.SetInt(propertyName, binaryReader.ReadInt32());
								}
								else if (num2 == 1)
								{
									pEBase.material.SetFloat(propertyName, binaryReader.ReadSingle());
								}
								else if (num2 == 2)
								{
									pEBase.material.SetColor(propertyName, new Color(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle()));
								}
								else if (num2 == 3)
								{
									pEBase.material.SetVector(propertyName, new Vector4(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle()));
								}
								else if (num2 == 4)
								{
									Asset asset = AssetLibrary.Load(binaryReader.ReadString(), AssetType.Texture2D, LoadType.Type_Auto);
									if (asset != null && asset.texture2D != null)
									{
										pEBase.material.SetTexture(propertyName, asset.texture2D);
									}
								}
							}
							pEBase.LoadParams();
						}
					}
				}
				else
				{
					pEBase.enabled = false;
				}
			}
		}
		this.peConfigLoaded = true;
	}

	public void StaticBatchCombine()
	{
		StaticBatchingUtility.Combine(GameScene.staticInsRoot);
	}

	public void FirstRun()
	{
		if (GameScene.staticInsRoot == null)
		{
			GameScene.staticInsRoot = new GameObject("staticInsRoot");
			GameScene._rootIns = GameScene.staticInsRoot.AddComponent<Root>();
		}
	}

	public void AddCamera(Camera camera)
	{
		for (int i = 0; i < this.cameras.Count; i++)
		{
			if (this.cameras[i] == camera)
			{
				return;
			}
		}
		if (this.mainCamera == null)
		{
			this.mainCamera = camera;
		}
		camera.backgroundColor = this._terrainConfig.fogColor;
		this.cameras.Add(camera);
	}

	public bool IsValidForWalk(Vector3 worldPostion, int collisionSize)
	{
		return GameScene._mapPath != null && GameScene._mapPath.IsValidForWalk(worldPostion, collisionSize);
	}

	public bool IsValidForWalk(Vector3 worldPostion, int collisionSize, out int gridType)
	{
		if (GameScene._mapPath != null)
		{
			return GameScene._mapPath.IsValidForWalk(worldPostion, collisionSize, out gridType);
		}
		gridType = 0;
		return false;
	}

	public int getGridType(Vector3 worldPostion)
	{
		if (GameScene._mapPath != null)
		{
			int result = 0;
			if (GameScene._mapPath.IsValidForWalk(worldPostion, 1, out result))
			{
				return result;
			}
		}
		return 1;
	}

	public int getGridValue(Vector3 worldPostion)
	{
		if (GameScene._mapPath != null)
		{
			int num = 0;
			if (GameScene._mapPath.IsValidForWalk(worldPostion, 1, out num))
			{
				if ((num & 1) > 0)
				{
					return 1;
				}
				return 0;
			}
		}
		return 1;
	}

	public void UpdateWalkBlocker()
	{
	}

	public GameObjectUnit CreateEmptyUnit(int createID = -1)
	{
		if (createID < 0)
		{
			this.unitIdCount++;
			createID = this.unitCount;
		}
		GameObjectUnit gameObjectUnit;
		if (this.staticUnitcCache.Count > 0)
		{
			gameObjectUnit = this.staticUnitcCache[0];
			gameObjectUnit.destroyed = false;
			this.staticUnitcCache.RemoveAt(0);
		}
		else
		{
			gameObjectUnit = new GameObjectUnit(createID);
		}
		return gameObjectUnit;
	}

	public void RemoveEmptyUnit(GameObjectUnit unit)
	{
		if (!this.staticUnitcCache.Contains(unit))
		{
			this.staticUnitcCache.Add(unit);
		}
	}

	public GameObjectUnit CreateUnit(Vector3 pos, string prePath, bool isStatic = true)
	{
		this.unitIdCount++;
		GameObjectUnit gameObjectUnit = GameObjectUnit.Create(this, pos, this.unitIdCount, prePath);
		gameObjectUnit.name = "Unit_" + gameObjectUnit.createID;
		gameObjectUnit.isStatic = isStatic;
		UnityEngine.Object o = Resources.Load(prePath, typeof(UnityEngine.Object));
		GameObject gameObject = DelegateProxy.Instantiate(o) as GameObject;
		gameObject.transform.position = pos;
		gameObjectUnit.localScale = gameObject.transform.localScale;
		gameObjectUnit.rotation = gameObject.transform.rotation;
		gameObjectUnit.type = UnitType.GetType(gameObject.layer);
		gameObjectUnit.unitParser = UnitType.GenUnitParser(gameObjectUnit.type);
		gameObjectUnit.unitParser.unit = gameObjectUnit;
		gameObjectUnit.ins = gameObject;
		gameObjectUnit.insTrans = gameObject.transform;
		gameObjectUnit.ComputeTiles();
		gameObjectUnit.UpdateViewRange();
		DelegateProxy.DestroyObjectImmediate(gameObject);
		this.AddUnit(gameObjectUnit);
		return gameObjectUnit;
	}

	public GameObjectUnit CreateUnit(Vector3 pos, string prePath, Quaternion rotation, bool isStatic = true, UnitParser parser = null)
	{
		this.unitIdCount++;
		GameObjectUnit gameObjectUnit = GameObjectUnit.Create(this, pos, this.unitIdCount, prePath);
		gameObjectUnit.name = "Unit_" + gameObjectUnit.createID;
		gameObjectUnit.isStatic = isStatic;
		UnityEngine.Object o = Resources.Load(prePath, typeof(UnityEngine.Object));
		GameObject gameObject = DelegateProxy.Instantiate(o) as GameObject;
		gameObject.transform.position = pos;
		gameObjectUnit.localScale = gameObject.transform.localScale;
		gameObjectUnit.rotation = rotation;
		gameObjectUnit.type = UnitType.GetType(gameObject.layer);
		if (parser == null)
		{
			gameObjectUnit.unitParser = UnitType.GenUnitParser(gameObjectUnit.type);
		}
		else
		{
			gameObjectUnit.unitParser = parser;
		}
		gameObjectUnit.unitParser.unit = gameObjectUnit;
		gameObjectUnit.ins = gameObject;
		gameObjectUnit.insTrans = gameObject.transform;
		gameObjectUnit.ComputeTiles();
		gameObjectUnit.UpdateViewRange();
		DelegateProxy.DestroyObjectImmediate(gameObject);
		this.AddUnit(gameObjectUnit);
		return gameObjectUnit;
	}

	public DynamicUnit CreateDynamicUnit(Vector3 pos, string prePath, float radius, int uType = 0, float dynamiCullingDistance = -1f)
	{
		if (radius == 0f)
		{
			radius = 0.2f;
		}
		this.unitIdCount++;
		DynamicUnit dynamicUnit;
		if (this.dynamicUnitsCache.Count > 0)
		{
			dynamicUnit = this.dynamicUnitsCache[0];
			dynamicUnit.createID = this.unitIdCount + this.dynamicUnitStartCount;
			dynamicUnit.prePath = prePath;
			dynamicUnit.destroyed = false;
			dynamicUnit.isMainUint = false;
			dynamicUnit.willRemoved = false;
			dynamicUnit.scene = this;
			dynamicUnit.type = uType;
			dynamicUnit.isStatic = false;
			this.AddUnit(dynamicUnit);
			if (dynamiCullingDistance > 0f)
			{
				dynamicUnit.near = dynamiCullingDistance;
			}
			else
			{
				dynamicUnit.near = this.terrainConfig.dynamiCullingDistance;
			}
			dynamicUnit.far = dynamicUnit.near + 2f;
			this.dynamicUnitsCache.RemoveAt(0);
		}
		else
		{
			dynamicUnit = DynamicUnit.Create(this, pos, this.unitIdCount + this.dynamicUnitStartCount, prePath, radius, dynamiCullingDistance);
			dynamicUnit.isStatic = false;
			this.AddUnit(dynamicUnit);
			dynamicUnit.type = uType;
		}
		dynamicUnit.position = pos;
		dynamicUnit.position.y = this.SampleHeight(pos, true);
		return dynamicUnit;
	}

	public DynamicUnit CreateDynamicUnitImmediately(Vector3 pos, string prePath, float radius, int uType = 0, float dynamiCullingDistance = -1f)
	{
		DynamicUnit dynamicUnit = this.CreateDynamicUnit(pos, prePath, radius, uType, dynamiCullingDistance);
		float num = dynamicUnit.position.x - this.eyePos.x;
		float num2 = dynamicUnit.position.z - this.eyePos.z;
		dynamicUnit.viewDistance = Mathf.Sqrt(num * num + num2 * num2);
		if (dynamicUnit.position.y > 1E+08f)
		{
			dynamicUnit.position.y = this.SampleHeight(dynamicUnit.position, true);
		}
		if (dynamicUnit.viewDistance < dynamicUnit.far)
		{
			dynamicUnit.Visible();
		}
		return dynamicUnit;
	}

	public GameObjectUnit FindUnitInRegions(int createID)
	{
		int count = this.regions.Count;
		for (int i = 0; i < count; i++)
		{
			GameObjectUnit gameObjectUnit = this.regions[i].FindUint(createID);
			if (gameObjectUnit != null)
			{
				return gameObjectUnit;
			}
		}
		return null;
	}

	public GameObjectUnit FindUnitInTiles(int createID)
	{
		int count = this.tiles.Count;
		for (int i = 0; i < count; i++)
		{
			GameObjectUnit gameObjectUnit = this.tiles[i].FindUnit(createID);
			if (gameObjectUnit != null)
			{
				return gameObjectUnit;
			}
		}
		return null;
	}

	public GameObjectUnit FindUnit(string name)
	{
		if (!name.Contains("Unit_"))
		{
			return null;
		}
		int key;
		if (!GameScene.isPlaying)
		{
			string[] array = name.Split(new char[]
			{
				':'
			});
			key = int.Parse(array[array.Length - 1].Replace("Unit_", string.Empty));
		}
		else
		{
			key = int.Parse(name.Replace("Unit_", string.Empty));
		}
		if (this.unitsMap.ContainsKey(key))
		{
			return this.unitsMap[key];
		}
		return null;
	}

	public GameObjectUnit FindFirstUnit(int type)
	{
		int count = this.units.Count;
		for (int i = 0; i < count; i++)
		{
			if (this.units[i].type == type)
			{
				return this.units[i];
			}
		}
		return null;
	}

	public GameObjectUnit FindUnit(int cID)
	{
		if (this.unitsMap.ContainsKey(cID))
		{
			return this.unitsMap[cID];
		}
		return null;
	}

	public bool ContainUnit(GameObjectUnit unit)
	{
		return this.unitsMap.ContainsKey(unit.createID);
	}

	public void RemoveUnitInEditor(string name)
	{
		GameObjectUnit gameObjectUnit = this.FindUnit(name);
		if (gameObjectUnit == null)
		{
			return;
		}
		gameObjectUnit.ClearTiles();
		for (int i = 0; i < this.tiles.Count; i++)
		{
			this.tiles[i].RemoveUnit(gameObjectUnit);
		}
		if (gameObjectUnit.ins != null)
		{
			DelegateProxy.DestroyObjectImmediate(gameObjectUnit.ins);
		}
		this.unitsMap.Remove(gameObjectUnit.createID);
		this.units.Remove(gameObjectUnit);
		this.unitCount--;
	}

	public void RemoveUnitInEditor(GameObjectUnit unit)
	{
		if (unit == null)
		{
			return;
		}
		unit.ClearTiles();
		for (int i = 0; i < this.tiles.Count; i++)
		{
			this.tiles[i].RemoveUnit(unit);
		}
		if (unit.ins != null)
		{
			DelegateProxy.DestroyObjectImmediate(unit.ins);
		}
		this.unitsMap.Remove(unit.createID);
		this.units.Remove(unit);
		this.unitCount--;
	}

	public bool AddUnit(GameObjectUnit unit)
	{
		if (this.unitsMap.ContainsKey(unit.createID))
		{
			return false;
		}
		this.units.Add(unit);
		this.unitsMap.Add(unit.createID, unit);
		this.unitCount++;
		return true;
	}

	public void RemoveDynamicUnit(DynamicUnit unit, bool cache = true, bool immediately = false)
	{
		if (unit == null)
		{
			return;
		}
		if (unit.mDynState == DynamicState.LINK_PARENT || unit.mDynState == DynamicState.LINK_PARENT_CHILD)
		{
			unit.RemoveAllLinkDynamic();
		}
		if (!true)
		{
			unit.Stop();
			unit.willRemoved = true;
			this.removeDynUnits.Add(unit);
		}
		else
		{
			this.mapPath.SetDynamicCollision(unit.position, unit.collisionSize, true, 1);
			unit.Invisible();
			unit.Destroy();
			this.unitsMap.Remove(unit.createID);
			this.units.Remove(unit);
			this.unitCount--;
			unit.createID = -1;
			if (cache && !this.dynamicUnitsCache.Contains(unit))
			{
				this.dynamicUnitsCache.Add(unit);
			}
		}
	}

	public void RemoveUnit(GameObjectUnit unit, bool destroy = false, bool cache = false)
	{
		unit.Invisible();
		this.unitsMap.Remove(unit.createID);
		this.units.Remove(unit);
		if (destroy)
		{
			unit.Destroy();
		}
		this.unitCount--;
		if (cache && !this.staticUnitcCache.Contains(unit))
		{
			this.staticUnitcCache.Add(unit);
		}
	}

	public void AddTile(Tile tile)
	{
		if (this.tilesMap.ContainsKey(tile.key))
		{
			return;
		}
		this.tilesMap.Add(tile.key, tile);
		this.tiles.Add(tile);
	}

	public bool ContainTile(Tile tile)
	{
		return this.tilesMap.ContainsKey(tile.key);
	}

	public void RemoveTile(Tile tile, bool destroy = false)
	{
		if (this.tilesMap.ContainsKey(tile.key))
		{
			tile.Invisible();
			if (destroy)
			{
				tile.Destroy();
			}
			this.tilesMap.Remove(tile.key);
			this.tiles.Remove(tile);
		}
	}

	public Tile FindTile(string key)
	{
		if (this.tilesMap.ContainsKey(key))
		{
			return this.tilesMap[key];
		}
		return null;
	}

	public GameScene Read(byte[] bytes)
	{
		this.sceneLoadUnitTick = 0;
		this._terrainConfig = new TerrainConfig();
		MemoryStream memoryStream = new MemoryStream(bytes);
		BinaryReader binaryReader = new BinaryReader(memoryStream);
		long position = binaryReader.BaseStream.Position;
		if (binaryReader.ReadInt32() == 999999)
		{
			long num = binaryReader.ReadInt64();
			byte[] buffer = binaryReader.ReadBytes((int)num);
			MemoryStream memoryStream2 = new MemoryStream(buffer);
			MemoryStream memoryStream3 = new MemoryStream();
			StreamZip.Unzip(memoryStream2, memoryStream3);
			memoryStream = memoryStream3;
			memoryStream2.Dispose();
			binaryReader.Close();
			binaryReader = new BinaryReader(memoryStream);
			binaryReader.BaseStream.Position = 0L;
		}
		else
		{
			binaryReader.BaseStream.Position = position;
		}
		this.sceneID = binaryReader.ReadInt32();
		if (QualitySettings.vSyncCount == 1)
		{
			Application.targetFrameRate = 60;
		}
		else
		{
			Application.targetFrameRate = this.targetFrameRate;
		}
		this._terrainConfig.regionSize = binaryReader.ReadInt32();
		this._terrainConfig.tileSize = binaryReader.ReadInt32();
		this._terrainConfig.defaultTerrainHeight = binaryReader.ReadSingle();
		this._terrainConfig.maxTerrainHeight = binaryReader.ReadSingle();
		this._terrainConfig.cameraLookAt.x = binaryReader.ReadSingle();
		this._terrainConfig.cameraLookAt.y = binaryReader.ReadSingle();
		this._terrainConfig.cameraLookAt.z = binaryReader.ReadSingle();
		this._terrainConfig.cameraDistance = binaryReader.ReadSingle();
		position = binaryReader.BaseStream.Position;
		if (binaryReader.ReadInt32() == 10031)
		{
			this._terrainConfig.cameraRotationX = binaryReader.ReadSingle();
			this._terrainConfig.cameraRotationY = binaryReader.ReadSingle();
			this._terrainConfig.cameraRotationZ = binaryReader.ReadSingle();
		}
		else
		{
			binaryReader.BaseStream.Position = position;
		}
		position = binaryReader.BaseStream.Position;
		if (binaryReader.ReadInt32() == 10032)
		{
			RenderSettings.fog = binaryReader.ReadBoolean();
			int qualityLevel = QualitySettings.GetQualityLevel();
			if (qualityLevel == 1 || qualityLevel == 0)
			{
				RenderSettings.fog = false;
			}
			else
			{
				RenderSettings.fog = true;
			}
			RenderSettings.fogMode = (FogMode)binaryReader.ReadInt32();
			RenderSettings.fogColor = new Color(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
			RenderSettings.fogDensity = binaryReader.ReadSingle();
			RenderSettings.fogStartDistance = binaryReader.ReadSingle();
			RenderSettings.fogEndDistance = binaryReader.ReadSingle();
		}
		else
		{
			binaryReader.BaseStream.Position = position;
		}
		position = binaryReader.BaseStream.Position;
		if (binaryReader.ReadInt32() == 10012)
		{
			if (binaryReader.ReadInt32() == 1)
			{
				string path = binaryReader.ReadString();
				RenderSettings.skybox = (Resources.Load(path, typeof(Material)) as Material);
			}
		}
		else
		{
			binaryReader.BaseStream.Position = position;
		}
		position = binaryReader.BaseStream.Position;
		if (binaryReader.ReadInt32() == 13001)
		{
			this._terrainConfig.enableTerrain = binaryReader.ReadBoolean();
		}
		else
		{
			binaryReader.BaseStream.Position = position;
		}
		position = binaryReader.BaseStream.Position;
		if (binaryReader.ReadInt32() == 10041)
		{
			this._terrainConfig.weather = binaryReader.ReadInt32();
		}
		else
		{
			binaryReader.BaseStream.Position = position;
		}
		RenderSettings.ambientLight = new Color(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
		RenderSettings.ambientLight = new Color(1f, 1f, 1f, 1f);
		this.sunLightObj = new GameObject();
		Light light = this.sunLightObj.AddComponent<Light>();
		this.sunLightObj.name = "sunLight";
		light.type = LightType.Directional;
		light.color = new Color(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
		light.transform.rotation = new Quaternion(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
		light.intensity = binaryReader.ReadSingle();
		light.shadows = LightShadows.Hard;
		light.shadowStrength = binaryReader.ReadSingle();
		light.shadowBias = binaryReader.ReadSingle();
		light.shadowSoftness = binaryReader.ReadSingle();
		light.shadowSoftnessFade = binaryReader.ReadSingle();
		light.renderMode = LightRenderMode.ForcePixel;
		if (Application.isPlaying)
		{
			light.cullingMask = (~(1 << LayerMask.NameToLayer("Plant")) & ~(1 << LayerMask.NameToLayer("Builder")) & ~(1 << LayerMask.NameToLayer("Water")) & ~(1 << LayerMask.NameToLayer("Default")));
		}
		position = binaryReader.BaseStream.Position;
		if (binaryReader.ReadInt32() == 10010)
		{
			this._terrainConfig.waveScale = binaryReader.ReadSingle();
			this._terrainConfig.waveSpeed = new Vector4(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
			this._terrainConfig.horizonColor = new Color(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
			this._terrainConfig.defaultWaterHeight = binaryReader.ReadSingle();
			this._terrainConfig.waterVisibleDepth = binaryReader.ReadSingle();
			this._terrainConfig.waterDiffValue = binaryReader.ReadSingle();
			this._terrainConfig.colorControl = AssetLibrary.Load(binaryReader.ReadString(), AssetType.Texture2D, LoadType.Type_Resources).texture2D;
			this._terrainConfig.waterBumpMap = AssetLibrary.Load(binaryReader.ReadString(), AssetType.Texture2D, LoadType.Type_Resources).texture2D;
		}
		else
		{
			binaryReader.BaseStream.Position = position;
		}
		position = binaryReader.BaseStream.Position;
		if (binaryReader.ReadInt32() == 10017)
		{
			this._terrainConfig.waterSpecRange = binaryReader.ReadSingle();
			this._terrainConfig.waterSpecStrength = binaryReader.ReadSingle();
			this._terrainConfig.sunLightDir = new Vector4(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
		}
		else
		{
			binaryReader.BaseStream.Position = position;
		}
		this._terrainConfig.fogColor = new Color(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
		this._terrainConfig.startDistance = binaryReader.ReadSingle();
		this._terrainConfig.globalDensity = binaryReader.ReadSingle();
		this._terrainConfig.heightScale = binaryReader.ReadSingle();
		this._terrainConfig.height = binaryReader.ReadSingle();
		this._terrainConfig.fogIntensity.x = binaryReader.ReadSingle();
		this._terrainConfig.fogIntensity.y = binaryReader.ReadSingle();
		this._terrainConfig.fogIntensity.z = binaryReader.ReadSingle();
		this._terrainConfig.fogIntensity.w = binaryReader.ReadSingle();
		Shader.SetGlobalColor("_FogColor", this._terrainConfig.fogColor);
		Shader.SetGlobalVector("_FogParam", new Vector4(this._terrainConfig.startDistance, this._terrainConfig.globalDensity, this._terrainConfig.heightScale, this._terrainConfig.height));
		Shader.SetGlobalVector("_FogIntensity", this._terrainConfig.fogIntensity);
		position = binaryReader.BaseStream.Position;
		if (binaryReader.ReadInt32() == 10021)
		{
			this._terrainConfig.pointLightRangeMax = binaryReader.ReadSingle();
			this._terrainConfig.pointLightRangeMin = binaryReader.ReadSingle();
			this._terrainConfig.pointLightIntensity = binaryReader.ReadSingle();
		}
		else
		{
			binaryReader.BaseStream.Position = position;
		}
		position = binaryReader.BaseStream.Position;
		if (binaryReader.ReadInt32() == 10051)
		{
			this._terrainConfig.enablePointLight = binaryReader.ReadBoolean();
			this._terrainConfig.enablePointLight = false;
			this._terrainConfig.pointLightColor = new Color(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
		}
		else
		{
			binaryReader.BaseStream.Position = position;
		}
		position = binaryReader.BaseStream.Position;
		if (binaryReader.ReadInt32() == 10022)
		{
			this._terrainConfig.rolePointLightPostion = new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
			this._terrainConfig.rolePointLightColor = new Color(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
			this._terrainConfig.rolePointLightRange = binaryReader.ReadSingle();
			this._terrainConfig.rolePointLightIntensity = binaryReader.ReadSingle();
		}
		else
		{
			binaryReader.BaseStream.Position = position;
		}
		this.peConfig.Clear();
		position = binaryReader.BaseStream.Position;
		if (binaryReader.ReadInt32() == 70000)
		{
			int num2 = binaryReader.ReadInt32();
			for (int i = 0; i < num2; i++)
			{
				string key = binaryReader.ReadString();
				int count = binaryReader.ReadInt32();
				byte[] value = binaryReader.ReadBytes(count);
				this.peConfig.Add(key, value);
			}
		}
		else
		{
			binaryReader.BaseStream.Position = position;
		}
		position = binaryReader.BaseStream.Position;
		if (binaryReader.ReadInt32() == 70001)
		{
			this._terrainConfig.coolColor = new Color(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
			this._terrainConfig.warmColor = new Color(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
		}
		else
		{
			binaryReader.BaseStream.Position = position;
		}
		Shader.SetGlobalVector("coolColor", this._terrainConfig.coolColor);
		Shader.SetGlobalVector("warmColor", this._terrainConfig.warmColor);
		position = binaryReader.BaseStream.Position;
		if (binaryReader.ReadInt32() == 10017)
		{
			string text = binaryReader.ReadString();
			if (!text.Contains("Terrain"))
			{
				binaryReader.ReadSingle();
				binaryReader.ReadSingle();
				binaryReader.ReadSingle();
				binaryReader.ReadSingle();
				binaryReader.ReadSingle();
				binaryReader.ReadSingle();
				binaryReader.ReadSingle();
				binaryReader.ReadInt32();
			}
			else
			{
				binaryReader.BaseStream.Position = position;
				binaryReader.ReadInt32();
			}
		}
		else
		{
			binaryReader.BaseStream.Position = position;
		}
		binaryReader.ReadString();
		binaryReader.ReadSingle();
		binaryReader.ReadSingle();
		binaryReader.ReadSingle();
		binaryReader.ReadSingle();
		this._terrainConfig.tileCountPerSide = this._terrainConfig.regionSize / this._terrainConfig.tileSize;
		this._terrainConfig.tileCountPerRegion = this._terrainConfig.tileCountPerSide * this._terrainConfig.tileCountPerSide;
		this.unitIdCount = binaryReader.ReadInt32();
		this._terrainConfig.tileCullingDistance = binaryReader.ReadSingle();
		this._terrainConfig.unitCullingDistance = binaryReader.ReadSingle();
		this._terrainConfig.cullingBaseDistance = binaryReader.ReadSingle();
		position = binaryReader.BaseStream.Position;
		if (binaryReader.ReadInt32() == 10018)
		{
			this._terrainConfig.dynamiCullingDistance = binaryReader.ReadSingle();
		}
		else
		{
			binaryReader.BaseStream.Position = position;
		}
		position = binaryReader.BaseStream.Position;
		if (binaryReader.ReadInt32() == 10023)
		{
			this._terrainConfig.cullingAngleFactor = binaryReader.ReadSingle();
		}
		else
		{
			binaryReader.BaseStream.Position = position;
		}
		position = binaryReader.BaseStream.Position;
		if (binaryReader.ReadInt32() == 10024)
		{
			this._terrainConfig.viewAngleLodFactor = binaryReader.ReadSingle();
		}
		else
		{
			binaryReader.BaseStream.Position = position;
		}
		if (this._terrainConfig.cullingAngleFactor < 0.001f)
		{
			this._terrainConfig.cullingAngleFactor = 3f;
		}
		if (GameScene.isPlaying)
		{
			this._terrainConfig.tileCullingDistance = 75f;
			this._terrainConfig.cullingBaseDistance = 20.5f;
			this._terrainConfig.unitCullingDistance = 0.7f;
			this._terrainConfig.dynamiCullingDistance = 16f;
		}
		position = binaryReader.BaseStream.Position;
		if (binaryReader.ReadInt32() == 10003)
		{
			int iWidth = binaryReader.ReadInt32();
			int iHeight = binaryReader.ReadInt32();
			MapPath mapPath = new MapPath(this._terrainConfig.gridSize, iWidth, iHeight);
			mapPath.Read2(iWidth, iHeight, binaryReader);
		}
		else
		{
			binaryReader.BaseStream.Position = position;
		}
		position = binaryReader.BaseStream.Position;
		if (binaryReader.ReadInt32() == 20003)
		{
			int num3 = binaryReader.ReadInt32();
			for (int i = 0; i < num3; i++)
			{
				this.preloadUnitAssetTypeList.Add(binaryReader.ReadInt32());
				this.preloadUnitAssetPathList.Add(binaryReader.ReadString());
			}
		}
		else
		{
			binaryReader.BaseStream.Position = position;
		}
		position = binaryReader.BaseStream.Position;
		if (binaryReader.ReadInt32() == 20004)
		{
			int num4 = binaryReader.ReadInt32();
			for (int i = 0; i < num4; i++)
			{
				this.preloadUnitAssetTypeList.Add(3);
				this.preloadUnitAssetPathList.Add(binaryReader.ReadString());
			}
		}
		else
		{
			binaryReader.BaseStream.Position = position;
		}
		int num5 = binaryReader.ReadInt32();
		position = binaryReader.BaseStream.Position;
		if (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
		{
			if (binaryReader.ReadInt32() == 10008)
			{
				binaryReader.ReadInt32();
			}
			else
			{
				binaryReader.BaseStream.Position = position;
			}
		}
		this.lightmapCount = num5;
		for (int i = 0; i < num5; i++)
		{
			binaryReader.ReadString();
		}
		if (num5 > 0 && !GameScene.isPlaying)
		{
			light.enabled = false;
		}
		if (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
		{
			position = binaryReader.BaseStream.Position;
			if (binaryReader.ReadInt32() == 20011)
			{
				int num6 = binaryReader.ReadInt32();
				int num7 = binaryReader.ReadInt32();
				if (this.heights == null)
				{
					this.heights = new float[num6, num7];
				}
				for (int i = 0; i < num6; i++)
				{
					for (int j = 0; j < num7; j++)
					{
						float num8 = binaryReader.ReadSingle();
						this.heights[i, j] = num8;
					}
				}
			}
			else
			{
				binaryReader.BaseStream.Position = position;
			}
		}
		this.terrainData = new global::TerrainData(this._terrainConfig.sceneWidth, this._terrainConfig.sceneHeight, 4, this._terrainConfig.maxTerrainHeight, this._terrainConfig.defaultTerrainHeight);
		if (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
		{
			position = binaryReader.BaseStream.Position;
			if (binaryReader.ReadInt32() == 20005)
			{
				int num9 = binaryReader.ReadInt32();
				int num10 = binaryReader.ReadInt32();
				for (int i = 0; i < num9; i++)
				{
					for (int j = 0; j < num10; j++)
					{
						float num11 = binaryReader.ReadSingle();
						this.terrainData.heightmap[i, j] = num11;
					}
				}
			}
			else
			{
				binaryReader.BaseStream.Position = position;
			}
		}
		if (GameScene.isPlaying)
		{
			this.terrainData.Release();
			this.terrainData = null;
		}
		if (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
		{
			position = binaryReader.BaseStream.Position;
			int num12 = binaryReader.ReadInt32();
			if (num12 == 10005)
			{
				int iWidth2 = binaryReader.ReadInt32();
				int iHeight2 = binaryReader.ReadInt32();
				GameScene._mapPath = new MapPath(this._terrainConfig.gridSize, iWidth2, iHeight2);
				GameScene._mapPath.Read2(iWidth2, iHeight2, binaryReader);
			}
			else if (num12 == 10025)
			{
				int iWidth3 = binaryReader.ReadInt32();
				int iHeight3 = binaryReader.ReadInt32();
				GameScene._mapPath = new MapPath(this._terrainConfig.gridSize, iWidth3, iHeight3);
				GameScene._mapPath.Read(iWidth3, iHeight3, binaryReader);
			}
			else
			{
				binaryReader.BaseStream.Position = position;
			}
		}
		binaryReader.Close();
		memoryStream.Flush();
		Asset asset = AssetLibrary.Load("Scenes/" + this.sceneID + "/Prefabs/Root", AssetType.GameObject, LoadType.Type_ResMan);
		if (asset != null)
		{
			GameObject gameObject = asset.gameObject;
			if (gameObject != null)
			{
				GameScene.staticInsRoot = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
			}
			else
			{
				LogSystem.LogWarning(new object[]
				{
					"@@@ GameObject.Instantiate Root is null:" + this.sceneID
				});
			}
			if (GameScene.staticInsRoot == null)
			{
				LogSystem.LogWarning(new object[]
				{
					"@@@ staticInsRoot is null:" + this.sceneID
				});
			}
		}
		else
		{
			LogSystem.LogWarning(new object[]
			{
				"@@@ AssetLibrary.Load Root is null:" + this.sceneID
			});
		}
		Asset asset2 = AssetLibrary.Load("Textures/Terrain", AssetType.AssetBundle, LoadType.Type_Auto);
		if (asset2.loader is ResourceLoader)
		{
			this.loadTerrainTextureComplate = true;
		}
		else if (asset2.loader is AssetBundleLoader)
		{
			asset2.loadedListener = new Asset.LoadedListener(this.LoadedTerTex);
		}
		GameScene.mainScene = this;
		this.readCompalte = true;
		return this;
	}

	public void LoadedTerTex(Asset asset)
	{
		this.loadTerrainTextureComplate = true;
	}

	public void RequestPaths(Vector3 startPoint, Vector3 endPoint, int collisionSize, out List<Vector3> paths)
	{
		if (this.mapPath != null)
		{
			this.mapPath.RequestPaths(startPoint, endPoint, collisionSize, out paths);
		}
		else
		{
			paths = new List<Vector3>();
		}
	}

	public void UpdateShaderConstant()
	{
		if (GameScene.lightmapCorrectOn != this._oldLightmapCorrectOn)
		{
			if (GameScene.lightmapCorrectOn)
			{
				Shader.EnableKeyword("LightmapCorrectOn");
				Shader.DisableKeyword("LightmapCorrectOff");
			}
			else
			{
				Shader.DisableKeyword("LightmapCorrectOn");
				Shader.EnableKeyword("LightmapCorrectOff");
			}
			this._oldLightmapCorrectOn = GameScene.lightmapCorrectOn;
		}
		Shader.SetGlobalVector("coolColor", this._terrainConfig.coolColor);
		Shader.SetGlobalVector("warmColor", this._terrainConfig.warmColor);
		this.worldSpaceLightPosition = this._terrainConfig.rolePointLightPostion + this.eyePos;
		this.worldSpaceLightPosition.w = 1f;
		Shader.SetGlobalVector("_worldSpaceLightPosition", this.worldSpaceLightPosition);
		if (Camera.main != null)
		{
			Shader.SetGlobalVector("_worldSpaceViewPos", Camera.main.transform.position);
		}
		Shader.SetGlobalVector("_lightColor", this._terrainConfig.rolePointLightColor);
		Shader.SetGlobalFloat("_lightRange", this._terrainConfig.rolePointLightRange);
		Shader.SetGlobalFloat("_lightIntensity", this._terrainConfig.rolePointLightIntensity * 100f);
	}

	private void Preoad()
	{
		int num = 0;
		int count = this.preloadUnitAssetPathList.Count;
		for (int i = this.loadIndex; i < count; i++)
		{
			AssetType type = (AssetType)this.preloadUnitAssetTypeList[i];
			Asset item = AssetLibrary.Load(this.preloadUnitAssetPathList[i], type, LoadType.Type_ResMan);
			this.preloadUnitAssetList.Add(item);
			num++;
			if (num >= this.preloadMaxCountPer)
			{
				this.loadIndex = i + 1;
				return;
			}
		}
	}

	private bool CheckPreLoadComplate()
	{
		if (this.preloadUnitAssetList.Count < this.preloadUnitAssetPathList.Count)
		{
			return false;
		}
		int count = this.preloadUnitAssetList.Count;
		for (int i = 0; i < count; i++)
		{
			if (!this.preloadUnitAssetList[i].loaded)
			{
				return false;
			}
		}
		this.preloadComplate = true;
		return true;
	}

	public void LoadLightmap()
	{
		List<LightmapData> list = new List<LightmapData>();
		for (int i = 0; i < this.lightmapCount; i++)
		{
			string path = string.Concat(new object[]
			{
				"Scenes/",
				this.sceneID,
				"/lightmap/LightmapFar-",
				i
			});
			LightmapData lightmapData = new LightmapData();
			Asset asset;
			if (!this.loadFromAssetBund)
			{
				asset = AssetLibrary.Load(path, AssetType.Texture2D, LoadType.Type_Resources);
			}
			else
			{
				asset = AssetLibrary.Load(path, AssetType.Texture2D, LoadType.Type_AssetBundle);
			}
			if (asset.texture2D != null)
			{
				if (GameScene.lightmapCorrectOn)
				{
					LightmapCorrection.mapsCount++;
					lightmapData.lightmapFar = LightmapCorrection.Bake(asset.texture2D, 1024);
					AssetLibrary.RemoveAsset(path);
				}
				else
				{
					lightmapData.lightmapFar = asset.texture2D;
				}
				list.Add(lightmapData);
			}
		}
		LightmapSettings.lightmapsMode = LightmapsMode.Single;
		if (GameScene.lightmapCorrectOn)
		{
			LightmapCorrection.Clear();
		}
		LightmapData[] lightmaps = list.ToArray();
		LightmapSettings.lightmaps = lightmaps;
	}

	private float PreLoadProgress()
	{
		int count = this.preloadUnitAssetList.Count;
		this.curPreLoadCount = 0;
		for (int i = 0; i < count; i++)
		{
			if (this.preloadUnitAssetList[i].loaded)
			{
				this.curPreLoadCount++;
			}
		}
		return (float)this.curPreLoadCount / (float)this.preloadUnitAssetPathList.Count;
	}

	public void UpdateView(Vector3 eyePos)
	{
		if (this.destroyed)
		{
			return;
		}
		if (GameScene.isPlaying && !this.readCompalte)
		{
			return;
		}
		if (!this.loadTerrainTextureComplate)
		{
			return;
		}
		if (this.mainCamera == null)
		{
			this.mainCamera = Camera.main;
			this.mainCamera.backgroundColor = RenderSettings.fogColor;
			if (GameScene.mainCameraCullingMask < 0)
			{
				GameScene.mainCameraCullingMask = this.mainCamera.cullingMask;
			}
			this.mainCamera.farClipPlane = 160f;
		}
		if (this.mainCamera == null)
		{
			return;
		}
		if (GameScene.isPlaying && !this.loadSceneComplate)
		{
			this.mainCamera.cullingMask = 0;
		}
		if (!this.loadSceneComplate)
		{
			Application.targetFrameRate = 50;
		}
		else
		{
			Application.targetFrameRate = this.targetFrameRate;
		}
		if (!this.needPreload || this.preloadComplate)
		{
			if (!this.loadSceneComplate)
			{
				if (this.oldSceneLoadUnitTick < this.sceneLoadUnitTick)
				{
					float num = this.preloadMaxProgress + (float)this.sceneLoadUnitTick / (float)this.sceneMaxLoadUnitTick;
					if (this.loadProgress <= num)
					{
						this.loadProgress += 0.01f;
					}
					if (this.loadProgress > 1f)
					{
						this.loadProgress = 1f;
					}
					try
					{
						if (this.sceneLoadingListener != null)
						{
							this.sceneLoadingListener();
						}
					}
					catch (Exception ex)
					{
						LogSystem.LogError(new object[]
						{
							"场景加载中回调错误!错误内容:" + ex.ToString()
						});
					}
					this.oldSceneLoadUnitTick = this.sceneLoadUnitTick;
				}
				if (this.tick > this.sceneLoadUnitTick + 10)
				{
					if (this.loadProgress < 1f)
					{
						this.loadProgress += 0.02f;
					}
					else if (this.loadTerrainTextureComplate)
					{
						if (this.loadComplateWaitTick == 2)
						{
							Resources.UnloadUnusedAssets();
							GC.Collect();
						}
						this.loadComplateWaitTick++;
						this.mainCamera.cullingMask = GameScene.mainCameraCullingMask;
						if (this.loadComplateWaitTick > 10)
						{
							this.loadSceneComplate = true;
							try
							{
								this.loadProgress = 1f;
								if (this.sceneLoadCompleListener != null)
								{
									this.sceneLoadCompleListener();
								}
							}
							catch (Exception ex2)
							{
								LogSystem.LogError(new object[]
								{
									"场景加载完毕回调错误! 错误信息: " + ex2.ToString()
								});
							}
						}
					}
				}
			}
			GameScene.isPlaying = Application.isPlaying;
			if (GameScene.isPlaying)
			{
				this.time += 0.002f;
			}
			eyePos.y = this.SampleHeight(eyePos, true);
			this.viewDir = eyePos - this.mainCamera.transform.position;
			this.viewDir.Normalize();
			if (!this.peConfigLoaded)
			{
				this.LoadPostEffectConfig();
			}
			this.UpdateShaderConstant();
			if (GameScene.isPlaying)
			{
				this.frames++;
				float num2 = Time.time;
				if (num2 > this.lastInterval + this.updateInterval)
				{
					this.fps = (float)this.frames / (num2 - this.lastInterval);
					this.frames = 0;
					this.lastInterval = num2;
					if (this.fps < 5f)
					{
						this.fps = 5f;
					}
				}
			}
			else
			{
				this.fps = 30f;
			}
			if (this.tick == 0)
			{
				this.LoadLightmap();
				this._oldLightmapCorrectOn = !GameScene.lightmapCorrectOn;
				this.lastInterval = Time.time;
				this.FirstRun();
				this.UpdateRegions();
				if (GameScene.isPlaying)
				{
					GameScene.dontCullUnit = false;
				}
			}
			if (this.tick % 5 == 0)
			{
				if (GameScene.isEditor)
				{
					this.ActivePostEffect(GameScene.postEffectEnable);
				}
				else if (GameScene.postEffectEnable)
				{
					this.ActivePostEffect(true);
				}
				else
				{
					this.ActivePostEffect(false);
				}
			}
			this.eyePos = eyePos;
			this.UpdateTiles();
			this.UpdateUnits();
			if (GameScene.isPlaying)
			{
				if (this.tick % 5 == 0 && Vector3.Distance(eyePos, this.lastPos) > 6f)
				{
					Tile tileAt = this.GetTileAt(eyePos);
					if (tileAt != this.curTile)
					{
						this.UpdateRegions();
						this.curTile = tileAt;
					}
					this.lastPos = eyePos;
				}
			}
			else
			{
				this.UpdateRegions();
			}
			if (GameScene.isPlaying && this.loadSceneComplate && this.tick % 10 == 0)
			{
				this.terBakeCount = 0;
				while (this.terBakeCount < this.perTerBakeCount)
				{
					for (int i = 0; i < this.tiles.Count; i++)
					{
						if (this.tiles[i].terrain != null && this.tiles[i].terrain.terrainRenderer.enabled && this.tiles[i].terrain.terrainMapIndex < 0 && this.tiles[i].viewDistance < 33f)
						{
							this.tiles[i].terrain.Bake();
							break;
						}
					}
					this.terBakeCount++;
				}
			}
			this.tick++;
			return;
		}
		if (this.preloadTick == 0)
		{
			if (this.preloadUnitAssetPathList.Count < 1)
			{
				this.preloadComplate = true;
				return;
			}
			this.preloadMaxTick = this.preloadInterval * this.preloadUnitAssetPathList.Count;
		}
		float num3 = this.PreLoadProgress() * this.preloadMaxProgress;
		if (this.loadProgress < num3)
		{
			this.loadProgress += this.progressInc;
			return;
		}
		this.progressInc = 0.01f;
		if (this.preloadTick % this.preloadInterval == 0)
		{
			this.Preoad();
		}
		this.preloadTick++;
		if (this.preloadTick % 2 == 0)
		{
			this.CheckPreLoadComplate();
		}
		if (this.preloadTick > this.preloadMaxTick)
		{
			this.preloadComplate = true;
		}
	}

	public void ResetLoad()
	{
		this.loadSceneComplate = false;
		this.tick = 0;
		this.sceneLoadUnitTick = 0;
		this.loadProgress = 0f;
		this.oldSceneLoadUnitTick = 0;
	}

	private void UpdateRegions()
	{
		float num = Mathf.Abs(this.eyePos.x);
		float num2 = Mathf.Abs(this.eyePos.z);
		float num3 = num / this.eyePos.x;
		float num4 = num2 / this.eyePos.z;
		if (this.eyePos.x == 0f)
		{
			this.curRegionX = 0;
		}
		else
		{
			this.curRegionX = (int)(Mathf.Ceil(num / (float)this._terrainConfig.regionSize) * num3);
		}
		if (this.eyePos.z == 0f)
		{
			this.curRegionY = 0;
		}
		else
		{
			this.curRegionY = (int)(Mathf.Ceil(num2 / (float)this._terrainConfig.regionSize) * num4);
		}
		int num5 = this.curRegionX - this.viewRect;
		int num6 = this.curRegionX + this.viewRect;
		int num7 = this.curRegionY - this.viewRect;
		int num8 = this.curRegionY + this.viewRect;
		num5 = -1;
		num7 = -1;
		num6 = 1;
		num8 = 1;
		if (!GameScene.dontCullUnit)
		{
			this.regions.Clear();
		}
		for (int i = num5; i <= num6; i++)
		{
			for (int j = num7; j <= num8; j++)
			{
				Region region = null;
				bool flag = this.regionsMap.ContainsKey(i + "_" + j);
				if (flag)
				{
					region = this.regionsMap[i + "_" + j];
				}
				if (region == null)
				{
					string path = string.Empty;
					Asset asset;
					if (!GameScene.isPlaying)
					{
						path = string.Concat(new object[]
						{
							"Scenes/",
							this.sceneID,
							"/",
							i,
							"_",
							j,
							"/Region"
						});
						asset = AssetLibrary.Load(path, AssetType.Region, LoadType.Type_Resources);
					}
					else
					{
						path = string.Concat(new object[]
						{
							"Scenes/",
							this.sceneID,
							"/",
							i,
							"_",
							j,
							"/Region"
						});
						if (this.loadFromAssetBund)
						{
							asset = AssetLibrary.Load(path, AssetType.Region, LoadType.Type_AssetBundle);
						}
						else
						{
							asset = AssetLibrary.Load(path, AssetType.Region, LoadType.Type_Resources);
						}
					}
					if (asset.loaded)
					{
						region = asset.region;
						this.regions.Add(region);
						this.regionsMap.Add(region.regionX + "_" + region.regionY, region);
					}
					else if (!GameScene.isPlaying)
					{
						region = Region.Create(this, i, j);
						this.regions.Add(region);
						this.regionsMap.Add(region.regionX + "_" + region.regionY, region);
					}
				}
				else if (GameScene.dontCullUnit)
				{
					if (!flag)
					{
						this.regions.Add(region);
					}
				}
				else
				{
					this.regions.Add(region);
				}
			}
		}
		if (!GameScene.dontCullUnit)
		{
			this.regionsMap.Clear();
		}
		for (int i = 0; i < this.regions.Count; i++)
		{
			this.regKey = this.regions[i].regionX + "_" + this.regions[i].regionY;
			if (!this.regionsMap.ContainsKey(this.regKey))
			{
				this.regionsMap.Add(this.regKey, this.regions[i]);
			}
		}
		int count = this.regions.Count;
		for (int i = 0; i < count; i++)
		{
			this.regions[i].Update(this.eyePos);
		}
	}

	public void UpdateViewRange()
	{
		for (int i = 0; i < this.regions.Count; i++)
		{
			this.regions[i].UpdateViewRange();
		}
		for (int i = 0; i < this.units.Count; i++)
		{
			if (this.units[i].isStatic)
			{
				this.units[i].UpdateViewRange();
			}
		}
		for (int j = 0; j < this.dynamicUnits.Count; j++)
		{
			GameObjectUnit gameObjectUnit = this.dynamicUnits[j];
			gameObjectUnit.near = this.terrainConfig.dynamiCullingDistance;
			gameObjectUnit.far = gameObjectUnit.near + 2f;
		}
	}

	private void UpdateTiles()
	{
		int num = this.tiles.Count;
		if (this.tick % 1 == 0)
		{
			this.visTileCount = 0;
		}
		for (int i = 0; i < num; i++)
		{
			this.dx = this.eyePos.x - this.tiles[i].position.x;
			this.dz = this.eyePos.z - this.tiles[i].position.z;
			this.tiles[i].viewDistance = Mathf.Sqrt(this.dx * this.dx + this.dz * this.dz);
			if (this.tiles[i].terrain != null)
			{
				if (this.tiles[i].viewDistance > 30f)
				{
					this.tiles[i].terrain.WithoutShadow();
				}
				else
				{
					this.tiles[i].terrain.ReceiveShadow();
				}
			}
			if (this.tiles[i].viewDistance > 39f && this.tiles[i].terrain != null)
			{
				this.tiles[i].terrain.CancelBake();
			}
			if (this.tiles[i].viewDistance > this.tiles[i].far + 8f)
			{
				if (!GameScene.dontCullUnit)
				{
					this.RemoveTile(this.tiles[i], false);
					num--;
					i--;
				}
			}
			else
			{
				this.tiles[i].Update(this.eyePos);
				if (!this.tiles[i].visible && this.visTileCount < this.visibleTilePerFrame)
				{
					this.tiles[i].Visible();
					this.visTileCount++;
				}
			}
		}
	}

	public List<string> CollectStaticUnitAssetPath()
	{
		List<string> list = new List<string>();
		int count = this.units.Count;
		for (int i = 0; i < count; i++)
		{
			if (this.units[i].isStatic && !list.Contains(this.units[i].prePath) && this.units[i].ins != null)
			{
				list.Add(this.units[i].prePath);
			}
		}
		return list;
	}

	public List<string> CollectTerrainSplatsAssetPath()
	{
		List<string> list = new List<string>();
		int count = this.tiles.Count;
		for (int i = 0; i < count; i++)
		{
			if (this.tiles[i].terrain != null)
			{
				Splat[] splats = this.tiles[i].terrain.terrainData.splats;
				for (int j = 0; j < splats.Length; j++)
				{
					if (splats[j] != null && !list.Contains(splats[j].key))
					{
						list.Add(splats[j].key);
					}
				}
			}
		}
		return list;
	}

	public void UpdateUnits()
	{
		int num = this.units.Count;
		this.visibleDynamicUnitCount = 0;
		this.visibleStaticUnitCount = 0;
		if (this.tick % 3 == 0)
		{
			this.visibleDynaUnitPerFrame = 0;
		}
		this.visibleStaticUnitPerFrame = 0;
		this.hideStaticUnitPerFrame = 0;
		this.useMaterialsCount = 0;
		this.materials.Clear();
		this.visibleStaticTypeCount = 0;
		this.staticTypeMap.Clear();
		this.dynamicUnits.Clear();
		GameObjectUnit gameObjectUnit = null;
		GameObjectUnit gameObjectUnit2 = null;
		this.shadowUnits.Clear();
		for (int i = 0; i < num; i++)
		{
			GameObjectUnit gameObjectUnit3 = this.units[i];
			if (!GameScene.isPlaying && gameObjectUnit3.isStatic)
			{
				gameObjectUnit3.Update();
			}
			if (gameObjectUnit3.isStatic && (this.hideStaticUnitPerFrame < 1 || this.visibleStaticUnitPerFrame < 1))
			{
				this.dx = gameObjectUnit3.center.x - this.eyePos.x;
				this.dz = gameObjectUnit3.center.z - this.eyePos.z;
				gameObjectUnit3.viewDistance = Mathf.Sqrt(this.dx * this.dx + this.dz * this.dz);
				if (gameObjectUnit3.visible && this.hideStaticUnitPerFrame < 1 && gameObjectUnit3.viewDistance > gameObjectUnit3.far)
				{
					if (!GameScene.dontCullUnit)
					{
						this.RemoveUnit(gameObjectUnit3, false, false);
						num--;
						i--;
						this.hideStaticUnitPerFrame++;
					}
					gameObjectUnit3.active = false;
				}
				if (!gameObjectUnit3.visible && this.visibleStaticUnitPerFrame < 1 && gameObjectUnit3.viewDistance < gameObjectUnit3.near && (gameObjectUnit3.combineParentUnitID < 0 || GameScene.SampleMode))
				{
					gameObjectUnit3.active = true;
					Vector3 lhs = gameObjectUnit3.center - this.mainCamera.transform.position;
					lhs.Normalize();
					gameObjectUnit3.viewAngle = Mathf.Acos(Vector3.Dot(lhs, this.viewDir)) / gameObjectUnit3.cullingFactor;
					if (gameObjectUnit3.viewAngle < this.terrainConfig.cullingAngleFactor)
					{
						gameObjectUnit3.Visible();
						this.visibleStaticUnitPerFrame++;
						if (!this.loadSceneComplate)
						{
							this.sceneLoadUnitTick++;
						}
					}
					this.visibleStaticUnitCount++;
				}
			}
			if (!gameObjectUnit3.isStatic)
			{
				if (gameObjectUnit3.visible)
				{
					this.visibleDynamicUnitCount++;
				}
				this.dx = gameObjectUnit3.position.x - this.eyePos.x;
				this.dz = gameObjectUnit3.position.z - this.eyePos.z;
				gameObjectUnit3.viewDistance = Mathf.Sqrt(this.dx * this.dx + this.dz * this.dz);
				if (gameObjectUnit3.position.y > 1E+08f)
				{
					gameObjectUnit3.position.y = this.SampleHeight(gameObjectUnit3.position, true);
				}
				if (gameObjectUnit == null)
				{
					if (!gameObjectUnit3.visible)
					{
						gameObjectUnit = gameObjectUnit3;
					}
				}
				else if (!gameObjectUnit3.visible && gameObjectUnit3.viewDistance < gameObjectUnit.viewDistance)
				{
					gameObjectUnit = gameObjectUnit3;
				}
				if (gameObjectUnit3.willRemoved && this.removeDynUnits.Count > 0)
				{
					if (gameObjectUnit2 == null)
					{
						gameObjectUnit2 = gameObjectUnit3;
					}
					else if (gameObjectUnit3.viewDistance > gameObjectUnit2.viewDistance)
					{
						gameObjectUnit2 = gameObjectUnit3;
					}
				}
				this.dynamicUnits.Add(gameObjectUnit3);
				if (this.shadowUnits.Count < this.maxCastShadowsUnitCount || gameObjectUnit3.isMainUint)
				{
					this.shadowUnits.Add(gameObjectUnit3);
				}
				else
				{
					float viewDistance = gameObjectUnit3.viewDistance;
					int num2 = -1;
					for (int j = 0; j < this.shadowUnits.Count; j++)
					{
						if (this.shadowUnits[j].viewDistance > viewDistance)
						{
							num2 = j;
							viewDistance = this.shadowUnits[j].viewDistance;
						}
					}
					if (num2 > -1)
					{
						this.shadowUnits.RemoveAt(num2);
						this.shadowUnits.Add(gameObjectUnit3);
					}
				}
				if (gameObjectUnit3.viewDistance > gameObjectUnit3.far)
				{
					if (gameObjectUnit3.willRemoved)
					{
						this.RemoveDynamicUnit(gameObjectUnit3 as DynamicUnit, true, true);
						num--;
					}
					else
					{
						gameObjectUnit3.Invisible();
					}
				}
				if (this.mapPath != null && gameObjectUnit3.hasCollision)
				{
					this.mapPath.SetDynamicCollision(gameObjectUnit3.position, gameObjectUnit3.collisionSize, true, 1);
					gameObjectUnit3.hasCollision = false;
				}
				if (!gameObjectUnit3.willRemoved)
				{
					gameObjectUnit3.Update();
					if (this.enableDynamicGrid && gameObjectUnit3.viewDistance < this._terrainConfig.collisionComputeRange && this.mapPath != null && gameObjectUnit3.isCollider && gameObjectUnit3.grids == null)
					{
						this.mapPath.SetDynamicCollision(gameObjectUnit3.position, gameObjectUnit3.collisionSize, false, 1);
						gameObjectUnit3.hasCollision = true;
					}
				}
			}
		}
		if (gameObjectUnit != null && gameObjectUnit.viewDistance < gameObjectUnit.near && this.visibleDynaUnitPerFrame < 1)
		{
			gameObjectUnit.Visible();
			this.visibleDynaUnitPerFrame++;
		}
		if (gameObjectUnit2 != null)
		{
			this.RemoveDynamicUnit(gameObjectUnit2 as DynamicUnit, true, true);
			this.removeDynUnits.Remove(gameObjectUnit2 as DynamicUnit);
		}
		for (int i = 0; i < this.dynamicUnits.Count; i++)
		{
			this.dynamicUnits[i].castShadows = false;
		}
		for (int i = 0; i < this.shadowUnits.Count; i++)
		{
			this.shadowUnits[i].castShadows = true;
		}
	}

	public void RemvoeWillRemoveDynUnits()
	{
		for (int i = 0; i < this.units.Count; i++)
		{
			GameObjectUnit gameObjectUnit = this.units[i];
			if (!gameObjectUnit.isStatic && gameObjectUnit.willRemoved)
			{
				this.RemoveDynamicUnit(gameObjectUnit as DynamicUnit, true, true);
				this.removeDynUnits.Remove(gameObjectUnit as DynamicUnit);
				i--;
			}
		}
	}

	public GameObjectUnit GetTouchDynamicUnit(float touchRange = 700f)
	{
		int count = this.dynamicUnits.Count;
		float x = Input.mousePosition.x;
		float y = Input.mousePosition.y;
		for (int i = 0; i < count; i++)
		{
			GameObjectUnit gameObjectUnit = this.dynamicUnits[i];
			if (gameObjectUnit.visible && gameObjectUnit.mouseEnable)
			{
				float num = x - gameObjectUnit.screenPoint.x;
				float num2 = y - gameObjectUnit.screenPoint.y;
				if (num * num + num2 * num2 < touchRange)
				{
					return gameObjectUnit;
				}
			}
		}
		return null;
	}

	public Tile GetTileAt(Vector3 worldPosition)
	{
		int num = (int)Mathf.Floor((worldPosition.x + (float)this._terrainConfig.regionSize * 0.5f) / (float)this._terrainConfig.regionSize);
		int num2 = (int)Mathf.Floor((worldPosition.z + (float)this._terrainConfig.regionSize * 0.5f) / (float)this._terrainConfig.regionSize);
		string key = num + "_" + num2;
		Region region = null;
		if (this.regionsMap.ContainsKey(key))
		{
			region = this.regionsMap[key];
		}
		if (region == null)
		{
			return null;
		}
		return region.GetTile(worldPosition);
	}

	public Tile GetNeighborTile(Tile tile, int dirX, int dirY)
	{
		int num = tile.region.regionX;
		int num2 = tile.region.regionY;
		int tileX = tile.tileX;
		int tileY = tile.tileY;
		int num3 = tileX + dirX;
		int num4 = tileY + dirY;
		int num5 = Mathf.FloorToInt((float)this._terrainConfig.tileCountPerSide * 0.5f);
		if (num3 < -num5)
		{
			num--;
			num3 = num5;
		}
		else if (num3 > num5)
		{
			num++;
			num3 = -num5;
		}
		if (num4 < -num5)
		{
			num2--;
			num4 = num5;
		}
		else if (num4 > num5)
		{
			num2++;
			num4 = -num5;
		}
		string key = string.Concat(new object[]
		{
			num,
			"_",
			num2,
			"_",
			num3,
			"_",
			num4
		});
		if (this.tilesMap.ContainsKey(key))
		{
			return this.tilesMap[key];
		}
		return null;
	}

	public float SampleHeight(Vector3 worldPosition, bool interpolation = true)
	{
		if (this.terrainConfig == null)
		{
			return 1E+09f;
		}
		if (this.heights != null)
		{
			int num = (int)this._terrainConfig.sceneHeightmapResolution;
			float num2 = worldPosition.x + this.terrainConfig.sceneHeightmapResolution * 0.5f;
			float num3 = worldPosition.z + this.terrainConfig.sceneHeightmapResolution * 0.5f;
			float num4 = num2 % 1f;
			float num5 = num3 % 1f;
			int num6 = Mathf.FloorToInt(num2);
			int num7 = Mathf.FloorToInt(num3);
			int num8 = num6 + 1;
			int num9 = num7 + 1;
			if (num6 < 0 || num7 < 0 || num6 >= num || num7 >= num)
			{
				return this._terrainConfig.defaultTerrainHeight;
			}
			if (num8 < 0 || num9 < 0 || num8 > num || num9 >= num)
			{
				return this._terrainConfig.defaultTerrainHeight;
			}
			float num10 = this.heights[num6, num7] * (1f - num4) + this.heights[num8, num7] * num4;
			float num11 = this.heights[num6, num9] * (1f - num4) + this.heights[num8, num9] * num4;
			return num11 * num5 + num10 * (1f - num5);
		}
		else
		{
			int num12 = (int)Mathf.Floor((worldPosition.x + (float)this._terrainConfig.regionSize * 0.5f) / (float)this._terrainConfig.regionSize);
			int num13 = (int)Mathf.Floor((worldPosition.z + (float)this._terrainConfig.regionSize * 0.5f) / (float)this._terrainConfig.regionSize);
			Region region = null;
			this.smkey = num12 + "_" + num13;
			if (this.regionsMap.ContainsKey(this.smkey))
			{
				region = this.regionsMap[this.smkey];
			}
			if (region == null)
			{
				return 1E+09f;
			}
			Tile tile = region.GetTile(worldPosition);
			if (tile == null)
			{
				LogSystem.LogWarning(new object[]
				{
					"SampleHeight tile is null, position-> " + worldPosition
				});
				return 1E+09f;
			}
			if (interpolation)
			{
				return tile.SampleHeightInterpolation(worldPosition);
			}
			return tile.SampleHeight(worldPosition, 0f);
		}
	}

	public bool Underwater(Vector3 postion)
	{
		int num = (int)Mathf.Floor((postion.x + (float)this._terrainConfig.regionSize * 0.5f) / (float)this._terrainConfig.regionSize);
		int num2 = (int)Mathf.Floor((postion.z + (float)this._terrainConfig.regionSize * 0.5f) / (float)this._terrainConfig.regionSize);
		Region region = null;
		string key = num + "_" + num2;
		if (this.regionsMap.ContainsKey(key))
		{
			region = this.regionsMap[key];
		}
		if (region == null)
		{
			return false;
		}
		Tile tile = region.GetTile(postion);
		if (tile == null || tile.water == null)
		{
			return false;
		}
		if (tile.water.Underwater(postion.y))
		{
			this.waterHeight = tile.water.waterData.height;
			return true;
		}
		return false;
	}

	public void GetGroundType(Vector3 worldPos)
	{
	}
}
