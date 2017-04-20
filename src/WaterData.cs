using com.snailgames.chosen.scene.library;
using System;
using System.IO;
using UnityEngine;

public class WaterData
{
	public int ambienceSamplerID;

	public float height;

	public Vector4 waveSpeed = new Vector4(2f, 2f, -2f, -2f);

	public float waveScale = 0.02f;

	public Color horizonColor;

	public string depthMapPath = string.Empty;

	public string colorControlPath = string.Empty;

	public string waterBumpMapPath = string.Empty;

	public float waterVisibleDepth;

	public float waterDiffValue;

	public float reflDistort = 0.44f;

	public float refrDistort = 0.2f;

	public Texture2D colorControlMap;

	public Texture2D bumpMap;

	public Texture2D depthMap;

	public float alpha = 1f;

	public void Read(BinaryReader br)
	{
		this.ambienceSamplerID = br.ReadInt32();
		this.height = br.ReadSingle();
		this.waveSpeed.x = br.ReadSingle();
		this.waveSpeed.y = br.ReadSingle();
		this.waveSpeed.z = br.ReadSingle();
		this.waveSpeed.w = br.ReadSingle();
		this.waveScale = br.ReadSingle();
		this.horizonColor = new Color(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
		this.waterVisibleDepth = br.ReadSingle();
		this.waterDiffValue = br.ReadSingle();
		long position = br.BaseStream.Position;
		if (br.ReadInt32() == 10015)
		{
			this.alpha = br.ReadSingle();
		}
		else
		{
			br.BaseStream.Position = position;
		}
		this.depthMapPath = br.ReadString();
		this.colorControlPath = br.ReadString();
		this.waterBumpMapPath = br.ReadString();
		this.colorControlMap = AssetLibrary.Load(this.colorControlPath, AssetType.Texture2D, LoadType.Type_Resources).texture2D;
		this.bumpMap = AssetLibrary.Load(this.waterBumpMapPath, AssetType.Texture2D, LoadType.Type_Resources).texture2D;
		this.depthMap = AssetLibrary.Load(this.depthMapPath, AssetType.Texture2D, LoadType.Type_Auto).texture2D;
	}

	public void Release()
	{
		this.colorControlMap = null;
		this.bumpMap = null;
		this.depthMap = null;
	}
}
