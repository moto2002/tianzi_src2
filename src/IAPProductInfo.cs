using System;

public class IAPProductInfo
{
	private int no;

	private string pId;

	private string title;

	private string desc;

	private string price;

	private string priceDesc;

	private string priceSymbol;

	public string PId
	{
		get
		{
			return this.pId;
		}
	}

	public string Title
	{
		get
		{
			return this.title;
		}
	}

	public string Desc
	{
		get
		{
			return this.desc;
		}
	}

	public string Price
	{
		get
		{
			return this.price;
		}
	}

	public int NO
	{
		get
		{
			return this.no;
		}
		set
		{
			this.no = value;
		}
	}

	public string PriceDesc
	{
		get
		{
			return this.priceDesc;
		}
	}

	public string PriceSymbol
	{
		get
		{
			return this.priceSymbol;
		}
	}

	public IAPProductInfo(string pId, string title, string desc, string price, string priceDesc, string priceSymbol)
	{
		this.pId = pId;
		this.title = title;
		this.desc = desc;
		this.price = price;
		this.priceDesc = priceDesc;
		this.priceSymbol = priceSymbol;
	}
}
