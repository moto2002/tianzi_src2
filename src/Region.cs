using System;
using System.IO;
using UnityEngine;

public class Region
{
	public GameScene scene;

	public Vector3 postion;

	public int regionX = -1;

	public int regionY = -1;

	public float actualX;

	public float actualY;

	public int lightmapCount;

	public Tile[] tiles;

	public string regionDataPath = string.Empty;

	private float localX;

	private float localY;

	private int r;

	private int c;

	private int tileInd;

	private float dx;

	private float dz;

	public Tile GetTile(Vector3 worldPosition)
	{
		try
		{
			this.localX = worldPosition.x - this.actualX + (float)this.scene.terrainConfig.regionSize * 0.5f;
			this.localY = worldPosition.z - this.actualY + (float)this.scene.terrainConfig.regionSize * 0.5f;
			this.r = Mathf.FloorToInt(this.localX / (float)this.scene.terrainConfig.tileSize);
			this.c = Mathf.FloorToInt(this.localY / (float)this.scene.terrainConfig.tileSize);
			if (this.r > this.scene.terrainConfig.tileCountPerSide)
			{
				this.r = this.scene.terrainConfig.tileCountPerSide;
			}
			if (this.c > this.scene.terrainConfig.tileCountPerSide)
			{
				this.c = this.scene.terrainConfig.tileCountPerSide;
			}
			this.tileInd = this.c * this.scene.terrainConfig.tileCountPerSide + this.r;
			if (this.tileInd > this.tiles.Length - 1)
			{
				Tile result = null;
				return result;
			}
			if (this.tiles[this.tileInd] != null)
			{
				Tile result = this.tiles[this.tileInd];
				return result;
			}
		}
		catch (Exception ex)
		{
			LogSystem.LogError(new object[]
			{
				"获取地形切片错误 :  " + ex.Message
			});
			Tile result = null;
			return result;
		}
		return null;
	}

	public Tile GetTile(int tileX, int tileY)
	{
		return this.tiles[tileX + tileY * this.scene.terrainConfig.tileCountPerSide];
	}

	public GameObjectUnit FindUint(int createID)
	{
		int num = this.tiles.Length;
		for (int i = 0; i < num; i++)
		{
			if (this.tiles[i] != null)
			{
				GameObjectUnit gameObjectUnit = this.tiles[i].FindUnit(createID);
				if (gameObjectUnit != null)
				{
					return gameObjectUnit;
				}
			}
		}
		return null;
	}

	public static Region Create(GameScene scene, int regionX, int regionY)
	{
		Region region = new Region();
		region.scene = scene;
		region.tiles = new Tile[scene.terrainConfig.tileCountPerRegion];
		region.regionX = regionX;
		region.regionY = regionY;
		region.regionDataPath = string.Concat(new object[]
		{
			"Scenes/",
			scene.sceneID,
			"/",
			regionX,
			"_",
			regionY,
			"/Region"
		});
		region.actualX = (float)(regionX * scene.terrainConfig.regionSize);
		region.actualY = (float)(regionY * scene.terrainConfig.regionSize);
		region.postion = Vector3.zero;
		region.postion.x = region.actualX;
		region.postion.y = 0f;
		region.postion.z = region.actualY;
		int num = Mathf.FloorToInt((float)scene.terrainConfig.tileCountPerSide * 0.5f);
		for (int i = 0; i < scene.terrainConfig.tileCountPerSide; i++)
		{
			for (int j = 0; j < scene.terrainConfig.tileCountPerSide; j++)
			{
				Tile tile = Tile.Create(region, i - num, j - num);
				region.tiles[j * scene.terrainConfig.tileCountPerSide + i] = tile;
			}
		}
		return region;
	}

	public void UpdateViewRange()
	{
		for (int i = 0; i < this.tiles.Length; i++)
		{
			if (this.tiles[i] != null)
			{
				this.tiles[i].UpdateViewRange();
			}
		}
	}

	public Region Read(byte[] bytes)
	{
		MemoryStream memoryStream = new MemoryStream(bytes);
		BinaryReader binaryReader = new BinaryReader(memoryStream);
		long position = binaryReader.BaseStream.Position;
		int num;
		if (binaryReader.ReadInt32() == 10013)
		{
			num = binaryReader.ReadInt32();
		}
		else
		{
			num = GameScene.mainScene.sceneID;
			binaryReader.BaseStream.Position = position;
		}
		this.scene = GameScene.mainScene;
		this.tiles = new Tile[this.scene.terrainConfig.tileCountPerRegion];
		this.regionX = binaryReader.ReadInt32();
		this.regionY = binaryReader.ReadInt32();
		this.regionDataPath = string.Concat(new object[]
		{
			"Scenes/",
			num,
			"/",
			this.regionX,
			"_",
			this.regionY,
			"/Region"
		});
		this.actualX = (float)(this.regionX * this.scene.terrainConfig.regionSize);
		this.actualY = (float)(this.regionY * this.scene.terrainConfig.regionSize);
		this.lightmapCount = binaryReader.ReadInt32();
		int num2 = 0;
		int num3 = Mathf.FloorToInt((float)this.scene.terrainConfig.tileCountPerSide * 0.5f);
		while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length && binaryReader.ReadString() == "Tile")
		{
			Tile tile = new Tile(this);
			tile.Read(binaryReader);
			int num4 = tile.tileX + num3;
			int num5 = tile.tileY + num3;
			this.tiles[num5 * this.scene.terrainConfig.tileCountPerSide + num4] = tile;
			num2++;
			if (num2 >= this.scene.terrainConfig.tileCountPerRegion)
			{
				break;
			}
		}
		this.postion = Vector3.zero;
		this.postion.x = this.actualX;
		this.postion.y = 0f;
		this.postion.z = this.actualY;
		binaryReader.Close();
		memoryStream.Flush();
		return this;
	}

	public void Destroy()
	{
		for (int i = 0; i < this.tiles.Length; i++)
		{
			Tile tile = this.tiles[i];
			if (tile != null)
			{
				if (this.scene.ContainTile(tile))
				{
					this.scene.RemoveTile(tile, true);
				}
				else
				{
					tile.Destroy();
				}
			}
		}
		this.tiles = null;
		this.scene = null;
	}

	public void Update(Vector3 eyePos)
	{
		if (this.tiles == null)
		{
			return;
		}
		int num = this.tiles.Length;
		for (int i = 0; i < num; i++)
		{
			if (this.tiles[i] != null && !this.tiles[i].visible && !this.scene.ContainTile(this.tiles[i]))
			{
				this.dx = eyePos.x - this.tiles[i].position.x;
				this.dz = eyePos.z - this.tiles[i].position.z;
				this.tiles[i].viewDistance = Mathf.Sqrt(this.dx * this.dx + this.dz * this.dz);
				if (this.tiles[i].viewDistance < this.tiles[i].far)
				{
					this.scene.AddTile(this.tiles[i]);
				}
			}
		}
	}
}
