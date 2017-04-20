using System;
using UnityEngine;

public class TerrainConfig
{
	public int sceneWidth = 480;

	public int sceneHeight = 480;

	public float sceneHeightmapResolution = 480f;

	public Splat baseSplat = new Splat();

	public Vector3 cameraLookAt = Vector3.zero;

	public float cameraDistance;

	public float cameraRotationX;

	public float cameraRotationY;

	public float cameraRotationZ;

	public Color fogColor = new Color(0.266666681f, 0.8156863f, 1f);

	public float startDistance = 6f;

	public float globalDensity = 1f;

	public float heightScale = 1f;

	public float height = 110f;

	public Vector4 fogIntensity = new Vector4(1f, 1f, 1f, 1f);

	public int tileSize = 32;

	public int tileCountPerSide;

	public int tileCountPerRegion;

	public int regionSize = 160;

	public int heightmapResolution = 32;

	public int waterDepthmapResolution = 64;

	public int gridResolution = 32;

	public int splatmapResolution = 32;

	public int spaltsmapLayers = 4;

	public float blockHeight = 1f;

	public float maxReachTerrainHeight = 200f;

	public float gridSize = 1f;

	public float defaultTerrainHeight = 50f;

	public float maxTerrainHeight = 200f;

	public float tileCullingDistance = 100f;

	public float unitCullingDistance = 30f;

	public float cullingBaseDistance = 10f;

	public float cullingAngleFactor = 3f;

	public float viewAngleLodFactor = 2f;

	public float dynamiCullingDistance = 15f;

	public float defautCullingFactor = 2f;

	public Vector4 sunLightDir = new Vector4(-0.41f, 0.74f, 0.18f, 0f);

	public float waterSpecRange = 46.3f;

	public float waterSpecStrength = 0.84f;

	public float waterDiffRange;

	public float waterDiffStrength;

	public Vector4 waveSpeed = new Vector4(2f, 2f, -2f, -2f);

	public float waveScale = 0.02f;

	public Color horizonColor;

	public Texture2D colorControl;

	public Texture2D waterBumpMap;

	public float defaultWaterHeight = 48f;

	public float waterVisibleDepth = 0.5f;

	public float waterAlpha = 1f;

	public float reflDistort = 0.44f;

	public float refrDistort = 0.2f;

	public float waterDiffValue;

	public float collisionComputeRange = 3f;

	public bool enablePointLight = true;

	public float pointLightRangeMin = 2f;

	public float pointLightRangeMax = 5.6f;

	public float pointLightIntensity = 1f;

	public Color pointLightColor = new Color(1f, 1f, 1f);

	public Vector3 rolePointLightPostion = new Vector3(-100.12f, -12.86f, 270.2f);

	public Color rolePointLightColor = new Color(1f, 1f, 1f);

	public float rolePointLightRange = 19.7f;

	public float rolePointLightIntensity = 2.68f;

	public Color coolColor = new Color(1f, 1f, 1f, 1f);

	public Color warmColor = new Color(1f, 1f, 1f, 1f);

	public int weather;

	public bool enableTerrain = true;
}
