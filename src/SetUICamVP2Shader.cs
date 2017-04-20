using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SetUICamVP2Shader : MonoBehaviour
{
	private Camera mCamera;

	private float size;

	private float Size
	{
		set
		{
			if (this.size - value > 0.01f || this.size - value < -0.01f)
			{
				this.size = value;
				this.SetCamera();
			}
		}
	}

	private void Awake()
	{
		this.mCamera = base.GetComponent<Camera>();
	}

	private void Start()
	{
		this.Size = this.mCamera.orthographicSize;
	}

	private void SetCamera()
	{
		Matrix4x4 matrix4x = default(Matrix4x4);
		float num = this.size * this.mCamera.aspect;
		float nearClipPlane = this.mCamera.nearClipPlane;
		float farClipPlane = this.mCamera.farClipPlane;
		float value = 1f / num;
		float value2 = 1f / this.size;
		float value3 = -2f / (farClipPlane - nearClipPlane);
		float value4 = -nearClipPlane / (farClipPlane - nearClipPlane);
		matrix4x[0, 0] = value;
		matrix4x[0, 1] = 0f;
		matrix4x[0, 2] = 0f;
		matrix4x[0, 3] = 0f;
		matrix4x[1, 0] = 0f;
		matrix4x[1, 1] = value2;
		matrix4x[1, 2] = 0f;
		matrix4x[1, 3] = 0f;
		matrix4x[2, 0] = 0f;
		matrix4x[2, 1] = 0f;
		matrix4x[2, 2] = value3;
		matrix4x[2, 3] = value4;
		matrix4x[3, 0] = 0f;
		matrix4x[3, 1] = 0f;
		matrix4x[3, 2] = 0f;
		matrix4x[3, 3] = 1f;
		Matrix4x4 mat = this.mCamera.projectionMatrix * this.mCamera.worldToCameraMatrix;
		Shader.SetGlobalMatrix("UI_CAM_MATRIX_VP", mat);
	}

	private void LateUpdate()
	{
		this.Size = this.mCamera.orthographicSize;
	}
}
