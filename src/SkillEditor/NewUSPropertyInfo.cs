using SkillEditor.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace SkillEditor
{
	public class NewUSPropertyInfo
	{
		public PropertyTypeInfo propertyType = PropertyTypeInfo.None;

		public Component component;

		public string componentType;

		public string propertyName;

		public string fieldName;

		public PropertyInfo cachedPropertyInfo;

		public FieldInfo cachedFieldInfo;

		public List<NewUSInternalCurve> newCurves = new List<NewUSInternalCurve>();

		[Obsolete("curves0 - curves 4 is now Obsolete, please use the curves list, instead.")]
		public AnimationCurve curves0;

		[Obsolete("curves0 - curves 4 is now Obsolete, please use the curves list, instead.")]
		public AnimationCurve curves1;

		[Obsolete("curves0 - curves 4 is now Obsolete, please use the curves list, instead.")]
		public AnimationCurve curves2;

		[Obsolete("curves0 - curves 4 is now Obsolete, please use the curves list, instead.")]
		public AnimationCurve curves3;

		private bool tmpBool;

		private Keyframe tmpKeyframe = default(Keyframe);

		private Vector2 tmpVector2 = Vector2.zero;

		private Vector3 tmpVector3 = Vector3.zero;

		private Vector4 tmpVector4 = Vector4.zero;

		private Quaternion tmpQuat = Quaternion.identity;

		private Color tmpColour = Color.black;

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

		private string internalName;

		public PropertyInfo propertyInfo
		{
			get
			{
				if (this.cachedPropertyInfo != null)
				{
					return this.cachedPropertyInfo;
				}
				if (!this.component || string.IsNullOrEmpty(this.propertyName))
				{
					return null;
				}
				Type type = this.component.GetType();
				this.cachedPropertyInfo = PlatformSpecificFactory.ReflectionHelper.GetProperty(type, this.propertyName);
				return this.cachedPropertyInfo;
			}
			set
			{
				if (value != null)
				{
					this.propertyName = value.Name;
				}
				else
				{
					this.propertyName = null;
				}
				this.cachedPropertyInfo = value;
			}
		}

		public FieldInfo fieldInfo
		{
			get
			{
				if (this.cachedFieldInfo != null)
				{
					return this.cachedFieldInfo;
				}
				if (!this.component || string.IsNullOrEmpty(this.fieldName))
				{
					return null;
				}
				Type type = this.component.GetType();
				this.cachedFieldInfo = PlatformSpecificFactory.ReflectionHelper.GetField(type, this.fieldName);
				return this.cachedFieldInfo;
			}
			set
			{
				if (value != null)
				{
					this.fieldName = value.Name;
				}
				else
				{
					this.fieldName = null;
				}
				this.cachedFieldInfo = value;
			}
		}

		public bool UseCurrentValue
		{
			get
			{
				return this.newCurves.Any((NewUSInternalCurve curve) => curve.UseCurrentValue);
			}
			set
			{
				foreach (NewUSInternalCurve current in this.newCurves)
				{
					current.UseCurrentValue = value;
				}
			}
		}

		public Component Component
		{
			get
			{
				return this.component;
			}
			set
			{
				this.component = value;
				if (this.component != null)
				{
					this.ComponentType = this.component.GetType().ToString();
				}
			}
		}

		public string ComponentType
		{
			get
			{
				if (this.componentType == string.Empty && this.component)
				{
					this.componentType = this.component.GetType().ToString();
				}
				return this.componentType;
			}
			set
			{
				this.componentType = USRuntimeUtility.ConvertToSerializableName(value);
			}
		}

		public string PropertyName
		{
			get
			{
				if (this.fieldInfo != null)
				{
					return this.fieldInfo.Name;
				}
				if (this.propertyInfo != null)
				{
					return this.propertyInfo.Name;
				}
				throw new Exception("No Property");
			}
			private set
			{
			}
		}

		public PropertyTypeInfo PropertyType
		{
			get
			{
				return this.propertyType;
			}
		}

		public string InternalName
		{
			get
			{
				return this.internalName;
			}
			set
			{
				this.internalName = value;
			}
		}

		public void Init(USPropertyInfoSerialize fSerialize)
		{
			this.componentType = fSerialize.componentType;
			this.component = fSerialize.component;
			this.propertyName = fSerialize.propertyName;
			this.fieldName = fSerialize.fieldName;
			this.internalName = fSerialize.internalName;
			this.previousTime = fSerialize.previousTime;
			this.propertyType = fSerialize.propertyType;
			this.baseInt = fSerialize.baseInt;
			this.baseLong = fSerialize.baseLong;
			this.baseFloat = fSerialize.baseFloat;
			this.baseDouble = fSerialize.baseDouble;
			this.baseBool = fSerialize.baseBool;
			this.baseVector2 = fSerialize.baseVector2;
			this.baseVector3 = fSerialize.baseVector3;
			this.baseVector4 = fSerialize.baseVector4;
			this.baseQuat = fSerialize.baseQuat;
			this.baseColour = fSerialize.baseColour;
			if (fSerialize.newCurvesSerialize != null)
			{
				for (int i = 0; i < fSerialize.newCurvesSerialize.Count; i++)
				{
					USInternalCurveSerialize fSerialize2 = fSerialize.newCurvesSerialize[i];
					NewUSInternalCurve newUSInternalCurve = new NewUSInternalCurve();
					newUSInternalCurve.Init(fSerialize2);
					this.newCurves.Add(newUSInternalCurve);
				}
			}
		}

		public void CreatePropertyInfo(PropertyTypeInfo createdPropertyType)
		{
			if (this.propertyType != PropertyTypeInfo.None)
			{
				Debug.LogWarning("We are trying to CreatePropertyInfo, but it will be an overwrite!");
				return;
			}
			this.propertyType = createdPropertyType;
			this.CreatePropertyCurves();
		}

		public void SetValue(float time)
		{
			if (this.newCurves.Count == 0)
			{
				return;
			}
			if (this.propertyType == PropertyTypeInfo.None)
			{
				Debug.LogWarning("Atempting to Set A Value, before calling CreatePropertyInfo");
				return;
			}
			float num = float.PositiveInfinity;
			float num2 = 0f;
			for (int i = 0; i < this.newCurves.Count; i++)
			{
				NewUSInternalCurve newUSInternalCurve = this.newCurves[i];
				if (num > newUSInternalCurve.FirstKeyframeTime)
				{
					num = newUSInternalCurve.FirstKeyframeTime;
				}
				if (num2 < newUSInternalCurve.LastKeyframeTime)
				{
					num2 = newUSInternalCurve.LastKeyframeTime;
				}
			}
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			if (this.previousTime >= num && this.previousTime <= num2)
			{
				flag = true;
			}
			if (time >= num && time <= num2)
			{
				flag2 = true;
			}
			if ((this.previousTime < num && time > num2) || (this.previousTime > num2 && time < num))
			{
				flag3 = true;
			}
			if (flag3 && this.previousTime > time)
			{
				flag4 = true;
			}
			for (int j = 0; j < this.newCurves.Count; j++)
			{
				if (this.newCurves[j].UseCurrentValue && this.newCurves[j] != null)
				{
					flag5 = true;
					float firstKeyframeTime = this.newCurves[j].FirstKeyframeTime;
					bool flag6 = this.previousTime < firstKeyframeTime && time >= firstKeyframeTime;
					if (flag6)
					{
						this.BuildForCurrentValue(j);
					}
				}
			}
			this.previousTime = time;
			bool flag7 = false;
			if (flag3)
			{
				flag7 = true;
			}
			if (flag2)
			{
				flag7 = true;
			}
			if (!flag2 && flag)
			{
				flag7 = true;
			}
			if (flag4 && flag5)
			{
				flag7 = false;
			}
			if (!flag7)
			{
				return;
			}
			object obj = null;
			switch (this.propertyType)
			{
			case PropertyTypeInfo.Int:
				obj = (int)this.newCurves[0].Evaluate(time);
				break;
			case PropertyTypeInfo.Long:
				obj = this.newCurves[0].Evaluate(time);
				break;
			case PropertyTypeInfo.Float:
				obj = this.newCurves[0].Evaluate(time);
				break;
			case PropertyTypeInfo.Double:
				obj = this.newCurves[0].Evaluate(time);
				break;
			case PropertyTypeInfo.Bool:
			{
				bool flag8 = false;
				foreach (NewUSInternalKeyframe current in this.newCurves[0].Keys)
				{
					if (current.Time > time)
					{
						break;
					}
					if (current.Value >= 1f)
					{
						flag8 = true;
					}
					else if (current.Value <= 0f)
					{
						flag8 = false;
					}
				}
				if (this.newCurves[0].Evaluate(time) >= 1f)
				{
					this.tmpBool = true;
				}
				else if (this.newCurves[0].Evaluate(time) <= 0f)
				{
					this.tmpBool = false;
				}
				else
				{
					this.tmpBool = flag8;
				}
				obj = this.tmpBool;
				break;
			}
			case PropertyTypeInfo.Vec2:
				this.tmpVector2.x = this.newCurves[0].Evaluate(time);
				this.tmpVector2.y = this.newCurves[1].Evaluate(time);
				obj = this.tmpVector2;
				break;
			case PropertyTypeInfo.Vec3:
				this.tmpVector3.x = this.newCurves[0].Evaluate(time);
				this.tmpVector3.y = this.newCurves[1].Evaluate(time);
				this.tmpVector3.z = this.newCurves[2].Evaluate(time);
				obj = this.tmpVector3;
				break;
			case PropertyTypeInfo.Vec4:
				this.tmpVector4.x = this.newCurves[0].Evaluate(time);
				this.tmpVector4.y = this.newCurves[1].Evaluate(time);
				this.tmpVector4.z = this.newCurves[2].Evaluate(time);
				this.tmpVector4.w = this.newCurves[3].Evaluate(time);
				obj = this.tmpVector4;
				break;
			case PropertyTypeInfo.Quat:
				this.tmpQuat.x = this.newCurves[0].Evaluate(time);
				this.tmpQuat.y = this.newCurves[1].Evaluate(time);
				this.tmpQuat.z = this.newCurves[2].Evaluate(time);
				this.tmpQuat.w = this.newCurves[3].Evaluate(time);
				obj = this.tmpQuat;
				break;
			case PropertyTypeInfo.Colour:
				this.tmpColour.r = this.newCurves[0].Evaluate(time);
				this.tmpColour.g = this.newCurves[1].Evaluate(time);
				this.tmpColour.b = this.newCurves[2].Evaluate(time);
				this.tmpColour.a = this.newCurves[3].Evaluate(time);
				obj = this.tmpColour;
				break;
			}
			if (obj == null)
			{
				return;
			}
			if (this.component == null)
			{
				return;
			}
			if (this.fieldInfo != null)
			{
				this.fieldInfo.SetValue(this.component, obj);
			}
			if (this.propertyInfo != null)
			{
				this.propertyInfo.SetValue(this.component, obj, null);
			}
		}

		public void AddKeyframe(List<NewUSInternalCurve> settingCurves, object ourValue, float time, CurveAutoTangentModes tangentMode)
		{
			if (settingCurves.Count == 0)
			{
				return;
			}
			if (this.propertyType == PropertyTypeInfo.None)
			{
				Debug.LogWarning("Atempting to Add A Keyframe, before calling CreatePropertyInfo");
				return;
			}
			if (NewUSPropertyInfo.GetMappedType(ourValue.GetType()) != this.propertyType)
			{
				Debug.LogWarning("Trying to Add a Keyframe of the wrong type");
				return;
			}
			switch (NewUSPropertyInfo.GetMappedType(ourValue.GetType()))
			{
			case PropertyTypeInfo.Int:
				this.AddKeyframe((int)ourValue, time, tangentMode);
				break;
			case PropertyTypeInfo.Long:
				this.AddKeyframe((long)ourValue, time, tangentMode);
				break;
			case PropertyTypeInfo.Float:
				this.AddKeyframe((float)ourValue, time, tangentMode);
				break;
			case PropertyTypeInfo.Double:
				this.AddKeyframe((double)ourValue, time, tangentMode);
				break;
			case PropertyTypeInfo.Bool:
				this.AddKeyframe((bool)ourValue, time, tangentMode);
				break;
			case PropertyTypeInfo.Vec2:
			{
				Vector2 vector = (Vector2)ourValue;
				this.tmpKeyframe.time = time;
				if (settingCurves.Contains(this.newCurves[0]))
				{
					this.tmpKeyframe.value = vector.x;
					this.AddKeyframe(this.newCurves[0], this.tmpKeyframe, tangentMode);
				}
				if (settingCurves.Contains(this.newCurves[1]))
				{
					this.tmpKeyframe.value = vector.y;
					this.AddKeyframe(this.newCurves[1], this.tmpKeyframe, tangentMode);
				}
				break;
			}
			case PropertyTypeInfo.Vec3:
			{
				Vector3 vector2 = (Vector3)ourValue;
				this.tmpKeyframe.time = time;
				if (settingCurves.Contains(this.newCurves[0]))
				{
					this.tmpKeyframe.value = vector2.x;
					this.AddKeyframe(this.newCurves[0], this.tmpKeyframe, tangentMode);
				}
				if (settingCurves.Contains(this.newCurves[1]))
				{
					this.tmpKeyframe.value = vector2.y;
					this.AddKeyframe(this.newCurves[1], this.tmpKeyframe, tangentMode);
				}
				if (settingCurves.Contains(this.newCurves[2]))
				{
					this.tmpKeyframe.value = vector2.z;
					this.AddKeyframe(this.newCurves[2], this.tmpKeyframe, tangentMode);
				}
				break;
			}
			case PropertyTypeInfo.Vec4:
			{
				Vector4 vector3 = (Vector4)ourValue;
				this.tmpKeyframe.time = time;
				if (settingCurves.Contains(this.newCurves[0]))
				{
					this.tmpKeyframe.value = vector3.x;
					this.AddKeyframe(this.newCurves[0], this.tmpKeyframe, tangentMode);
				}
				if (settingCurves.Contains(this.newCurves[1]))
				{
					this.tmpKeyframe.value = vector3.y;
					this.AddKeyframe(this.newCurves[1], this.tmpKeyframe, tangentMode);
				}
				if (settingCurves.Contains(this.newCurves[2]))
				{
					this.tmpKeyframe.value = vector3.z;
					this.AddKeyframe(this.newCurves[2], this.tmpKeyframe, tangentMode);
				}
				if (settingCurves.Contains(this.newCurves[3]))
				{
					this.tmpKeyframe.value = vector3.w;
					this.AddKeyframe(this.newCurves[3], this.tmpKeyframe, tangentMode);
				}
				break;
			}
			case PropertyTypeInfo.Quat:
			{
				Quaternion quaternion = (Quaternion)ourValue;
				this.tmpKeyframe.time = time;
				if (settingCurves.Contains(this.newCurves[0]))
				{
					this.tmpKeyframe.value = quaternion.x;
					this.AddKeyframe(this.newCurves[0], this.tmpKeyframe, tangentMode);
				}
				if (settingCurves.Contains(this.newCurves[1]))
				{
					this.tmpKeyframe.value = quaternion.y;
					this.AddKeyframe(this.newCurves[1], this.tmpKeyframe, tangentMode);
				}
				if (settingCurves.Contains(this.newCurves[2]))
				{
					this.tmpKeyframe.value = quaternion.z;
					this.AddKeyframe(this.newCurves[2], this.tmpKeyframe, tangentMode);
				}
				if (settingCurves.Contains(this.newCurves[3]))
				{
					this.tmpKeyframe.value = quaternion.w;
					this.AddKeyframe(this.newCurves[3], this.tmpKeyframe, tangentMode);
				}
				break;
			}
			case PropertyTypeInfo.Colour:
			{
				Color color = (Color)ourValue;
				this.tmpKeyframe.time = time;
				if (settingCurves.Contains(this.newCurves[0]))
				{
					this.tmpKeyframe.value = color.r;
					this.AddKeyframe(this.newCurves[0], this.tmpKeyframe, tangentMode);
				}
				if (settingCurves.Contains(this.newCurves[1]))
				{
					this.tmpKeyframe.value = color.g;
					this.AddKeyframe(this.newCurves[1], this.tmpKeyframe, tangentMode);
				}
				if (settingCurves.Contains(this.newCurves[2]))
				{
					this.tmpKeyframe.value = color.b;
					this.AddKeyframe(this.newCurves[2], this.tmpKeyframe, tangentMode);
				}
				if (settingCurves.Contains(this.newCurves[3]))
				{
					this.tmpKeyframe.value = color.a;
					this.AddKeyframe(this.newCurves[3], this.tmpKeyframe, tangentMode);
				}
				break;
			}
			}
		}

		public void AddKeyframe(float time, CurveAutoTangentModes tangentMode)
		{
			object ourValue = null;
			if (this.fieldInfo != null)
			{
				ourValue = this.fieldInfo.GetValue(this.component);
			}
			if (this.propertyInfo != null)
			{
				ourValue = this.propertyInfo.GetValue(this.component, null);
			}
			this.AddKeyframe(ourValue, time, tangentMode);
		}

		public void AddKeyframe(object ourValue, float time, CurveAutoTangentModes tangentMode)
		{
			if (this.propertyType == PropertyTypeInfo.None)
			{
				Debug.LogWarning("Atempting to Add A Keyframe, before calling CreatePropertyInfo");
				return;
			}
			if (NewUSPropertyInfo.GetMappedType(ourValue.GetType()) != this.propertyType)
			{
				Debug.LogWarning("Trying to Add a Keyframe of the wrong type");
				return;
			}
			switch (NewUSPropertyInfo.GetMappedType(ourValue.GetType()))
			{
			case PropertyTypeInfo.Int:
				this.AddKeyframe((int)ourValue, time, tangentMode);
				break;
			case PropertyTypeInfo.Long:
				this.AddKeyframe((long)ourValue, time, tangentMode);
				break;
			case PropertyTypeInfo.Float:
				this.AddKeyframe((float)ourValue, time, tangentMode);
				break;
			case PropertyTypeInfo.Double:
				this.AddKeyframe((double)ourValue, time, tangentMode);
				break;
			case PropertyTypeInfo.Bool:
				this.AddKeyframe((bool)ourValue, time, tangentMode);
				break;
			case PropertyTypeInfo.Vec2:
				this.AddKeyframe((Vector2)ourValue, time, tangentMode);
				break;
			case PropertyTypeInfo.Vec3:
				this.AddKeyframe((Vector3)ourValue, time, tangentMode);
				break;
			case PropertyTypeInfo.Vec4:
				this.AddKeyframe((Vector4)ourValue, time, tangentMode);
				break;
			case PropertyTypeInfo.Quat:
				this.AddKeyframe((Quaternion)ourValue, time, tangentMode);
				break;
			case PropertyTypeInfo.Colour:
				this.AddKeyframe((Color)ourValue, time, tangentMode);
				break;
			}
		}

		private void AddKeyframe(NewUSInternalCurve curve, Keyframe keyframe, CurveAutoTangentModes tangentMode)
		{
			NewUSInternalKeyframe newUSInternalKeyframe = curve.AddKeyframe(keyframe.time, keyframe.value);
			if (this.propertyType == PropertyTypeInfo.Bool)
			{
				tangentMode = CurveAutoTangentModes.None;
				newUSInternalKeyframe.RightTangentConstant();
				newUSInternalKeyframe.LeftTangentConstant();
			}
			if (tangentMode == CurveAutoTangentModes.Smooth)
			{
				newUSInternalKeyframe.Smooth();
			}
			else if (tangentMode == CurveAutoTangentModes.Flatten)
			{
				newUSInternalKeyframe.Flatten();
			}
			else if (tangentMode == CurveAutoTangentModes.RightLinear)
			{
				newUSInternalKeyframe.RightTangentLinear();
			}
			else if (tangentMode == CurveAutoTangentModes.LeftLinear)
			{
				newUSInternalKeyframe.LeftTangentLinear();
			}
			else if (tangentMode == CurveAutoTangentModes.BothLinear)
			{
				newUSInternalKeyframe.BothTangentLinear();
			}
			this.SetValue(keyframe.time);
		}

		private void AddKeyframe(int ourValue, float time, CurveAutoTangentModes tangentMode)
		{
			this.tmpKeyframe.time = time;
			this.tmpKeyframe.value = (float)ourValue;
			this.AddKeyframe(this.newCurves[0], this.tmpKeyframe, tangentMode);
		}

		private void AddKeyframe(long ourValue, float time, CurveAutoTangentModes tangentMode)
		{
			this.tmpKeyframe.time = time;
			this.tmpKeyframe.value = (float)ourValue;
			this.AddKeyframe(this.newCurves[0], this.tmpKeyframe, tangentMode);
		}

		private void AddKeyframe(float ourValue, float time, CurveAutoTangentModes tangentMode)
		{
			this.tmpKeyframe.time = time;
			this.tmpKeyframe.value = ourValue;
			this.AddKeyframe(this.newCurves[0], this.tmpKeyframe, tangentMode);
		}

		private void AddKeyframe(double ourValue, float time, CurveAutoTangentModes tangentMode)
		{
			this.tmpKeyframe.time = time;
			this.tmpKeyframe.value = (float)ourValue;
			this.AddKeyframe(this.newCurves[0], this.tmpKeyframe, tangentMode);
		}

		private void AddKeyframe(bool ourValue, float time, CurveAutoTangentModes tangentMode)
		{
			float value = 0f;
			if (ourValue)
			{
				value = 1f;
			}
			this.tmpKeyframe.time = time;
			this.tmpKeyframe.value = value;
			this.AddKeyframe(this.newCurves[0], this.tmpKeyframe, CurveAutoTangentModes.None);
		}

		private void AddKeyframe(Vector2 ourValue, float time, CurveAutoTangentModes tangentMode)
		{
			this.tmpKeyframe.time = time;
			this.tmpKeyframe.value = ourValue.x;
			this.AddKeyframe(this.newCurves[0], this.tmpKeyframe, tangentMode);
			this.tmpKeyframe.time = time;
			this.tmpKeyframe.value = ourValue.y;
			this.AddKeyframe(this.newCurves[1], this.tmpKeyframe, tangentMode);
		}

		private void AddKeyframe(Vector3 ourValue, float time, CurveAutoTangentModes tangentMode)
		{
			this.tmpKeyframe.time = time;
			this.tmpKeyframe.value = ourValue.x;
			this.AddKeyframe(this.newCurves[0], this.tmpKeyframe, tangentMode);
			this.tmpKeyframe.time = time;
			this.tmpKeyframe.value = ourValue.y;
			this.AddKeyframe(this.newCurves[1], this.tmpKeyframe, tangentMode);
			this.tmpKeyframe.time = time;
			this.tmpKeyframe.value = ourValue.z;
			this.AddKeyframe(this.newCurves[2], this.tmpKeyframe, tangentMode);
		}

		private void AddKeyframe(Vector4 ourValue, float time, CurveAutoTangentModes tangentMode)
		{
			this.tmpKeyframe.time = time;
			this.tmpKeyframe.value = ourValue.x;
			this.AddKeyframe(this.newCurves[0], this.tmpKeyframe, tangentMode);
			this.tmpKeyframe.time = time;
			this.tmpKeyframe.value = ourValue.y;
			this.AddKeyframe(this.newCurves[1], this.tmpKeyframe, tangentMode);
			this.tmpKeyframe.time = time;
			this.tmpKeyframe.value = ourValue.z;
			this.AddKeyframe(this.newCurves[2], this.tmpKeyframe, tangentMode);
			this.tmpKeyframe.time = time;
			this.tmpKeyframe.value = ourValue.w;
			this.AddKeyframe(this.newCurves[3], this.tmpKeyframe, tangentMode);
		}

		private void AddKeyframe(Quaternion ourValue, float time, CurveAutoTangentModes tangentMode)
		{
			this.tmpKeyframe.time = time;
			this.tmpKeyframe.value = ourValue.x;
			this.AddKeyframe(this.newCurves[0], this.tmpKeyframe, tangentMode);
			this.tmpKeyframe.time = time;
			this.tmpKeyframe.value = ourValue.y;
			this.AddKeyframe(this.newCurves[1], this.tmpKeyframe, tangentMode);
			this.tmpKeyframe.time = time;
			this.tmpKeyframe.value = ourValue.z;
			this.AddKeyframe(this.newCurves[2], this.tmpKeyframe, tangentMode);
			this.tmpKeyframe.time = time;
			this.tmpKeyframe.value = ourValue.w;
			this.AddKeyframe(this.newCurves[3], this.tmpKeyframe, tangentMode);
		}

		private void AddKeyframe(Color ourValue, float time, CurveAutoTangentModes tangentMode)
		{
			this.tmpKeyframe.time = time;
			this.tmpKeyframe.value = ourValue.r;
			this.AddKeyframe(this.newCurves[0], this.tmpKeyframe, tangentMode);
			this.tmpKeyframe.time = time;
			this.tmpKeyframe.value = ourValue.g;
			this.AddKeyframe(this.newCurves[1], this.tmpKeyframe, tangentMode);
			this.tmpKeyframe.time = time;
			this.tmpKeyframe.value = ourValue.b;
			this.AddKeyframe(this.newCurves[2], this.tmpKeyframe, tangentMode);
			this.tmpKeyframe.time = time;
			this.tmpKeyframe.value = ourValue.a;
			this.AddKeyframe(this.newCurves[3], this.tmpKeyframe, tangentMode);
		}

		private void CreatePropertyCurves()
		{
			switch (this.propertyType)
			{
			case PropertyTypeInfo.Int:
				this.newCurves.Add(new NewUSInternalCurve());
				this.newCurves[0].UnityAnimationCurve = new AnimationCurve();
				break;
			case PropertyTypeInfo.Long:
				this.newCurves.Add(new NewUSInternalCurve());
				this.newCurves[0].UnityAnimationCurve = new AnimationCurve();
				break;
			case PropertyTypeInfo.Float:
				this.newCurves.Add(new NewUSInternalCurve());
				this.newCurves[0].UnityAnimationCurve = new AnimationCurve();
				break;
			case PropertyTypeInfo.Double:
				this.newCurves.Add(new NewUSInternalCurve());
				this.newCurves[0].UnityAnimationCurve = new AnimationCurve();
				break;
			case PropertyTypeInfo.Bool:
				this.newCurves.Add(new NewUSInternalCurve());
				this.newCurves[0].UnityAnimationCurve = new AnimationCurve();
				break;
			case PropertyTypeInfo.Vec2:
				this.newCurves.Add(new NewUSInternalCurve());
				this.newCurves.Add(new NewUSInternalCurve());
				this.newCurves[0].UnityAnimationCurve = new AnimationCurve();
				this.newCurves[1].UnityAnimationCurve = new AnimationCurve();
				break;
			case PropertyTypeInfo.Vec3:
				this.newCurves.Add(new NewUSInternalCurve());
				this.newCurves.Add(new NewUSInternalCurve());
				this.newCurves.Add(new NewUSInternalCurve());
				this.newCurves[0].UnityAnimationCurve = new AnimationCurve();
				this.newCurves[1].UnityAnimationCurve = new AnimationCurve();
				this.newCurves[2].UnityAnimationCurve = new AnimationCurve();
				break;
			case PropertyTypeInfo.Vec4:
				this.newCurves.Add(new NewUSInternalCurve());
				this.newCurves.Add(new NewUSInternalCurve());
				this.newCurves.Add(new NewUSInternalCurve());
				this.newCurves.Add(new NewUSInternalCurve());
				this.newCurves[0].UnityAnimationCurve = new AnimationCurve();
				this.newCurves[1].UnityAnimationCurve = new AnimationCurve();
				this.newCurves[2].UnityAnimationCurve = new AnimationCurve();
				this.newCurves[3].UnityAnimationCurve = new AnimationCurve();
				break;
			case PropertyTypeInfo.Quat:
				this.newCurves.Add(new NewUSInternalCurve());
				this.newCurves.Add(new NewUSInternalCurve());
				this.newCurves.Add(new NewUSInternalCurve());
				this.newCurves.Add(new NewUSInternalCurve());
				this.newCurves[0].UnityAnimationCurve = new AnimationCurve();
				this.newCurves[1].UnityAnimationCurve = new AnimationCurve();
				this.newCurves[2].UnityAnimationCurve = new AnimationCurve();
				this.newCurves[3].UnityAnimationCurve = new AnimationCurve();
				break;
			case PropertyTypeInfo.Colour:
				this.newCurves.Add(new NewUSInternalCurve());
				this.newCurves.Add(new NewUSInternalCurve());
				this.newCurves.Add(new NewUSInternalCurve());
				this.newCurves.Add(new NewUSInternalCurve());
				this.newCurves[0].UnityAnimationCurve = new AnimationCurve();
				this.newCurves[1].UnityAnimationCurve = new AnimationCurve();
				this.newCurves[2].UnityAnimationCurve = new AnimationCurve();
				this.newCurves[3].UnityAnimationCurve = new AnimationCurve();
				break;
			}
		}

		public object GetValueForTime(float time)
		{
			object obj = null;
			switch (this.propertyType)
			{
			case PropertyTypeInfo.Int:
				obj = this.newCurves[0].Evaluate(time);
				break;
			case PropertyTypeInfo.Long:
				obj = this.newCurves[0].Evaluate(time);
				break;
			case PropertyTypeInfo.Float:
				obj = this.newCurves[0].Evaluate(time);
				break;
			case PropertyTypeInfo.Double:
				obj = this.newCurves[0].Evaluate(time);
				break;
			case PropertyTypeInfo.Bool:
			{
				bool flag = false;
				foreach (NewUSInternalKeyframe current in this.newCurves[0].Keys)
				{
					if (current.Time > time)
					{
						break;
					}
					if (current.Value >= 1f)
					{
						flag = true;
					}
					else if (current.Value <= 0f)
					{
						flag = false;
					}
				}
				if (this.newCurves[0].Evaluate(time) >= 1f)
				{
					this.tmpBool = true;
				}
				else if (this.newCurves[0].Evaluate(time) <= 0f)
				{
					this.tmpBool = false;
				}
				else
				{
					this.tmpBool = flag;
				}
				obj = this.tmpBool;
				break;
			}
			case PropertyTypeInfo.Vec2:
				this.tmpVector2.x = this.newCurves[0].Evaluate(time);
				this.tmpVector2.y = this.newCurves[1].Evaluate(time);
				obj = this.tmpVector2;
				break;
			case PropertyTypeInfo.Vec3:
				this.tmpVector3.x = this.newCurves[0].Evaluate(time);
				this.tmpVector3.y = this.newCurves[1].Evaluate(time);
				this.tmpVector3.z = this.newCurves[2].Evaluate(time);
				obj = this.tmpVector3;
				break;
			case PropertyTypeInfo.Vec4:
				this.tmpVector4.x = this.newCurves[0].Evaluate(time);
				this.tmpVector4.y = this.newCurves[1].Evaluate(time);
				this.tmpVector4.z = this.newCurves[2].Evaluate(time);
				this.tmpVector4.w = this.newCurves[3].Evaluate(time);
				obj = this.tmpVector4;
				break;
			case PropertyTypeInfo.Quat:
				this.tmpQuat.x = this.newCurves[0].Evaluate(time);
				this.tmpQuat.y = this.newCurves[1].Evaluate(time);
				this.tmpQuat.z = this.newCurves[2].Evaluate(time);
				this.tmpQuat.w = this.newCurves[3].Evaluate(time);
				obj = this.tmpQuat;
				break;
			case PropertyTypeInfo.Colour:
				this.tmpColour.r = this.newCurves[0].Evaluate(time);
				this.tmpColour.g = this.newCurves[1].Evaluate(time);
				this.tmpColour.b = this.newCurves[2].Evaluate(time);
				this.tmpColour.a = this.newCurves[3].Evaluate(time);
				obj = this.tmpColour;
				break;
			}
			if (obj == null)
			{
				return null;
			}
			return obj;
		}

		public List<NewModification> GetModifiedCurvesAtTime(float runningTime)
		{
			List<NewModification> list = new List<NewModification>();
			if (!this.component)
			{
				return list;
			}
			bool flag = false;
			foreach (NewUSInternalCurve current in this.newCurves)
			{
				if (runningTime >= current.FirstKeyframeTime && runningTime <= current.LastKeyframeTime)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				return list;
			}
			object obj = null;
			if (this.fieldInfo != null)
			{
				obj = this.fieldInfo.GetValue(this.component);
			}
			if (this.propertyInfo != null)
			{
				obj = this.propertyInfo.GetValue(this.component, null);
			}
			switch (this.propertyType)
			{
			case PropertyTypeInfo.Int:
			{
				int num = (int)this.newCurves[0].Evaluate(runningTime);
				if (num != (int)obj)
				{
					list.Add(new NewModification(this.newCurves[0], runningTime, (float)obj));
				}
				break;
			}
			case PropertyTypeInfo.Long:
			{
				long num2 = (long)this.newCurves[0].Evaluate(runningTime);
				if (num2 != (long)obj)
				{
					list.Add(new NewModification(this.newCurves[0], runningTime, (float)obj));
				}
				break;
			}
			case PropertyTypeInfo.Float:
			{
				float a = this.newCurves[0].Evaluate(runningTime);
				if (!Mathf.Approximately(a, (float)obj))
				{
					list.Add(new NewModification(this.newCurves[0], runningTime, (float)obj));
				}
				break;
			}
			case PropertyTypeInfo.Double:
			{
				float a2 = this.newCurves[0].Evaluate(runningTime);
				if (!Mathf.Approximately(a2, (float)obj))
				{
					list.Add(new NewModification(this.newCurves[0], runningTime, (float)obj));
				}
				break;
			}
			case PropertyTypeInfo.Bool:
			{
				float num3 = this.newCurves[0].Evaluate(runningTime);
				bool flag2 = num3 >= 1f;
				if (flag2 != (bool)obj)
				{
					list.Add(new NewModification(this.newCurves[0], runningTime, (float)obj));
				}
				break;
			}
			case PropertyTypeInfo.Vec2:
			{
				this.tmpVector2 = (Vector2)obj;
				float a3 = this.newCurves[0].Evaluate(runningTime);
				if (!Mathf.Approximately(a3, this.tmpVector2.x))
				{
					list.Add(new NewModification(this.newCurves[0], runningTime, this.tmpVector2.x));
				}
				float a4 = this.newCurves[1].Evaluate(runningTime);
				if (!Mathf.Approximately(a4, this.tmpVector2.y))
				{
					list.Add(new NewModification(this.newCurves[1], runningTime, this.tmpVector2.y));
				}
				break;
			}
			case PropertyTypeInfo.Vec3:
			{
				this.tmpVector3 = (Vector3)obj;
				float a5 = this.newCurves[0].Evaluate(runningTime);
				if (!Mathf.Approximately(a5, this.tmpVector3.x))
				{
					list.Add(new NewModification(this.newCurves[0], runningTime, this.tmpVector3.x));
				}
				float a6 = this.newCurves[1].Evaluate(runningTime);
				if (!Mathf.Approximately(a6, this.tmpVector3.y))
				{
					list.Add(new NewModification(this.newCurves[1], runningTime, this.tmpVector3.y));
				}
				float a7 = this.newCurves[2].Evaluate(runningTime);
				if (!Mathf.Approximately(a7, this.tmpVector3.z))
				{
					list.Add(new NewModification(this.newCurves[2], runningTime, this.tmpVector3.z));
				}
				break;
			}
			case PropertyTypeInfo.Vec4:
			{
				this.tmpVector4 = (Vector4)obj;
				float a8 = this.newCurves[0].Evaluate(runningTime);
				if (!Mathf.Approximately(a8, this.tmpVector4.x))
				{
					list.Add(new NewModification(this.newCurves[0], runningTime, this.tmpVector4.x));
				}
				float a9 = this.newCurves[1].Evaluate(runningTime);
				if (!Mathf.Approximately(a9, this.tmpVector4.y))
				{
					list.Add(new NewModification(this.newCurves[1], runningTime, this.tmpVector4.y));
				}
				float a10 = this.newCurves[2].Evaluate(runningTime);
				if (!Mathf.Approximately(a10, this.tmpVector4.z))
				{
					list.Add(new NewModification(this.newCurves[2], runningTime, this.tmpVector4.z));
				}
				float a11 = this.newCurves[3].Evaluate(runningTime);
				if (!Mathf.Approximately(a11, this.tmpVector4.w))
				{
					list.Add(new NewModification(this.newCurves[3], runningTime, this.tmpVector4.w));
				}
				break;
			}
			case PropertyTypeInfo.Quat:
			{
				this.tmpQuat = (Quaternion)obj;
				float a12 = this.newCurves[0].Evaluate(runningTime);
				if (!Mathf.Approximately(a12, this.tmpQuat.x))
				{
					list.Add(new NewModification(this.newCurves[0], runningTime, this.tmpQuat.x));
				}
				float a13 = this.newCurves[1].Evaluate(runningTime);
				if (!Mathf.Approximately(a13, this.tmpQuat.y))
				{
					list.Add(new NewModification(this.newCurves[1], runningTime, this.tmpQuat.y));
				}
				float a14 = this.newCurves[2].Evaluate(runningTime);
				if (!Mathf.Approximately(a14, this.tmpQuat.z))
				{
					list.Add(new NewModification(this.newCurves[2], runningTime, this.tmpQuat.z));
				}
				float a15 = this.newCurves[3].Evaluate(runningTime);
				if (!Mathf.Approximately(a15, this.tmpQuat.w))
				{
					list.Add(new NewModification(this.newCurves[3], runningTime, this.tmpQuat.w));
				}
				break;
			}
			case PropertyTypeInfo.Colour:
			{
				this.tmpColour = (Color)obj;
				float a16 = this.newCurves[0].Evaluate(runningTime);
				if (!Mathf.Approximately(a16, this.tmpColour.r))
				{
					list.Add(new NewModification(this.newCurves[0], runningTime, this.tmpColour.r));
				}
				float a17 = this.newCurves[1].Evaluate(runningTime);
				if (!Mathf.Approximately(a17, this.tmpColour.g))
				{
					list.Add(new NewModification(this.newCurves[1], runningTime, this.tmpColour.g));
				}
				float a18 = this.newCurves[2].Evaluate(runningTime);
				if (!Mathf.Approximately(a18, this.tmpColour.b))
				{
					list.Add(new NewModification(this.newCurves[2], runningTime, this.tmpColour.b));
				}
				float a19 = this.newCurves[3].Evaluate(runningTime);
				if (!Mathf.Approximately(a19, this.tmpColour.a))
				{
					list.Add(new NewModification(this.newCurves[3], runningTime, this.tmpColour.a));
				}
				break;
			}
			}
			return list;
		}

		public void StoreBaseState()
		{
			object obj = null;
			if (this.fieldInfo != null)
			{
				obj = this.fieldInfo.GetValue(this.component);
			}
			if (this.propertyInfo != null)
			{
				obj = this.propertyInfo.GetValue(this.component, null);
			}
			switch (this.propertyType)
			{
			case PropertyTypeInfo.Int:
				this.baseInt = (int)obj;
				break;
			case PropertyTypeInfo.Long:
				this.baseLong = (long)obj;
				break;
			case PropertyTypeInfo.Float:
				this.baseFloat = (float)obj;
				break;
			case PropertyTypeInfo.Double:
				this.baseDouble = (double)obj;
				break;
			case PropertyTypeInfo.Bool:
				this.baseBool = (bool)obj;
				break;
			case PropertyTypeInfo.Vec2:
				this.baseVector2 = (Vector2)obj;
				break;
			case PropertyTypeInfo.Vec3:
				this.baseVector3 = (Vector3)obj;
				break;
			case PropertyTypeInfo.Vec4:
				this.baseVector4 = (Vector4)obj;
				break;
			case PropertyTypeInfo.Quat:
				this.baseQuat = (Quaternion)obj;
				break;
			case PropertyTypeInfo.Colour:
				this.baseColour = (Color)obj;
				break;
			}
		}

		public void RestoreBaseState()
		{
			object value = null;
			switch (this.propertyType)
			{
			case PropertyTypeInfo.Int:
				value = this.baseInt;
				break;
			case PropertyTypeInfo.Long:
				value = this.baseLong;
				break;
			case PropertyTypeInfo.Float:
				value = this.baseFloat;
				break;
			case PropertyTypeInfo.Double:
				value = this.baseDouble;
				break;
			case PropertyTypeInfo.Bool:
				value = this.baseBool;
				break;
			case PropertyTypeInfo.Vec2:
				value = this.baseVector2;
				break;
			case PropertyTypeInfo.Vec3:
				value = this.baseVector3;
				break;
			case PropertyTypeInfo.Vec4:
				value = this.baseVector4;
				break;
			case PropertyTypeInfo.Quat:
				value = this.baseQuat;
				break;
			case PropertyTypeInfo.Colour:
				value = this.baseColour;
				break;
			}
			if (this.fieldInfo != null)
			{
				this.fieldInfo.SetValue(this.component, value);
			}
			if (this.propertyInfo != null)
			{
				this.propertyInfo.SetValue(this.component, value, null);
			}
		}

		public static PropertyTypeInfo GetMappedType(Type type)
		{
			if (type == typeof(int))
			{
				return PropertyTypeInfo.Int;
			}
			if (type == typeof(long))
			{
				return PropertyTypeInfo.Long;
			}
			if (type == typeof(float) || type == typeof(float))
			{
				return PropertyTypeInfo.Float;
			}
			if (type == typeof(double))
			{
				return PropertyTypeInfo.Double;
			}
			if (type == typeof(bool))
			{
				return PropertyTypeInfo.Bool;
			}
			if (type == typeof(Vector2))
			{
				return PropertyTypeInfo.Vec2;
			}
			if (type == typeof(Vector3))
			{
				return PropertyTypeInfo.Vec3;
			}
			if (type == typeof(Vector4))
			{
				return PropertyTypeInfo.Vec4;
			}
			if (type == typeof(Quaternion))
			{
				return PropertyTypeInfo.Quat;
			}
			if (type == typeof(Color))
			{
				return PropertyTypeInfo.Colour;
			}
			return PropertyTypeInfo.None;
		}

		public static Type GetMappedType(PropertyTypeInfo type)
		{
			if (type == PropertyTypeInfo.Int)
			{
				return typeof(int);
			}
			if (type == PropertyTypeInfo.Long)
			{
				return typeof(long);
			}
			if (type == PropertyTypeInfo.Float)
			{
				return typeof(float);
			}
			if (type == PropertyTypeInfo.Double)
			{
				return typeof(double);
			}
			if (type == PropertyTypeInfo.Bool)
			{
				return typeof(bool);
			}
			if (type == PropertyTypeInfo.Vec2)
			{
				return typeof(Vector2);
			}
			if (type == PropertyTypeInfo.Vec3)
			{
				return typeof(Vector3);
			}
			if (type == PropertyTypeInfo.Vec4)
			{
				return typeof(Vector4);
			}
			if (type == PropertyTypeInfo.Quat)
			{
				return typeof(Quaternion);
			}
			if (type == PropertyTypeInfo.Colour)
			{
				return typeof(Color);
			}
			return null;
		}

		private float GetValue(int curveIndex)
		{
			object obj = null;
			if (this.fieldInfo != null)
			{
				obj = this.fieldInfo.GetValue(this.component);
			}
			if (this.propertyInfo != null)
			{
				obj = this.propertyInfo.GetValue(this.component, null);
			}
			switch (NewUSPropertyInfo.GetMappedType(obj.GetType()))
			{
			case PropertyTypeInfo.Int:
				return (float)((int)obj);
			case PropertyTypeInfo.Long:
				return (float)((long)obj);
			case PropertyTypeInfo.Float:
				return (float)obj;
			case PropertyTypeInfo.Double:
				return (float)((double)obj);
			case PropertyTypeInfo.Bool:
			{
				bool flag = (bool)obj;
				float result = 0f;
				if (flag)
				{
					result = 1f;
				}
				return result;
			}
			case PropertyTypeInfo.Vec2:
				return ((Vector2)obj)[curveIndex];
			case PropertyTypeInfo.Vec3:
				return ((Vector3)obj)[curveIndex];
			case PropertyTypeInfo.Vec4:
				return ((Vector4)obj)[curveIndex];
			case PropertyTypeInfo.Quat:
				return ((Quaternion)obj)[curveIndex];
			case PropertyTypeInfo.Colour:
				return ((Color)obj)[curveIndex];
			default:
				throw new NotImplementedException("This should never happen");
			}
		}

		private void BuildForCurrentValue(int curveIndex)
		{
			this.newCurves[curveIndex].Keys[0].Value = this.GetValue(curveIndex);
		}

		public string GetJSON()
		{
			string arg = string.Format("\"ComponentType\": \"{0}\"", this.ComponentType);
			string arg2 = string.Format("\"ComponentName\": \"{0}\"", this.PropertyName);
			return string.Format("{{{0}, {1}}}", arg, arg2);
		}
	}
}
