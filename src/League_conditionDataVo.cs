using System;

[Serializable]
public class League_conditionDataVo
{
	public enum MatchType
	{
		LeagueWar = 1,
		PersonWar
	}

	public int Matchtype;

	public int GuildLv;

	public int PlayerLv;

	public int Cost;
}
