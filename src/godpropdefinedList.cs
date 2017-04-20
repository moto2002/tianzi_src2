using System;
using UnityEngine;

public class godpropdefinedList : BaseVoList
{
	[SerializeField]
	public godpropdefinedClass[] list;

	public override void Destroy()
	{
		this.list = new godpropdefinedClass[0];
	}
}
