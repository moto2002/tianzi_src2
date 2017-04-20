using System;
using System.Collections.Generic;

[Serializable]
public class FaBaoLevelPackgeInfo
{
	public int key;

	public int Cloud;

	public int Order;

	public int Level;

	public List<Package> Packges = new List<Package>();
}
