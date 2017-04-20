using System;
using UnityEngine;

public class legend_god_matrix_godList : BaseVoList
{
	[SerializeField]
	public legend_god_matrix_godClass[] list;

	public override void Destroy()
	{
		this.list = new legend_god_matrix_godClass[0];
	}
}
