using Algorithms;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapPath
{
	public delegate void PathFindEndBack(List<Vector3> paths);

	public delegate void OnFindPathFailed(int iFailResult);

	public int miOrignWidth;

	public int miOrignHeight;

	public int miHalfOrignWidth;

	public int miHalfOrignHeight;

	public int halfWidth;

	public int halfHeight;

	public ushort[,] grids;

	private PathFinderFast mPathfinder;

	public int maxDynamicCollisizeSize = 2;

	public int gridTypeMask = 5;

	private int fullMask;

	private sbyte[,] mDirection = new sbyte[,]
	{
		{
			0,
			-1
		},
		{
			1,
			0
		},
		{
			0,
			1
		},
		{
			-1,
			0
		},
		{
			1,
			-1
		},
		{
			1,
			1
		},
		{
			-1,
			1
		},
		{
			-1,
			-1
		}
	};

	public bool pathFindResult;

	public List<Vector3> paths = new List<Vector3>();

	public MapPath.PathFindEndBack pathFindEndBack;

	public static MapPath.OnFindPathFailed mOnFindPathFailed;

	public float gridSize
	{
		get;
		private set;
	}

	public MapPath(float gridSize, int iWidth = 0, int iHeight = 0)
	{
		for (int i = 0; i < this.maxDynamicCollisizeSize; i++)
		{
			this.fullMask |= 1 << i;
		}
		this.miOrignWidth = iWidth;
		this.miOrignHeight = iHeight;
		this.miHalfOrignWidth = this.miOrignWidth / 2;
		this.miHalfOrignHeight = this.miOrignHeight / 2;
		this.gridSize = gridSize;
		this.grids = new ushort[this.miHalfOrignWidth, this.miHalfOrignHeight];
		Array.Clear(this.grids, 0, this.grids.Length);
	}

	public void Read2(int iWidth, int iHeight, BinaryReader br)
	{
		for (int i = 0; i < iWidth; i++)
		{
			for (int j = 0; j < iHeight; j++)
			{
				ushort uValue;
				if (br.ReadInt32() == 0)
				{
					uValue = 0;
				}
				else
				{
					uValue = 1;
				}
				this.SetGridValue(i, j, uValue);
			}
		}
		this.mPathfinder = new PathFinderFast(this.grids, this.miHalfOrignWidth, this.miHalfOrignHeight, this.gridSize);
	}

	public void Read3(int iWidth, int iHeight, byte[,] grids)
	{
		for (int i = 0; i < iWidth; i++)
		{
			for (int j = 0; j < iHeight; j++)
			{
				ushort uValue;
				if (grids[i, j] == 0)
				{
					uValue = 0;
				}
				else
				{
					uValue = 1;
				}
				this.SetGridValue(i, j, uValue);
			}
		}
	}

	public byte GetGridValue(int ix, int iy)
	{
		ushort num = (ushort)(ix >> 1);
		ushort num2 = (ushort)(iy >> 1);
		ushort num3 = (ushort)(ix % 2);
		ushort num4 = (ushort)(iy % 2);
		if (num < 0 || num2 < 0 || (int)num >= this.miHalfOrignWidth || (int)num2 >= this.miHalfOrignHeight)
		{
			return 1;
		}
		return (byte)(this.grids[(int)num, (int)num2] >> (int)(8 * num4 + 4 * num3) & 15);
	}

	public byte GetGridValue2(int ix, int iy, ushort[,] grid)
	{
		ushort num = (ushort)(ix >> 1);
		ushort num2 = (ushort)(iy >> 1);
		ushort num3 = (ushort)(ix % 2);
		ushort num4 = (ushort)(iy % 2);
		if (num < 0 || num2 < 0 || (int)num >= this.miHalfOrignWidth || (int)num2 >= this.miHalfOrignHeight)
		{
			return 1;
		}
		return (byte)(grid[(int)num, (int)num2] >> (int)(8 * num4 + 4 * num3) & 15);
	}

	public void SetGridValue(int ix, int iy, ushort uValue)
	{
		int num = ix >> 1;
		int num2 = iy >> 1;
		if (num < 0 || num2 < 0 || num >= this.miHalfOrignWidth || num2 >= this.miHalfOrignHeight)
		{
			return;
		}
		int num3 = ix % 2;
		int num4 = iy % 2;
		int num5 = 8 * num4 + 4 * num3;
		int num6 = (int)this.grids[num, num2];
		int num7 = (int)uValue << num5;
		ushort num8 = (ushort)((num6 & ~(num7 ^ (num6 & 15 << num5))) | num7);
		this.grids[num, num2] = num8;
	}

	public Vector3 GetPostion(int x, int y)
	{
		Vector3 zero = Vector3.zero;
		zero.x = (float)((x << 1) - this.miHalfOrignWidth) * this.gridSize;
		zero.z = (float)((y << 1) - this.miHalfOrignHeight) * this.gridSize;
		return zero;
	}

	public void Read(int iWidth, int iHeight, BinaryReader br)
	{
		for (int i = 0; i < iWidth; i++)
		{
			for (int j = 0; j < iHeight; j++)
			{
				ushort uValue;
				if (br.ReadByte() == 0)
				{
					uValue = 0;
				}
				else
				{
					uValue = 1;
				}
				this.SetGridValue(i, j, uValue);
			}
		}
		this.mPathfinder = new PathFinderFast(this.grids, this.miHalfOrignWidth, this.miHalfOrignHeight, this.gridSize);
	}

	public int[,] CheckCustomGrids(int[,] customGrids)
	{
		if (customGrids == null)
		{
			return customGrids;
		}
		int num = 0;
		List<int> list = new List<int>();
		int length = customGrids.GetLength(0);
		for (int i = 0; i < length; i++)
		{
			int num2 = customGrids[i, 0] >> 1;
			num = customGrids[i, 1] >> 1;
			int gridValue = (int)this.GetGridValue(num2, num);
			if ((gridValue & 1) == 0)
			{
				list.Add(num2);
				list.Add(num);
			}
		}
		customGrids = new int[list.Count / 2, 2];
		int num3 = 0;
		for (int i = 0; i < list.Count; i++)
		{
			int num2 = list[i];
			if (i + 1 < list.Count)
			{
				num = list[i + 1];
			}
			i++;
			customGrids[num3, 0] = num2;
			customGrids[num3, 1] = num;
			num3++;
		}
		return customGrids;
	}

	public void SetDynamicCollision(Vector3 worldPostion, int[,] customGrids, bool isRemove = false, int type = 0)
	{
		if (this.grids == null || customGrids == null)
		{
			return;
		}
		int length = customGrids.GetLength(0);
		for (int i = 0; i < length; i++)
		{
			int ix = customGrids[i, 0];
			int iy = customGrids[i, 1];
			byte b = this.GetGridValue(ix, iy);
			if (!isRemove)
			{
				b |= 8;
				this.SetGridValue(ix, iy, (ushort)b);
			}
			else
			{
				byte b2 = 8;
				b &= ~b2;
				this.SetGridValue(ix, iy, (ushort)b);
			}
		}
	}

	public void SetDynamicCollision(Vector3 worldPostion, int size = 1, bool isRemove = false, int type = 1)
	{
		if (this.grids == null)
		{
			return;
		}
		int num = Mathf.FloorToInt(worldPostion.x / this.gridSize) + this.miHalfOrignWidth;
		int num2 = Mathf.FloorToInt(worldPostion.z / this.gridSize) + this.miHalfOrignHeight;
		if (size > 0)
		{
			size--;
		}
		for (int i = -size; i <= size; i++)
		{
			for (int j = -size; j <= size; j++)
			{
				byte b = this.GetGridValue(num + i, num2 + j);
				if ((b & 9) <= 0)
				{
					if (!isRemove)
					{
						b |= 4;
						this.SetGridValue(num + i, num2 + j, (ushort)b);
					}
					else
					{
						byte b2 = 4;
						b &= ~b2;
						this.SetGridValue(num + i, num2 + j, (ushort)b);
					}
				}
			}
		}
	}

	public bool IsValidForWalk(Vector3 postion, int collisionSize = 1)
	{
		int num = Mathf.FloorToInt(postion.x / this.gridSize);
		int num2 = Mathf.FloorToInt(postion.z / this.gridSize);
		int num3 = num + this.miHalfOrignWidth;
		int num4 = num2 + this.miHalfOrignHeight;
		if (num3 < 0 || num4 < 0 || num3 > this.miOrignWidth || num4 > this.miOrignHeight)
		{
			return false;
		}
		if ((this.GetGridValue(num3, num4) & 13) > 0)
		{
			return false;
		}
		if (collisionSize > 1)
		{
			for (int i = 0; i < 8; i++)
			{
				int num5 = num3 + (int)this.mDirection[i, 0];
				int num6 = num4 + (int)this.mDirection[i, 1];
				if (num5 >= 0 && num6 >= 0 && num5 <= this.miOrignWidth && num6 <= this.miOrignHeight)
				{
					if ((this.GetGridValue(num5, num6) & 13) > 0)
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	public bool IsValidForWalk(Vector3 postion, int collisionSize, out int iGridStatus)
	{
		int num = Mathf.FloorToInt(postion.x / this.gridSize);
		int num2 = Mathf.FloorToInt(postion.z / this.gridSize);
		int num3 = num + this.miHalfOrignWidth;
		int num4 = num2 + this.miHalfOrignHeight;
		if (num3 < 0 || num4 < 0 || num3 > this.miOrignWidth || num4 > this.miOrignHeight)
		{
			iGridStatus = 1;
			return false;
		}
		byte gridValue = this.GetGridValue(num3, num4);
		if ((gridValue & 13) > 0)
		{
			iGridStatus = (int)gridValue;
			return false;
		}
		if (collisionSize != 1)
		{
			for (int i = 0; i < 8; i++)
			{
				int num5 = num3 + (int)this.mDirection[i, 0];
				int num6 = num4 + (int)this.mDirection[i, 1];
				if (num5 >= 0 && num6 >= 0 && num5 <= this.miOrignWidth && num6 <= this.miOrignHeight)
				{
					gridValue = this.GetGridValue(num5, num6);
					if ((gridValue & 13) > 0)
					{
						iGridStatus = (int)gridValue;
						return false;
					}
				}
			}
		}
		iGridStatus = 0;
		return true;
	}

	public void Clear()
	{
		this.pathFindEndBack = null;
		this.grids = null;
		this.paths.Clear();
	}

	public void RequestPaths(Vector3 startPoint, Vector3 endPoint, int collisionSize, out List<Vector3> paths)
	{
		this.RequestPaths(startPoint, endPoint, collisionSize, null, 2000000);
		paths = this.paths;
	}

	private Color GetGridColor(byte bValue)
	{
		if ((bValue & 1) > 0)
		{
			return Color.red;
		}
		if ((bValue & 2) > 0)
		{
			return Color.cyan;
		}
		if ((bValue & 4) > 0)
		{
			return Color.yellow;
		}
		if ((bValue & 8) > 0)
		{
			return Color.magenta;
		}
		return Color.white;
	}

	private void PrintTextures(string strFileName, ushort[,] grid, List<Vector3> list)
	{
		try
		{
			Texture2D texture2D = new Texture2D(this.miOrignWidth, this.miOrignHeight);
			for (int i = 0; i < this.miOrignWidth; i++)
			{
				for (int j = 0; j < this.miOrignHeight; j++)
				{
					byte gridValue = this.GetGridValue2(i, j, grid);
					texture2D.SetPixel(i, j, this.GetGridColor(gridValue));
				}
			}
			if (list != null)
			{
				for (int k = 0; k < list.Count; k++)
				{
					Vector3 vector = list[k];
					int num = Mathf.FloorToInt(vector.x / this.gridSize) + this.miHalfOrignWidth;
					int num2 = Mathf.FloorToInt(vector.z / this.gridSize) + this.miHalfOrignHeight;
					byte gridValue2 = this.GetGridValue2(num, num2, grid);
					if ((gridValue2 & 13) > 0)
					{
						texture2D.SetPixel(num, num2, Color.blue);
					}
					else
					{
						texture2D.SetPixel(num, num2, Color.green);
					}
				}
			}
			texture2D.Apply();
			byte[] buffer = texture2D.EncodeToPNG();
			FileStream fileStream = File.Open(strFileName, FileMode.Create);
			BinaryWriter binaryWriter = new BinaryWriter(fileStream);
			binaryWriter.Write(buffer);
			fileStream.Close();
		}
		catch (Exception ex)
		{
			LogSystem.LogError(new object[]
			{
				ex.ToString()
			});
		}
	}

	public void RequestPaths(Vector3 startPoint, Vector3 endPoint, int collisionSize, MapPath.PathFindEndBack pathFindEndBack = null, int maxComputeCount = 8000)
	{
		startPoint.x = (float)(Mathf.FloorToInt(startPoint.x / this.gridSize) + this.miHalfOrignWidth);
		startPoint.z = (float)(Mathf.FloorToInt(startPoint.z / this.gridSize) + this.miHalfOrignHeight);
		endPoint.x = (float)(Mathf.FloorToInt(endPoint.x / this.gridSize) + this.miHalfOrignWidth);
		endPoint.z = (float)(Mathf.FloorToInt(endPoint.z / this.gridSize) + this.miHalfOrignHeight);
		if (this.mPathfinder != null)
		{
			this.paths.Clear();
			int num = this.mPathfinder.FindPath(startPoint, endPoint, ref this.paths);
			if (num > 0 && MapPath.mOnFindPathFailed != null)
			{
				MapPath.mOnFindPathFailed(num);
			}
		}
		this.pathFindResult = (this.paths.Count > 0);
		if (pathFindEndBack != null)
		{
			pathFindEndBack(this.paths);
		}
	}
}
