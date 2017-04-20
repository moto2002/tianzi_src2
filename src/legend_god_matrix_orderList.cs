using System;
using UnityEngine;

public class legend_god_matrix_orderList : BaseVoList
{
	[SerializeField]
	public legend_god_matrix_orderClass[] list;

	public override void Destroy()
	{
		this.list = new legend_god_matrix_orderClass[0];
	}
}
