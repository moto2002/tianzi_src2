using System;

[Serializable]
public class AchievementData
{
	public string key;

	public string id;

	public string before;

	public string next;

	public int curProgress;

	public int totalProgress;

	public bool Complete
	{
		get
		{
			return this.curProgress >= this.totalProgress;
		}
	}

	public double Percent
	{
		get
		{
			if (this.curProgress >= this.totalProgress)
			{
				return 100.0;
			}
			return Math.Round((double)this.curProgress / (double)this.totalProgress, 2) * 100.0;
		}
	}

	public bool Big(int progress)
	{
		return progress > this.curProgress;
	}

	public override string ToString()
	{
		return "key-->" + this.key + "  Percent--> " + this.Percent.ToString();
	}
}
