using System;

[Serializable]
public class Package
{
	public int id;

	public int PackageID;

	public Package(int id, int packageId)
	{
		this.id = id;
		this.PackageID = packageId;
	}
}
