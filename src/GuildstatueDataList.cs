using System;
using UnityEngine;

public class GuildstatueDataList : BaseVoList
{
	[SerializeField]
	public GuildstatueDataVo[] list;

	public float GetOffsetHight(int rank)
	{
		if (this.list == null)
		{
			return 0f;
		}
		for (int i = 0; i < this.list.Length; i++)
		{
			GuildstatueDataVo guildstatueDataVo = this.list[i];
			if (guildstatueDataVo != null && guildstatueDataVo.Rank == rank)
			{
				return guildstatueDataVo.Offset;
			}
		}
		return 0f;
	}

	public string GetModelName(int rank)
	{
		if (this.list == null)
		{
			return string.Empty;
		}
		for (int i = 0; i < this.list.Length; i++)
		{
			GuildstatueDataVo guildstatueDataVo = this.list[i];
			if (guildstatueDataVo != null && guildstatueDataVo.Rank == rank)
			{
				return guildstatueDataVo.Modle;
			}
		}
		return string.Empty;
	}

	public string GetTile(int rank)
	{
		if (this.list == null)
		{
			return string.Empty;
		}
		for (int i = 0; i < this.list.Length; i++)
		{
			GuildstatueDataVo guildstatueDataVo = this.list[i];
			if (guildstatueDataVo != null && guildstatueDataVo.Rank == rank)
			{
				return guildstatueDataVo.Tile;
			}
		}
		return string.Empty;
	}

	public string GetModePosition(int rank, int Nation)
	{
		if (this.list == null)
		{
			return string.Empty;
		}
		for (int i = 0; i < this.list.Length; i++)
		{
			GuildstatueDataVo guildstatueDataVo = this.list[i];
			if (guildstatueDataVo != null && guildstatueDataVo.Rank == rank)
			{
				if (Nation == 1)
				{
					return guildstatueDataVo.Pos1;
				}
				if (Nation == 2)
				{
					return guildstatueDataVo.Pos2;
				}
			}
		}
		return string.Empty;
	}

	public override void Destroy()
	{
		this.list = new GuildstatueDataVo[0];
	}
}
