using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Algorithms
{
	public class PathFinderFast
	{
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		internal struct PathFinderNodeFast
		{
			public ushort F;

			public ushort G;

			public ushort PX;

			public ushort PY;

			public byte Status;
		}

		internal class ComparePFNodeMatrix : IComparer<int>
		{
			private PathFinderFast.PathFinderNodeFast[] mMatrix;

			public ComparePFNodeMatrix(PathFinderFast.PathFinderNodeFast[] matrix)
			{
				this.mMatrix = matrix;
			}

			public int Compare(int a, int b)
			{
				if (this.mMatrix[a].F > this.mMatrix[b].F)
				{
					return 1;
				}
				if (this.mMatrix[a].F < this.mMatrix[b].F)
				{
					return -1;
				}
				return 0;
			}
		}

		public ushort[,] mGrid;

		private PriorityQueueB<int> mOpen;

		private List<PathFinderNode> mClose = new List<PathFinderNode>();

		private bool mStop;

		private bool mStopped = true;

		private int mHoriz;

		private HeuristicFormula mFormula = HeuristicFormula.Manhattan;

		private bool mDiagonals;

		private ushort mHEstimate = 2;

		private bool mPunishChangeDirection;

		private bool mTieBreaker;

		private bool mHeavyDiagonals;

		private int mSearchLimit = 180000;

		private double mCompletedTime;

		private bool mDebugProgress;

		private bool mDebugFoundPath;

		private PathFinderFast.PathFinderNodeFast[] mCalcGrid;

		private byte mOpenNodeValue = 1;

		private byte mCloseNodeValue = 2;

		private int mH;

		private int mLocation;

		private int mNewLocation;

		private ushort mLocationX;

		private ushort mLocationY;

		private ushort mNewLocationX;

		private ushort mNewLocationY;

		private int mCloseNodeCounter;

		private ushort mGridX;

		private ushort mGridY;

		private ushort mGridXMinus1;

		private ushort mGridYLog2;

		private ushort mOrginGridX;

		private ushort mOrginGridY;

		private bool mFound;

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

		private int mEndLocation;

		private int mNewG;

		private int miHalfWidth;

		private int miHalfHeight;

		private float mfGridSize = 0.5f;

		public bool Stopped
		{
			get
			{
				return this.mStopped;
			}
		}

		public HeuristicFormula Formula
		{
			get
			{
				return this.mFormula;
			}
			set
			{
				this.mFormula = value;
			}
		}

		public bool Diagonals
		{
			get
			{
				return this.mDiagonals;
			}
			set
			{
				this.mDiagonals = value;
			}
		}

		public bool HeavyDiagonals
		{
			get
			{
				return this.mHeavyDiagonals;
			}
			set
			{
				this.mHeavyDiagonals = value;
			}
		}

		public ushort HeuristicEstimate
		{
			get
			{
				return this.mHEstimate;
			}
			set
			{
				this.mHEstimate = value;
			}
		}

		public bool PunishChangeDirection
		{
			get
			{
				return this.mPunishChangeDirection;
			}
			set
			{
				this.mPunishChangeDirection = value;
			}
		}

		public bool TieBreaker
		{
			get
			{
				return this.mTieBreaker;
			}
			set
			{
				this.mTieBreaker = value;
			}
		}

		public int SearchLimit
		{
			get
			{
				return this.mSearchLimit;
			}
			set
			{
				this.mSearchLimit = value;
			}
		}

		public double CompletedTime
		{
			get
			{
				return this.mCompletedTime;
			}
			set
			{
				this.mCompletedTime = value;
			}
		}

		public bool DebugProgress
		{
			get
			{
				return this.mDebugProgress;
			}
			set
			{
				this.mDebugProgress = value;
			}
		}

		public bool DebugFoundPath
		{
			get
			{
				return this.mDebugFoundPath;
			}
			set
			{
				this.mDebugFoundPath = value;
			}
		}

		public PathFinderFast(ushort[,] grid, int iHalfWidth, int iHalfHeight, float fGridSize)
		{
			this.miHalfWidth = iHalfWidth;
			this.miHalfHeight = iHalfHeight;
			this.mfGridSize = fGridSize;
			if (grid != null)
			{
				int upperBound = grid.GetUpperBound(0);
				int upperBound2 = grid.GetUpperBound(1);
				this.mOrginGridX = (ushort)upperBound;
				this.mOrginGridY = (ushort)upperBound2;
				int num = this.GetNearestPower(upperBound) * 2;
				int num2 = this.GetNearestPower(upperBound2) * 2;
				this.mGrid = grid;
				this.mGridX = (ushort)num;
				this.mGridY = (ushort)num2;
				this.mGridXMinus1 = this.mGridX - 1;
				this.mGridYLog2 = (ushort)Math.Log((double)this.mGridY, 2.0);
				if (this.mCalcGrid == null || this.mCalcGrid.Length != (int)(this.mGridX * this.mGridY))
				{
					this.mCalcGrid = new PathFinderFast.PathFinderNodeFast[(int)(this.mGridX * this.mGridY)];
				}
				this.mOpen = new PriorityQueueB<int>(new PathFinderFast.ComparePFNodeMatrix(this.mCalcGrid));
			}
		}

		public int GetNearestPower(int iBound)
		{
			int num = 1;
			while (iBound / 2 != 0)
			{
				num++;
				iBound /= 2;
			}
			return 1 << num;
		}

		public void FindPathStop()
		{
			this.mStop = true;
		}

		public bool IsGridWalkable(int ix, int iy)
		{
			ushort num = (ushort)(ix >> 1);
			ushort num2 = (ushort)(iy >> 1);
			ushort num3 = (ushort)(ix % 2);
			ushort num4 = (ushort)(iy % 2);
			return num >= 0 && num2 >= 0 && num < this.mOrginGridX && num2 < this.mOrginGridY && ((byte)(this.mGrid[(int)num, (int)num2] >> (int)(8 * num4 + 4 * num3) & 15) & 11) == 0;
		}

		public int FindPath(Vector3 vstart, Vector3 vend, ref List<Vector3> paths)
		{
			this.mFound = false;
			this.mStop = false;
			this.mStopped = false;
			this.mCloseNodeCounter = 0;
			this.mOpenNodeValue += 2;
			this.mCloseNodeValue += 2;
			this.mOpen.Clear();
			this.mClose.Clear();
			paths.Clear();
			int num = (int)vstart.x;
			int num2 = (int)vstart.z;
			int num3 = (int)vend.x;
			int num4 = (int)vend.z;
			if (!this.IsGridWalkable(num, num2))
			{
				for (int i = 0; i < ((!this.mDiagonals) ? 4 : 8); i++)
				{
					if (this.IsGridWalkable(num + (int)this.mDirection[i, 0], num2 + (int)this.mDirection[i, 1]))
					{
						num += (int)this.mDirection[i, 0];
						num2 += (int)this.mDirection[i, 1];
						break;
					}
				}
				if (!this.IsGridWalkable(num, num2))
				{
					return 2;
				}
			}
			if (!this.IsGridWalkable(num3, num4))
			{
				for (int j = 0; j < ((!this.mDiagonals) ? 4 : 8); j++)
				{
					if (this.IsGridWalkable(num3 + (int)this.mDirection[j, 0], num4 + (int)this.mDirection[j, 1]))
					{
						num3 += (int)this.mDirection[j, 0];
						num4 += (int)this.mDirection[j, 1];
						break;
					}
				}
				if (!this.IsGridWalkable(num3, num4))
				{
					return 3;
				}
			}
			this.mLocation = (num2 << (int)this.mGridYLog2) + num;
			this.mEndLocation = (num4 << (int)this.mGridYLog2) + num3;
			this.mCalcGrid[this.mLocation].G = 0;
			this.mCalcGrid[this.mLocation].F = this.mHEstimate;
			this.mCalcGrid[this.mLocation].PX = (ushort)num;
			this.mCalcGrid[this.mLocation].PY = (ushort)num2;
			this.mCalcGrid[this.mLocation].Status = this.mOpenNodeValue;
			this.mOpen.Push(this.mLocation);
			while (this.mOpen.Count > 0 && !this.mStop)
			{
				this.mLocation = this.mOpen.Pop();
				if (this.mCalcGrid[this.mLocation].Status != this.mCloseNodeValue)
				{
					this.mLocationX = (ushort)(this.mLocation & (int)this.mGridXMinus1);
					this.mLocationY = (ushort)(this.mLocation >> (int)this.mGridYLog2);
					if (this.mLocation == this.mEndLocation)
					{
						this.mCalcGrid[this.mLocation].Status = this.mCloseNodeValue;
						this.mFound = true;
						break;
					}
					if (this.mCloseNodeCounter > this.mSearchLimit)
					{
						this.mStopped = true;
						return 4;
					}
					if (this.mPunishChangeDirection)
					{
						this.mHoriz = (int)(this.mLocationX - this.mCalcGrid[this.mLocation].PX);
					}
					for (int k = 0; k < ((!this.mDiagonals) ? 4 : 8); k++)
					{
						this.mNewLocationX = (ushort)((int)this.mLocationX + (int)this.mDirection[k, 0]);
						this.mNewLocationY = (ushort)((int)this.mLocationY + (int)this.mDirection[k, 1]);
						this.mNewLocation = ((int)this.mNewLocationY << (int)this.mGridYLog2) + (int)this.mNewLocationX;
						if (this.mNewLocationX < this.mGridX && this.mNewLocationY < this.mGridY)
						{
							if (this.IsGridWalkable((int)this.mNewLocationX, (int)this.mNewLocationY))
							{
								this.mNewG = (int)(this.mCalcGrid[this.mLocation].G + 1);
								if ((this.mCalcGrid[this.mNewLocation].Status != this.mOpenNodeValue && this.mCalcGrid[this.mNewLocation].Status != this.mCloseNodeValue) || (int)this.mCalcGrid[this.mNewLocation].G > this.mNewG)
								{
									this.mCalcGrid[this.mNewLocation].PX = this.mLocationX;
									this.mCalcGrid[this.mNewLocation].PY = this.mLocationY;
									this.mCalcGrid[this.mNewLocation].G = (ushort)this.mNewG;
									this.mH = (int)this.mHEstimate * (Math.Abs((int)this.mNewLocationX - num3) + Math.Abs((int)this.mNewLocationY - num4));
									if (this.mTieBreaker)
									{
										int num5 = (int)this.mLocationX - num3;
										int num6 = (int)this.mLocationY - num4;
										int num7 = num - num3;
										int num8 = num2 - num4;
										int num9 = Math.Abs(num5 * num8 - num7 * num6);
										this.mH = (int)((double)this.mH + (double)num9 * 0.001);
									}
									this.mCalcGrid[this.mNewLocation].F = (ushort)(this.mNewG + this.mH);
									this.mOpen.Push(this.mNewLocation);
									this.mCalcGrid[this.mNewLocation].Status = this.mOpenNodeValue;
								}
							}
						}
					}
					this.mCloseNodeCounter++;
					this.mCalcGrid[this.mLocation].Status = this.mCloseNodeValue;
				}
			}
			if (this.mFound)
			{
				this.mClose.Clear();
				PathFinderFast.PathFinderNodeFast pathFinderNodeFast = this.mCalcGrid[(num4 << (int)this.mGridYLog2) + num3];
				PathFinderNode item;
				item.F = (int)pathFinderNodeFast.F;
				item.G = (int)pathFinderNodeFast.G;
				item.H = 0;
				item.PX = (int)pathFinderNodeFast.PX;
				item.PY = (int)pathFinderNodeFast.PY;
				item.X = num3;
				item.Y = num4;
				while (item.X != item.PX || item.Y != item.PY)
				{
					this.mClose.Add(item);
					int pX = item.PX;
					int pY = item.PY;
					pathFinderNodeFast = this.mCalcGrid[(pY << (int)this.mGridYLog2) + pX];
					item.F = (int)pathFinderNodeFast.F;
					item.G = (int)pathFinderNodeFast.G;
					item.H = 0;
					item.PX = (int)pathFinderNodeFast.PX;
					item.PY = (int)pathFinderNodeFast.PY;
					item.X = pX;
					item.Y = pY;
				}
				this.mClose.Add(item);
				this.mStopped = true;
				this.OptimizePath(ref paths);
				return (paths.Count <= 0) ? 1 : 0;
			}
			this.mStopped = true;
			return 1;
		}

		private void OptimizePath(ref List<Vector3> vPaths)
		{
			this.mClose.Reverse();
			for (int i = 0; i < this.mClose.Count; i++)
			{
				PathFinderNode value = this.mClose[i];
				int num = 0;
				int num2 = 0;
				for (int j = 0; j < 8; j++)
				{
					if (!this.IsGridWalkable(value.X + (int)this.mDirection[j, 0], value.Y + (int)this.mDirection[j, 1]))
					{
						num -= (int)this.mDirection[j, 0];
						num2 -= (int)this.mDirection[j, 1];
					}
				}
				value.X += ((num == 0) ? 0 : (num / Mathf.Abs(num)));
				value.Y += ((num2 == 0) ? 0 : (num2 / Mathf.Abs(num2)));
				this.mClose[i] = value;
			}
			for (int k = 2; k < this.mClose.Count; k++)
			{
				PathFinderNode last = this.mClose[k - 2];
				PathFinderNode mid = this.mClose[k - 1];
				PathFinderNode end = this.mClose[k];
				if (this.IsSameDirection(last, mid, end))
				{
					this.mClose.RemoveAt(k - 1);
					k--;
				}
			}
			for (int l = 0; l < this.mClose.Count; l++)
			{
				PathFinderNode start = this.mClose[l];
				for (int m = this.mClose.Count - 1; m > l; m--)
				{
					PathFinderNode end2 = this.mClose[m];
					if (this.IsGridLineWalked(start, end2, this.mfGridSize))
					{
						this.mClose.RemoveRange(l + 1, m - l - 1);
						break;
					}
				}
			}
			Vector3 zero = Vector3.zero;
			for (int n = 0; n < this.mClose.Count; n++)
			{
				PathFinderNode pathFinderNode = this.mClose[n];
				zero.x = (float)(pathFinderNode.X - this.miHalfWidth) * this.mfGridSize;
				zero.z = (float)(pathFinderNode.Y - this.miHalfHeight) * this.mfGridSize;
				vPaths.Add(zero);
			}
		}

		private bool IsSameDirection(PathFinderNode last, PathFinderNode mid, PathFinderNode end)
		{
			int num = last.X - mid.X;
			int num2 = last.Y - mid.Y;
			int num3 = end.X - mid.X;
			int num4 = end.Y - mid.Y;
			return num * num4 == num3 * num2;
		}

		private bool IsGridLineWalked(PathFinderNode start, PathFinderNode end, float fGridSize = 0.5f)
		{
			if (start.X == end.X && start.Y == end.Y)
			{
				return true;
			}
			if (start.X == end.X)
			{
				int x = start.X;
				float num = (float)((start.Y <= end.Y) ? start.Y : end.Y);
				float num2 = (float)((start.Y <= end.Y) ? end.Y : start.Y);
				for (float num3 = num; num3 < num2; num3 += fGridSize)
				{
					if (!this.IsGridWalkable(x, (int)Math.Floor((double)num3)))
					{
						return false;
					}
				}
			}
			else if (start.Y == end.Y)
			{
				int y = start.Y;
				float num4 = (float)((start.X <= end.X) ? start.X : end.X);
				float num5 = (float)((start.X <= end.X) ? end.X : start.X);
				for (float num6 = num4; num6 < num5; num6 += fGridSize)
				{
					if (!this.IsGridWalkable((int)Math.Floor((double)num6), y))
					{
						return false;
					}
				}
			}
			else
			{
				int value = start.X - end.X;
				int value2 = start.Y - end.Y;
				float num7 = (float)start.Y;
				float num8 = (float)end.Y;
				float num9 = (float)start.X;
				float num10 = (float)end.X;
				if (Math.Abs(value) > Math.Abs(value2))
				{
					float num11 = num7 - num8;
					num11 /= num9 - num10;
					float num12 = (num7 * num10 - num8 * num9) / (num10 - num9);
					float num13 = (float)((start.X <= end.X) ? start.X : end.X);
					float num14 = (float)((start.X <= end.X) ? end.X : start.X);
					for (float num15 = num13; num15 < num14; num15 += fGridSize)
					{
						float num16 = num11 * num15 + num12;
						if (!this.IsGridWalkable((int)Math.Floor((double)num15), (int)Math.Floor((double)num16)))
						{
							return false;
						}
						if (!this.IsGridWalkable((int)Math.Floor((double)num15) - 1, (int)Math.Floor((double)num16)))
						{
							return false;
						}
					}
				}
				else
				{
					float num17 = num7 - num8;
					num17 /= num9 - num10;
					float num18 = (num7 * num10 - num8 * num9) / (num10 - num9);
					float num19 = (float)((start.Y <= end.Y) ? start.Y : end.Y);
					float num20 = (float)((start.Y <= end.Y) ? end.Y : start.Y);
					for (float num21 = num19; num21 < num20; num21 += fGridSize)
					{
						float num22 = (num21 - num18) / num17;
						if (!this.IsGridWalkable((int)Math.Floor((double)num22), (int)Math.Floor((double)num21)))
						{
							return false;
						}
						if (!this.IsGridWalkable((int)Math.Floor((double)num22), (int)Math.Floor((double)num21) - 1))
						{
							return false;
						}
					}
				}
			}
			return true;
		}
	}
}
