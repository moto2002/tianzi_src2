using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class NcDrawFpsRect : MonoBehaviour
{
	public bool centerTop = true;

	public Rect startRect = new Rect(0f, 0f, 75f, 50f);

	public bool updateColor = true;

	public bool allowDrag = true;

	public float frequency = 0.5f;

	public int nbDecimal = 1;

	private float accum;

	private int frames;

	private Color color = Color.white;

	private string sFPS = string.Empty;

	private GUIStyle style;

	private void Start()
	{
		base.StartCoroutine(this.FPS());
	}

	private void Update()
	{
		this.accum += Time.timeScale / Time.deltaTime;
		this.frames++;
	}

	[DebuggerHidden]
	private IEnumerator FPS()
	{
		NcDrawFpsRect.<FPS>c__IteratorC <FPS>c__IteratorC = new NcDrawFpsRect.<FPS>c__IteratorC();
		<FPS>c__IteratorC.<>f__this = this;
		return <FPS>c__IteratorC;
	}

	private void DoMyWindow(int windowID)
	{
		GUI.Label(new Rect(0f, 0f, this.startRect.width, this.startRect.height), this.sFPS + " FPS", this.style);
		if (this.allowDrag)
		{
			GUI.DragWindow(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height));
		}
	}
}
