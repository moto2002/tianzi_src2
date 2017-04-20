using System;
using System.Collections.Generic;
using UnityEngine;

namespace SkillEditor
{
	[Serializable]
	public class USPropertyInfoSerialize
	{
		public PropertyTypeInfo propertyType = PropertyTypeInfo.None;

		public Component component = new Component();

		public string componentType = string.Empty;

		public string propertyName = string.Empty;

		public string fieldName = string.Empty;

		public List<USInternalCurveSerialize> newCurvesSerialize = new List<USInternalCurveSerialize>();

		public int baseInt;

		public long baseLong;

		public float baseFloat;

		public double baseDouble;

		public bool baseBool;

		public Vector2 baseVector2 = Vector2.zero;

		public Vector3 baseVector3 = Vector3.zero;

		public Vector4 baseVector4 = Vector4.zero;

		public Quaternion baseQuat = Quaternion.identity;

		public Color baseColour = Color.black;

		public float previousTime = -1f;

		public string internalName = string.Empty;

		public void Copy(USPropertyInfo propertyInfo)
		{
			this.componentType = propertyInfo.ComponentType;
			this.component = propertyInfo.Component;
			this.propertyName = propertyInfo.propertyName;
			this.fieldName = propertyInfo.fieldName;
			this.internalName = propertyInfo.InternalName;
			this.previousTime = propertyInfo.previousTime;
			this.propertyType = propertyInfo.PropertyType;
			this.baseInt = (int)propertyInfo.GetPropertyValue(PropertyTypeInfo.Int);
			this.baseLong = (long)propertyInfo.GetPropertyValue(PropertyTypeInfo.Long);
			this.baseFloat = (float)propertyInfo.GetPropertyValue(PropertyTypeInfo.Float);
			this.baseDouble = (double)propertyInfo.GetPropertyValue(PropertyTypeInfo.Double);
			this.baseBool = (bool)propertyInfo.GetPropertyValue(PropertyTypeInfo.Bool);
			this.baseVector2 = (Vector2)propertyInfo.GetPropertyValue(PropertyTypeInfo.Vec2);
			this.baseVector3 = (Vector3)propertyInfo.GetPropertyValue(PropertyTypeInfo.Vec3);
			this.baseVector4 = (Vector4)propertyInfo.GetPropertyValue(PropertyTypeInfo.Vec4);
			this.baseQuat = (Quaternion)propertyInfo.GetPropertyValue(PropertyTypeInfo.Quat);
			this.baseColour = (Color)propertyInfo.GetPropertyValue(PropertyTypeInfo.Colour);
			if (propertyInfo.curves.Count > 0)
			{
				for (int i = 0; i < propertyInfo.curves.Count; i++)
				{
					USInternalCurve uSInternalCurve = propertyInfo.curves[i];
					if (!(uSInternalCurve == null))
					{
						USInternalCurveSerialize uSInternalCurveSerialize = new USInternalCurveSerialize();
						uSInternalCurveSerialize.Copy(uSInternalCurve);
						this.newCurvesSerialize.Add(uSInternalCurveSerialize);
					}
				}
			}
		}
	}
}
