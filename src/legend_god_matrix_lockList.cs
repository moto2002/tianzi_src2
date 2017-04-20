using System;
using UnityEngine;

public class legend_god_matrix_lockList : BaseVoList
{
	[SerializeField]
	public legend_god_matrix_lockClass[] list;

	public override void Destroy()
	{
		this.list = new legend_god_matrix_lockClass[0];
	}
}
