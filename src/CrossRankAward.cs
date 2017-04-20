using System;

[Serializable]
public class CrossRankAward
{
	public int start;

	public int end;

	public string desc;

	public string award;

	public string rankName;

	public string rankDesc;

	public bool Contains(int rank)
	{
		if (this.end < 0)
		{
			return rank >= this.start;
		}
		return rank >= this.start && rank <= this.end;
	}
}
