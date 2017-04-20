using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/UI/NGUI Sprite"), ExecuteInEditMode]
public class UISprite : UIWidget
{
	public enum Type
	{
		Simple,
		Sliced,
		Tiled,
		Filled,
		Advanced,
		Custom
	}

	public enum FillDirection
	{
		Horizontal,
		Vertical,
		Radial90,
		Radial180,
		Radial360
	}

	public enum AdvancedType
	{
		Invisible,
		Sliced,
		Tiled
	}

	public enum Flip
	{
		Nothing,
		Horizontally,
		Vertically,
		Both
	}

	public enum CustomType
	{
		Sliced,
		Tiled
	}

	public delegate void OnInitCallback(UISprite sprite);

	[HideInInspector]
	private UIAtlas mAtlas;

	[HideInInspector, SerializeField]
	private string mSpriteName;

	[HideInInspector, SerializeField]
	private UISprite.Type mType;

	[HideInInspector, SerializeField]
	private UISprite.FillDirection mFillDirection = UISprite.FillDirection.Radial360;

	[HideInInspector, Range(0f, 1f), SerializeField]
	private float mFillAmount = 1f;

	[HideInInspector, SerializeField]
	private bool mInvert;

	[HideInInspector, SerializeField]
	private UISprite.Flip mFlip;

	[HideInInspector, SerializeField]
	private bool mFillCenter = true;

	protected UISpriteData mSprite;

	protected Rect mInnerUV = default(Rect);

	protected Rect mOuterUV = default(Rect);

	private bool mSpriteSet;

	private List<UISprite.OnInitCallback> initCallbackList = new List<UISprite.OnInitCallback>();

	public UISprite.AdvancedType centerType = UISprite.AdvancedType.Sliced;

	public UISprite.AdvancedType leftType = UISprite.AdvancedType.Sliced;

	public UISprite.AdvancedType rightType = UISprite.AdvancedType.Sliced;

	public UISprite.AdvancedType bottomType = UISprite.AdvancedType.Sliced;

	public UISprite.AdvancedType topType = UISprite.AdvancedType.Sliced;

	[HideInInspector, SerializeField]
	private string mstrAtlasName = string.Empty;

	private int maskCount = 1;

	[HideInInspector, SerializeField]
	private Vector4 customValue1 = Vector4.zero;

	[HideInInspector, SerializeField]
	private Vector4 customValue2 = Vector4.zero;

	private static Vector2[] mTempPos = new Vector2[4];

	private static Vector2[] mTempUVs = new Vector2[4];

	public virtual UISprite.Type type
	{
		get
		{
			return this.mType;
		}
		set
		{
			if (this.mType != value)
			{
				this.mType = value;
				this.MarkAsChanged();
			}
		}
	}

	public string AtlasName
	{
		get
		{
			return this.mstrAtlasName;
		}
		set
		{
			if (this.mstrAtlasName != value)
			{
				if (this.mAtlas != null)
				{
					AtlasManager.PopAtlas(this.mstrAtlasName, base.gameObject);
				}
				if (Application.isPlaying)
				{
					AtlasManager.GetAtlas(value, base.gameObject, new AssetCallBack(this.OnLoadComplete));
				}
				this.mstrAtlasName = value;
			}
		}
	}

	public override Material material
	{
		get
		{
			return (!(this.mAtlas != null)) ? null : this.mAtlas.spriteMaterial;
		}
	}

	public UIAtlas atlas
	{
		get
		{
			return this.mAtlas;
		}
		set
		{
			if (this.mAtlas != value)
			{
				base.RemoveFromPanel();
				this.mAtlas = value;
				this.mSpriteSet = false;
				this.mSprite = null;
				if (string.IsNullOrEmpty(this.mSpriteName) && this.mAtlas != null && this.mAtlas.spriteList.Count > 0)
				{
					this.SetAtlasSprite(this.mAtlas.spriteList[0]);
					this.mSpriteName = this.mSprite.name;
				}
				if (!string.IsNullOrEmpty(this.mSpriteName))
				{
					string spriteName = this.mSpriteName;
					this.mSpriteName = string.Empty;
					this.spriteName = spriteName;
					this.MarkAsChanged();
				}
			}
		}
	}

	public string spriteName
	{
		get
		{
			return this.mSpriteName;
		}
		set
		{
			if (string.IsNullOrEmpty(value))
			{
				if (string.IsNullOrEmpty(this.mSpriteName))
				{
					return;
				}
				this.mSpriteName = string.Empty;
				this.mSprite = null;
				this.mChanged = true;
				this.mSpriteSet = false;
			}
			else if (this.mSpriteName != value)
			{
				this.mSpriteName = value;
				this.mSprite = null;
				this.mChanged = true;
				this.mSpriteSet = false;
			}
		}
	}

	public bool isValid
	{
		get
		{
			return this.GetAtlasSprite() != null;
		}
	}

	[Obsolete("Use 'centerType' instead")]
	public bool fillCenter
	{
		get
		{
			return this.centerType != UISprite.AdvancedType.Invisible;
		}
		set
		{
			if (value != (this.centerType != UISprite.AdvancedType.Invisible))
			{
				this.centerType = ((!value) ? UISprite.AdvancedType.Invisible : UISprite.AdvancedType.Sliced);
				this.MarkAsChanged();
			}
		}
	}

	public UISprite.FillDirection fillDirection
	{
		get
		{
			return this.mFillDirection;
		}
		set
		{
			if (this.mFillDirection != value)
			{
				this.mFillDirection = value;
				this.mChanged = true;
			}
		}
	}

	public float fillAmount
	{
		get
		{
			return this.mFillAmount;
		}
		set
		{
			float num = Mathf.Clamp01(value);
			if (this.mFillAmount != num)
			{
				this.mFillAmount = num;
				this.mChanged = true;
			}
		}
	}

	public int MaskCount
	{
		get
		{
			return this.maskCount;
		}
		set
		{
			if (value < 0 || value > 2)
			{
				return;
			}
			if (this.maskCount != value)
			{
				this.maskCount = value;
				this.mChanged = true;
			}
		}
	}

	public Vector4 CustomValue1
	{
		get
		{
			return this.customValue1;
		}
		set
		{
			if (value.z < 0f)
			{
				value.z = 0f;
			}
			if (value.w < 0f)
			{
				value.w = 0f;
			}
			if (this.customValue1 != value)
			{
				this.customValue1 = value;
				this.mChanged = true;
			}
		}
	}

	public Vector4 CustomValue2
	{
		get
		{
			return this.customValue2;
		}
		set
		{
			if (value.z < 0f)
			{
				value.z = 0f;
			}
			if (value.w < 0f)
			{
				value.w = 0f;
			}
			if (this.customValue2 != value)
			{
				this.customValue2 = value;
				this.mChanged = true;
			}
		}
	}

	public bool invert
	{
		get
		{
			return this.mInvert;
		}
		set
		{
			if (this.mInvert != value)
			{
				this.mInvert = value;
				this.mChanged = true;
			}
		}
	}

	public override Vector4 border
	{
		get
		{
			if (this.type != UISprite.Type.Sliced && this.type != UISprite.Type.Advanced && this.type != UISprite.Type.Custom)
			{
				return base.border;
			}
			UISpriteData atlasSprite = this.GetAtlasSprite();
			if (atlasSprite == null)
			{
				return Vector2.zero;
			}
			Vector4 zero = Vector4.zero;
			zero.x = (float)atlasSprite.borderLeft;
			zero.y = (float)atlasSprite.borderBottom;
			zero.z = (float)atlasSprite.borderRight;
			zero.w = (float)atlasSprite.borderTop;
			return zero;
		}
	}

	public override int minWidth
	{
		get
		{
			if (this.type == UISprite.Type.Sliced || this.type == UISprite.Type.Advanced)
			{
				Vector4 a = this.border;
				if (this.atlas != null)
				{
					a *= this.atlas.pixelSize;
				}
				int num = Mathf.RoundToInt(a.x + a.z);
				UISpriteData atlasSprite = this.GetAtlasSprite();
				if (atlasSprite != null)
				{
					num += atlasSprite.paddingLeft + atlasSprite.paddingRight;
				}
				return Mathf.Max(base.minWidth, ((num & 1) != 1) ? num : (num + 1));
			}
			return base.minWidth;
		}
	}

	public override int minHeight
	{
		get
		{
			if (this.type == UISprite.Type.Sliced || this.type == UISprite.Type.Advanced)
			{
				Vector4 a = this.border;
				if (this.atlas != null)
				{
					a *= this.atlas.pixelSize;
				}
				int num = Mathf.RoundToInt(a.y + a.w);
				UISpriteData atlasSprite = this.GetAtlasSprite();
				if (atlasSprite != null)
				{
					num += atlasSprite.paddingTop + atlasSprite.paddingBottom;
				}
				return Mathf.Max(base.minHeight, ((num & 1) != 1) ? num : (num + 1));
			}
			return base.minHeight;
		}
	}

	public override Vector4 drawingDimensions
	{
		get
		{
			Vector2 pivotOffset = base.pivotOffset;
			float num = -pivotOffset.x * (float)this.mWidth;
			float num2 = -pivotOffset.y * (float)this.mHeight;
			float num3 = num + (float)this.mWidth;
			float num4 = num2 + (float)this.mHeight;
			if (this.GetAtlasSprite() != null && this.mType != UISprite.Type.Tiled)
			{
				int paddingLeft = this.mSprite.paddingLeft;
				int paddingBottom = this.mSprite.paddingBottom;
				int num5 = this.mSprite.paddingRight;
				int num6 = this.mSprite.paddingTop;
				int num7 = this.mSprite.width + paddingLeft + num5;
				int num8 = this.mSprite.height + paddingBottom + num6;
				float num9 = 1f;
				float num10 = 1f;
				if (num7 > 0 && num8 > 0 && (this.mType == UISprite.Type.Simple || this.mType == UISprite.Type.Filled))
				{
					if ((num7 & 1) != 0)
					{
						num5++;
					}
					if ((num8 & 1) != 0)
					{
						num6++;
					}
					num9 = 1f / (float)num7 * (float)this.mWidth;
					num10 = 1f / (float)num8 * (float)this.mHeight;
				}
				if (this.mFlip == UISprite.Flip.Horizontally || this.mFlip == UISprite.Flip.Both)
				{
					num += (float)num5 * num9;
					num3 -= (float)paddingLeft * num9;
				}
				else
				{
					num += (float)paddingLeft * num9;
					num3 -= (float)num5 * num9;
				}
				if (this.mFlip == UISprite.Flip.Vertically || this.mFlip == UISprite.Flip.Both)
				{
					num2 += (float)num6 * num10;
					num4 -= (float)paddingBottom * num10;
				}
				else
				{
					num2 += (float)paddingBottom * num10;
					num4 -= (float)num6 * num10;
				}
			}
			if (this.atlas == null)
			{
				return Vector4.zero;
			}
			Vector4 vector = this.border * this.atlas.pixelSize;
			float num11 = vector.x + vector.z;
			float num12 = vector.y + vector.w;
			float x = Mathf.Lerp(num, num3 - num11, this.mDrawRegion.x);
			float y = Mathf.Lerp(num2, num4 - num12, this.mDrawRegion.y);
			float z = Mathf.Lerp(num + num11, num3, this.mDrawRegion.z);
			float w = Mathf.Lerp(num2 + num12, num4, this.mDrawRegion.w);
			Vector4 zero = Vector4.zero;
			zero.x = x;
			zero.y = y;
			zero.z = z;
			zero.w = w;
			return zero;
		}
	}

	protected virtual Vector4 drawingUVs
	{
		get
		{
			Vector4 zero = Vector4.zero;
			switch (this.mFlip)
			{
			case UISprite.Flip.Horizontally:
				zero.x = this.mOuterUV.xMax;
				zero.y = this.mOuterUV.yMin;
				zero.z = this.mOuterUV.xMin;
				zero.w = this.mOuterUV.yMax;
				return zero;
			case UISprite.Flip.Vertically:
				zero.x = this.mOuterUV.xMin;
				zero.y = this.mOuterUV.yMax;
				zero.z = this.mOuterUV.xMax;
				zero.w = this.mOuterUV.yMin;
				return zero;
			case UISprite.Flip.Both:
				zero.x = this.mOuterUV.xMax;
				zero.y = this.mOuterUV.yMax;
				zero.z = this.mOuterUV.xMin;
				zero.w = this.mOuterUV.yMin;
				return zero;
			default:
				zero.x = this.mOuterUV.xMin;
				zero.y = this.mOuterUV.yMin;
				zero.z = this.mOuterUV.xMax;
				zero.w = this.mOuterUV.yMax;
				return zero;
			}
		}
	}

	public void AddInitCallback(UISprite.OnInitCallback callBack)
	{
		if (!this.initCallbackList.Contains(callBack))
		{
			this.initCallbackList.Add(callBack);
		}
	}

	public UISpriteData GetAtlasSprite()
	{
		if (!this.mSpriteSet)
		{
			this.mSprite = null;
		}
		if (this.mSprite == null && this.mAtlas != null)
		{
			if (!string.IsNullOrEmpty(this.mSpriteName))
			{
				UISpriteData sprite = this.mAtlas.GetSprite(this.mSpriteName);
				if (sprite == null)
				{
					return null;
				}
				this.SetAtlasSprite(sprite);
			}
			if (this.mSprite == null && this.mAtlas.spriteList.Count > 0)
			{
				UISpriteData uISpriteData = this.mAtlas.spriteList[0];
				if (uISpriteData == null)
				{
					return null;
				}
				this.SetAtlasSprite(uISpriteData);
				if (this.mSprite == null)
				{
					LogSystem.LogWarning(new object[]
					{
						this.mAtlas.name,
						" seems to have a null sprite!"
					});
					return null;
				}
				this.mSpriteName = this.mSprite.name;
			}
		}
		return this.mSprite;
	}

	protected void SetAtlasSprite(UISpriteData sp)
	{
		this.mChanged = true;
		this.mSpriteSet = true;
		if (sp != null)
		{
			this.mSprite = sp;
			this.mSpriteName = this.mSprite.name;
		}
		else
		{
			this.mSpriteName = ((this.mSprite == null) ? string.Empty : this.mSprite.name);
			this.mSprite = sp;
		}
	}

	public override void MakePixelPerfect()
	{
		if (!this.isValid)
		{
			this.isMakePixelPerfect = true;
			return;
		}
		base.MakePixelPerfect();
		UISpriteData atlasSprite = this.GetAtlasSprite();
		if (atlasSprite == null)
		{
			return;
		}
		UISprite.Type type = this.type;
		if (type == UISprite.Type.Simple || type == UISprite.Type.Filled || !atlasSprite.hasBorder)
		{
			Texture mainTexture = this.mainTexture;
			if (mainTexture != null && atlasSprite != null)
			{
				int num = Mathf.RoundToInt(this.atlas.pixelSize * (float)(atlasSprite.width + atlasSprite.paddingLeft + atlasSprite.paddingRight));
				int num2 = Mathf.RoundToInt(this.atlas.pixelSize * (float)(atlasSprite.height + atlasSprite.paddingTop + atlasSprite.paddingBottom));
				if ((num & 1) == 1)
				{
					num++;
				}
				if ((num2 & 1) == 1)
				{
					num2++;
				}
				base.width = num;
				base.height = num2;
			}
		}
	}

	public override bool CheckWaitLoadingAtlas()
	{
		return !this.error && base.gameObject.activeInHierarchy && !string.IsNullOrEmpty(this.AtlasName) && this.atlas == null;
	}

	public override void CheckLoadAtlas()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		if (this.atlas == null && !string.IsNullOrEmpty(this.AtlasName))
		{
			AtlasManager.GetAtlas(this.AtlasName, base.gameObject, new AssetCallBack(this.OnLoadComplete));
		}
	}

	protected override void Awake()
	{
		if (this.atlas != null)
		{
			return;
		}
		if (!string.IsNullOrEmpty(this.mstrAtlasName))
		{
			AtlasManager.GetAtlas(this.mstrAtlasName, base.gameObject, new AssetCallBack(this.OnLoadComplete));
		}
	}

	private void OnLoadComplete(VarStore args)
	{
		UIAtlas uIAtlas = args[0] as UIAtlas;
		if (uIAtlas != null)
		{
			this.atlas = uIAtlas;
			this.MarkAsChanged();
		}
	}

	private void OnDestroy()
	{
		if (this.atlas != null)
		{
			this.atlas = null;
			AtlasManager.PopAtlas(this.AtlasName, base.gameObject);
		}
	}

	protected override void OnInit()
	{
		if (!this.mFillCenter)
		{
			this.mFillCenter = true;
			this.centerType = UISprite.AdvancedType.Invisible;
		}
		base.OnInit();
		if (this.initCallbackList != null && this.initCallbackList.Count > 0)
		{
			for (int i = 0; i < this.initCallbackList.Count; i++)
			{
				if (this.initCallbackList[i] != null)
				{
					this.initCallbackList[i](this);
				}
			}
			this.initCallbackList.Clear();
		}
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		if (this.mChanged || !this.mSpriteSet)
		{
			this.mSpriteSet = true;
			this.mSprite = null;
			this.mChanged = true;
		}
	}

	public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Texture mainTexture = this.mainTexture;
		if (mainTexture != null)
		{
			if (this.mSprite == null)
			{
				this.mSprite = this.atlas.GetSprite(this.spriteName);
			}
			if (this.mSprite == null)
			{
				return;
			}
			this.mOuterUV.Set((float)this.mSprite.x, (float)this.mSprite.y, (float)this.mSprite.width, (float)this.mSprite.height);
			this.mInnerUV.Set((float)(this.mSprite.x + this.mSprite.borderLeft), (float)(this.mSprite.y + this.mSprite.borderTop), (float)(this.mSprite.width - this.mSprite.borderLeft - this.mSprite.borderRight), (float)(this.mSprite.height - this.mSprite.borderBottom - this.mSprite.borderTop));
			this.mOuterUV = NGUIMath.ConvertToTexCoords(this.mOuterUV, mainTexture.width, mainTexture.height);
			this.mInnerUV = NGUIMath.ConvertToTexCoords(this.mInnerUV, mainTexture.width, mainTexture.height);
		}
		switch (this.type)
		{
		case UISprite.Type.Simple:
			this.SimpleFill(verts, uvs, cols);
			break;
		case UISprite.Type.Sliced:
			this.SlicedFill(verts, uvs, cols);
			break;
		case UISprite.Type.Tiled:
			this.TiledFill(verts, uvs, cols);
			break;
		case UISprite.Type.Filled:
			this.FilledFill(verts, uvs, cols);
			break;
		case UISprite.Type.Advanced:
			this.AdvancedFill(verts, uvs, cols);
			break;
		case UISprite.Type.Custom:
			this.CustomFill(verts, uvs, cols);
			break;
		}
	}

	protected void SimpleFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Vector4 drawingDimensions = this.drawingDimensions;
		Vector4 drawingUVs = this.drawingUVs;
		Vector3 zero = Vector3.zero;
		zero.x = drawingDimensions.x;
		zero.y = drawingDimensions.y;
		verts.Add(zero);
		zero.x = drawingDimensions.x;
		zero.y = drawingDimensions.w;
		verts.Add(zero);
		zero.x = drawingDimensions.z;
		zero.y = drawingDimensions.w;
		verts.Add(zero);
		zero.x = drawingDimensions.z;
		zero.y = drawingDimensions.y;
		verts.Add(zero);
		zero.x = drawingUVs.x;
		zero.y = drawingUVs.y;
		uvs.Add(zero);
		zero.x = drawingUVs.x;
		zero.y = drawingUVs.w;
		uvs.Add(zero);
		zero.x = drawingUVs.z;
		zero.y = drawingUVs.w;
		uvs.Add(zero);
		zero.x = drawingUVs.z;
		zero.y = drawingUVs.y;
		uvs.Add(zero);
		Color color = base.color;
		color.a = this.finalAlpha;
		if (this.atlas.premultipliedAlpha && color.a != 1f)
		{
			color.r *= color.a;
			color.g *= color.a;
			color.b *= color.a;
		}
		cols.Add(color);
		cols.Add(color);
		cols.Add(color);
		cols.Add(color);
	}

	protected void SlicedFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		if (!this.mSprite.hasBorder)
		{
			this.SimpleFill(verts, uvs, cols);
			return;
		}
		Vector4 drawingDimensions = this.drawingDimensions;
		Vector4 vector = this.border * this.atlas.pixelSize;
		UISprite.mTempPos[0].x = drawingDimensions.x;
		UISprite.mTempPos[0].y = drawingDimensions.y;
		UISprite.mTempPos[3].x = drawingDimensions.z;
		UISprite.mTempPos[3].y = drawingDimensions.w;
		if (this.mFlip == UISprite.Flip.Horizontally || this.mFlip == UISprite.Flip.Both)
		{
			UISprite.mTempPos[1].x = UISprite.mTempPos[0].x + vector.z;
			UISprite.mTempPos[2].x = UISprite.mTempPos[3].x - vector.x;
			UISprite.mTempUVs[3].x = this.mOuterUV.xMin;
			UISprite.mTempUVs[2].x = this.mInnerUV.xMin;
			UISprite.mTempUVs[1].x = this.mInnerUV.xMax;
			UISprite.mTempUVs[0].x = this.mOuterUV.xMax;
		}
		else
		{
			UISprite.mTempPos[1].x = UISprite.mTempPos[0].x + vector.x;
			UISprite.mTempPos[2].x = UISprite.mTempPos[3].x - vector.z;
			UISprite.mTempUVs[0].x = this.mOuterUV.xMin;
			UISprite.mTempUVs[1].x = this.mInnerUV.xMin;
			UISprite.mTempUVs[2].x = this.mInnerUV.xMax;
			UISprite.mTempUVs[3].x = this.mOuterUV.xMax;
		}
		if (this.mFlip == UISprite.Flip.Vertically || this.mFlip == UISprite.Flip.Both)
		{
			UISprite.mTempPos[1].y = UISprite.mTempPos[0].y + vector.w;
			UISprite.mTempPos[2].y = UISprite.mTempPos[3].y - vector.y;
			UISprite.mTempUVs[3].y = this.mOuterUV.yMin;
			UISprite.mTempUVs[2].y = this.mInnerUV.yMin;
			UISprite.mTempUVs[1].y = this.mInnerUV.yMax;
			UISprite.mTempUVs[0].y = this.mOuterUV.yMax;
		}
		else
		{
			UISprite.mTempPos[1].y = UISprite.mTempPos[0].y + vector.y;
			UISprite.mTempPos[2].y = UISprite.mTempPos[3].y - vector.w;
			UISprite.mTempUVs[0].y = this.mOuterUV.yMin;
			UISprite.mTempUVs[1].y = this.mInnerUV.yMin;
			UISprite.mTempUVs[2].y = this.mInnerUV.yMax;
			UISprite.mTempUVs[3].y = this.mOuterUV.yMax;
		}
		Color color = base.color;
		color.a = this.finalAlpha;
		Color32 item = (!this.atlas.premultipliedAlpha) ? color : NGUITools.ApplyPMA(color);
		for (int i = 0; i < 3; i++)
		{
			int num = i + 1;
			for (int j = 0; j < 3; j++)
			{
				if (this.centerType != UISprite.AdvancedType.Invisible || i != 1 || j != 1)
				{
					int num2 = j + 1;
					Vector3 zero = Vector3.zero;
					zero.x = UISprite.mTempPos[i].x;
					zero.y = UISprite.mTempPos[j].y;
					verts.Add(zero);
					zero.x = UISprite.mTempPos[i].x;
					zero.y = UISprite.mTempPos[num2].y;
					verts.Add(zero);
					zero.x = UISprite.mTempPos[num].x;
					zero.y = UISprite.mTempPos[num2].y;
					verts.Add(zero);
					zero.x = UISprite.mTempPos[num].x;
					zero.y = UISprite.mTempPos[j].y;
					verts.Add(zero);
					zero.x = UISprite.mTempUVs[i].x;
					zero.y = UISprite.mTempUVs[j].y;
					uvs.Add(zero);
					zero.x = UISprite.mTempUVs[i].x;
					zero.y = UISprite.mTempUVs[num2].y;
					uvs.Add(zero);
					zero.x = UISprite.mTempUVs[num].x;
					zero.y = UISprite.mTempUVs[num2].y;
					uvs.Add(zero);
					zero.x = UISprite.mTempUVs[num].x;
					zero.y = UISprite.mTempUVs[j].y;
					uvs.Add(zero);
					cols.Add(item);
					cols.Add(item);
					cols.Add(item);
					cols.Add(item);
				}
			}
		}
	}

	protected void TiledFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Texture mainTexture = this.material.mainTexture;
		if (mainTexture == null)
		{
			return;
		}
		Vector4 drawingDimensions = this.drawingDimensions;
		Vector4 vector;
		if (this.mFlip == UISprite.Flip.Horizontally || this.mFlip == UISprite.Flip.Both)
		{
			vector.x = this.mInnerUV.xMax;
			vector.z = this.mInnerUV.xMin;
		}
		else
		{
			vector.x = this.mInnerUV.xMin;
			vector.z = this.mInnerUV.xMax;
		}
		if (this.mFlip == UISprite.Flip.Vertically || this.mFlip == UISprite.Flip.Both)
		{
			vector.y = this.mInnerUV.yMax;
			vector.w = this.mInnerUV.yMin;
		}
		else
		{
			vector.y = this.mInnerUV.yMin;
			vector.w = this.mInnerUV.yMax;
		}
		Vector2 a = Vector2.zero;
		a.x = this.mInnerUV.width * (float)mainTexture.width;
		a.x = this.mInnerUV.height * (float)mainTexture.height;
		a *= this.atlas.pixelSize;
		Color color = base.color;
		color.a = this.finalAlpha;
		Color32 item = (!this.atlas.premultipliedAlpha) ? color : NGUITools.ApplyPMA(color);
		float num = drawingDimensions.x;
		float num2 = drawingDimensions.y;
		float x = vector.x;
		float y = vector.y;
		while (num2 < drawingDimensions.w)
		{
			num = drawingDimensions.x;
			float num3 = num2 + a.y;
			float y2 = vector.w;
			if (num3 > drawingDimensions.w)
			{
				y2 = Mathf.Lerp(vector.y, vector.w, (drawingDimensions.w - num2) / a.y);
				num3 = drawingDimensions.w;
			}
			while (num < drawingDimensions.z)
			{
				float num4 = num + a.x;
				float x2 = vector.z;
				if (num4 > drawingDimensions.z)
				{
					x2 = Mathf.Lerp(vector.x, vector.z, (drawingDimensions.z - num) / a.x);
					num4 = drawingDimensions.z;
				}
				Vector3 zero = Vector3.zero;
				zero.x = num;
				zero.y = num2;
				verts.Add(zero);
				zero.x = num;
				zero.y = num3;
				verts.Add(zero);
				zero.x = num4;
				zero.y = num3;
				verts.Add(zero);
				zero.x = num4;
				zero.y = num2;
				verts.Add(zero);
				zero.x = x;
				zero.y = y;
				uvs.Add(zero);
				zero.x = x;
				zero.y = y2;
				uvs.Add(zero);
				zero.x = x2;
				zero.y = y2;
				uvs.Add(zero);
				zero.x = x2;
				zero.y = y;
				uvs.Add(zero);
				cols.Add(item);
				cols.Add(item);
				cols.Add(item);
				cols.Add(item);
				num += a.x;
			}
			num2 += a.y;
		}
	}

	protected void FilledFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		if (this.mFillAmount < 0.001f)
		{
			return;
		}
		Color color = base.color;
		color.a = this.finalAlpha;
		Color32 item = (!this.atlas.premultipliedAlpha) ? color : NGUITools.ApplyPMA(color);
		Vector4 drawingDimensions = this.drawingDimensions;
		Vector4 drawingUVs = this.drawingUVs;
		if (this.mFillDirection == UISprite.FillDirection.Horizontal || this.mFillDirection == UISprite.FillDirection.Vertical)
		{
			if (this.mFillDirection == UISprite.FillDirection.Horizontal)
			{
				float num = (drawingUVs.z - drawingUVs.x) * this.mFillAmount;
				if (this.mInvert)
				{
					drawingDimensions.x = drawingDimensions.z - (drawingDimensions.z - drawingDimensions.x) * this.mFillAmount;
					drawingUVs.x = drawingUVs.z - num;
				}
				else
				{
					drawingDimensions.z = drawingDimensions.x + (drawingDimensions.z - drawingDimensions.x) * this.mFillAmount;
					drawingUVs.z = drawingUVs.x + num;
				}
			}
			else if (this.mFillDirection == UISprite.FillDirection.Vertical)
			{
				float num2 = (drawingUVs.w - drawingUVs.y) * this.mFillAmount;
				if (this.mInvert)
				{
					drawingDimensions.y = drawingDimensions.w - (drawingDimensions.w - drawingDimensions.y) * this.mFillAmount;
					drawingUVs.y = drawingUVs.w - num2;
				}
				else
				{
					drawingDimensions.w = drawingDimensions.y + (drawingDimensions.w - drawingDimensions.y) * this.mFillAmount;
					drawingUVs.w = drawingUVs.y + num2;
				}
			}
		}
		Vector2 zero = Vector2.zero;
		zero.x = drawingDimensions.x;
		zero.y = drawingDimensions.y;
		UISprite.mTempPos[0] = zero;
		zero.x = drawingDimensions.x;
		zero.y = drawingDimensions.w;
		UISprite.mTempPos[1] = zero;
		zero.x = drawingDimensions.z;
		zero.y = drawingDimensions.w;
		UISprite.mTempPos[2] = zero;
		zero.x = drawingDimensions.z;
		zero.y = drawingDimensions.y;
		UISprite.mTempPos[3] = zero;
		zero.x = drawingUVs.x;
		zero.y = drawingUVs.y;
		UISprite.mTempUVs[0] = zero;
		zero.x = drawingUVs.x;
		zero.y = drawingUVs.w;
		UISprite.mTempUVs[1] = zero;
		zero.x = drawingUVs.z;
		zero.y = drawingUVs.w;
		UISprite.mTempUVs[2] = zero;
		zero.x = drawingUVs.z;
		zero.y = drawingUVs.y;
		UISprite.mTempUVs[3] = zero;
		if (this.mFillAmount < 1f)
		{
			if (this.mFillDirection == UISprite.FillDirection.Radial90)
			{
				if (UISprite.RadialCut(UISprite.mTempPos, UISprite.mTempUVs, this.mFillAmount, this.mInvert, 0))
				{
					for (int i = 0; i < 4; i++)
					{
						verts.Add(UISprite.mTempPos[i]);
						uvs.Add(UISprite.mTempUVs[i]);
						cols.Add(item);
					}
				}
				return;
			}
			if (this.mFillDirection == UISprite.FillDirection.Radial180)
			{
				for (int j = 0; j < 2; j++)
				{
					float t = 0f;
					float t2 = 1f;
					float t3;
					float t4;
					if (j == 0)
					{
						t3 = 0f;
						t4 = 0.5f;
					}
					else
					{
						t3 = 0.5f;
						t4 = 1f;
					}
					UISprite.mTempPos[0].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, t3);
					UISprite.mTempPos[1].x = UISprite.mTempPos[0].x;
					UISprite.mTempPos[2].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, t4);
					UISprite.mTempPos[3].x = UISprite.mTempPos[2].x;
					UISprite.mTempPos[0].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, t);
					UISprite.mTempPos[1].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, t2);
					UISprite.mTempPos[2].y = UISprite.mTempPos[1].y;
					UISprite.mTempPos[3].y = UISprite.mTempPos[0].y;
					UISprite.mTempUVs[0].x = Mathf.Lerp(drawingUVs.x, drawingUVs.z, t3);
					UISprite.mTempUVs[1].x = UISprite.mTempUVs[0].x;
					UISprite.mTempUVs[2].x = Mathf.Lerp(drawingUVs.x, drawingUVs.z, t4);
					UISprite.mTempUVs[3].x = UISprite.mTempUVs[2].x;
					UISprite.mTempUVs[0].y = Mathf.Lerp(drawingUVs.y, drawingUVs.w, t);
					UISprite.mTempUVs[1].y = Mathf.Lerp(drawingUVs.y, drawingUVs.w, t2);
					UISprite.mTempUVs[2].y = UISprite.mTempUVs[1].y;
					UISprite.mTempUVs[3].y = UISprite.mTempUVs[0].y;
					float value = this.mInvert ? (this.mFillAmount * 2f - (float)(1 - j)) : (this.fillAmount * 2f - (float)j);
					if (UISprite.RadialCut(UISprite.mTempPos, UISprite.mTempUVs, Mathf.Clamp01(value), !this.mInvert, NGUIMath.RepeatIndex(j + 3, 4)))
					{
						for (int k = 0; k < 4; k++)
						{
							verts.Add(UISprite.mTempPos[k]);
							uvs.Add(UISprite.mTempUVs[k]);
							cols.Add(item);
						}
					}
				}
				return;
			}
			if (this.mFillDirection == UISprite.FillDirection.Radial360)
			{
				for (int l = 0; l < 4; l++)
				{
					float t5;
					float t6;
					if (l < 2)
					{
						t5 = 0f;
						t6 = 0.5f;
					}
					else
					{
						t5 = 0.5f;
						t6 = 1f;
					}
					float t7;
					float t8;
					if (l == 0 || l == 3)
					{
						t7 = 0f;
						t8 = 0.5f;
					}
					else
					{
						t7 = 0.5f;
						t8 = 1f;
					}
					UISprite.mTempPos[0].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, t5);
					UISprite.mTempPos[1].x = UISprite.mTempPos[0].x;
					UISprite.mTempPos[2].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, t6);
					UISprite.mTempPos[3].x = UISprite.mTempPos[2].x;
					UISprite.mTempPos[0].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, t7);
					UISprite.mTempPos[1].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, t8);
					UISprite.mTempPos[2].y = UISprite.mTempPos[1].y;
					UISprite.mTempPos[3].y = UISprite.mTempPos[0].y;
					UISprite.mTempUVs[0].x = Mathf.Lerp(drawingUVs.x, drawingUVs.z, t5);
					UISprite.mTempUVs[1].x = UISprite.mTempUVs[0].x;
					UISprite.mTempUVs[2].x = Mathf.Lerp(drawingUVs.x, drawingUVs.z, t6);
					UISprite.mTempUVs[3].x = UISprite.mTempUVs[2].x;
					UISprite.mTempUVs[0].y = Mathf.Lerp(drawingUVs.y, drawingUVs.w, t7);
					UISprite.mTempUVs[1].y = Mathf.Lerp(drawingUVs.y, drawingUVs.w, t8);
					UISprite.mTempUVs[2].y = UISprite.mTempUVs[1].y;
					UISprite.mTempUVs[3].y = UISprite.mTempUVs[0].y;
					float value2 = (!this.mInvert) ? (this.mFillAmount * 4f - (float)(3 - NGUIMath.RepeatIndex(l + 2, 4))) : (this.mFillAmount * 4f - (float)NGUIMath.RepeatIndex(l + 2, 4));
					if (UISprite.RadialCut(UISprite.mTempPos, UISprite.mTempUVs, Mathf.Clamp01(value2), this.mInvert, NGUIMath.RepeatIndex(l + 2, 4)))
					{
						for (int m = 0; m < 4; m++)
						{
							verts.Add(UISprite.mTempPos[m]);
							uvs.Add(UISprite.mTempUVs[m]);
							cols.Add(item);
						}
					}
				}
				return;
			}
		}
		for (int n = 0; n < 4; n++)
		{
			verts.Add(UISprite.mTempPos[n]);
			uvs.Add(UISprite.mTempUVs[n]);
			cols.Add(item);
		}
	}

	private static bool RadialCut(Vector2[] xy, Vector2[] uv, float fill, bool invert, int corner)
	{
		if (fill < 0.001f)
		{
			return false;
		}
		if ((corner & 1) == 1)
		{
			invert = !invert;
		}
		if (!invert && fill > 0.999f)
		{
			return true;
		}
		float num = Mathf.Clamp01(fill);
		if (invert)
		{
			num = 1f - num;
		}
		num *= 1.57079637f;
		float cos = Mathf.Cos(num);
		float sin = Mathf.Sin(num);
		UISprite.RadialCut(xy, cos, sin, invert, corner);
		UISprite.RadialCut(uv, cos, sin, invert, corner);
		return true;
	}

	private static void RadialCut(Vector2[] xy, float cos, float sin, bool invert, int corner)
	{
		int num = NGUIMath.RepeatIndex(corner + 1, 4);
		int num2 = NGUIMath.RepeatIndex(corner + 2, 4);
		int num3 = NGUIMath.RepeatIndex(corner + 3, 4);
		if ((corner & 1) == 1)
		{
			if (sin > cos)
			{
				cos /= sin;
				sin = 1f;
				if (invert)
				{
					xy[num].x = Mathf.Lerp(xy[corner].x, xy[num2].x, cos);
					xy[num2].x = xy[num].x;
				}
			}
			else if (cos > sin)
			{
				sin /= cos;
				cos = 1f;
				if (!invert)
				{
					xy[num2].y = Mathf.Lerp(xy[corner].y, xy[num2].y, sin);
					xy[num3].y = xy[num2].y;
				}
			}
			else
			{
				cos = 1f;
				sin = 1f;
			}
			if (!invert)
			{
				xy[num3].x = Mathf.Lerp(xy[corner].x, xy[num2].x, cos);
			}
			else
			{
				xy[num].y = Mathf.Lerp(xy[corner].y, xy[num2].y, sin);
			}
		}
		else
		{
			if (cos > sin)
			{
				sin /= cos;
				cos = 1f;
				if (!invert)
				{
					xy[num].y = Mathf.Lerp(xy[corner].y, xy[num2].y, sin);
					xy[num2].y = xy[num].y;
				}
			}
			else if (sin > cos)
			{
				cos /= sin;
				sin = 1f;
				if (invert)
				{
					xy[num2].x = Mathf.Lerp(xy[corner].x, xy[num2].x, cos);
					xy[num3].x = xy[num2].x;
				}
			}
			else
			{
				cos = 1f;
				sin = 1f;
			}
			if (invert)
			{
				xy[num3].y = Mathf.Lerp(xy[corner].y, xy[num2].y, sin);
			}
			else
			{
				xy[num].x = Mathf.Lerp(xy[corner].x, xy[num2].x, cos);
			}
		}
	}

	protected void AdvancedFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		if (!this.mSprite.hasBorder)
		{
			this.SimpleFill(verts, uvs, cols);
			return;
		}
		Texture mainTexture = this.material.mainTexture;
		if (mainTexture == null)
		{
			return;
		}
		Vector4 drawingDimensions = this.drawingDimensions;
		Vector4 vector = this.border * this.atlas.pixelSize;
		Vector2 a = Vector2.zero;
		a.x = this.mInnerUV.width * (float)mainTexture.width;
		a.y = this.mInnerUV.height * (float)mainTexture.height;
		a *= this.atlas.pixelSize;
		if (a.x < 1f)
		{
			a.x = 1f;
		}
		if (a.y < 1f)
		{
			a.y = 1f;
		}
		UISprite.mTempPos[0].x = drawingDimensions.x;
		UISprite.mTempPos[0].y = drawingDimensions.y;
		UISprite.mTempPos[3].x = drawingDimensions.z;
		UISprite.mTempPos[3].y = drawingDimensions.w;
		if (this.mFlip == UISprite.Flip.Horizontally || this.mFlip == UISprite.Flip.Both)
		{
			UISprite.mTempPos[1].x = UISprite.mTempPos[0].x + vector.z;
			UISprite.mTempPos[2].x = UISprite.mTempPos[3].x - vector.x;
			UISprite.mTempUVs[3].x = this.mOuterUV.xMin;
			UISprite.mTempUVs[2].x = this.mInnerUV.xMin;
			UISprite.mTempUVs[1].x = this.mInnerUV.xMax;
			UISprite.mTempUVs[0].x = this.mOuterUV.xMax;
		}
		else
		{
			UISprite.mTempPos[1].x = UISprite.mTempPos[0].x + vector.x;
			UISprite.mTempPos[2].x = UISprite.mTempPos[3].x - vector.z;
			UISprite.mTempUVs[0].x = this.mOuterUV.xMin;
			UISprite.mTempUVs[1].x = this.mInnerUV.xMin;
			UISprite.mTempUVs[2].x = this.mInnerUV.xMax;
			UISprite.mTempUVs[3].x = this.mOuterUV.xMax;
		}
		if (this.mFlip == UISprite.Flip.Vertically || this.mFlip == UISprite.Flip.Both)
		{
			UISprite.mTempPos[1].y = UISprite.mTempPos[0].y + vector.w;
			UISprite.mTempPos[2].y = UISprite.mTempPos[3].y - vector.y;
			UISprite.mTempUVs[3].y = this.mOuterUV.yMin;
			UISprite.mTempUVs[2].y = this.mInnerUV.yMin;
			UISprite.mTempUVs[1].y = this.mInnerUV.yMax;
			UISprite.mTempUVs[0].y = this.mOuterUV.yMax;
		}
		else
		{
			UISprite.mTempPos[1].y = UISprite.mTempPos[0].y + vector.y;
			UISprite.mTempPos[2].y = UISprite.mTempPos[3].y - vector.w;
			UISprite.mTempUVs[0].y = this.mOuterUV.yMin;
			UISprite.mTempUVs[1].y = this.mInnerUV.yMin;
			UISprite.mTempUVs[2].y = this.mInnerUV.yMax;
			UISprite.mTempUVs[3].y = this.mOuterUV.yMax;
		}
		Color color = base.color;
		color.a = this.finalAlpha;
		Color32 c = (!this.atlas.premultipliedAlpha) ? color : NGUITools.ApplyPMA(color);
		for (int i = 0; i < 3; i++)
		{
			int num = i + 1;
			for (int j = 0; j < 3; j++)
			{
				if (this.centerType != UISprite.AdvancedType.Invisible || i != 1 || j != 1)
				{
					int num2 = j + 1;
					if (i == 1 && j == 1)
					{
						if (this.centerType == UISprite.AdvancedType.Tiled)
						{
							float x = UISprite.mTempPos[i].x;
							float x2 = UISprite.mTempPos[num].x;
							float y = UISprite.mTempPos[j].y;
							float y2 = UISprite.mTempPos[num2].y;
							float x3 = UISprite.mTempUVs[i].x;
							float y3 = UISprite.mTempUVs[j].y;
							for (float num3 = y; num3 < y2; num3 += a.y)
							{
								float num4 = x;
								float num5 = UISprite.mTempUVs[num2].y;
								float num6 = num3 + a.y;
								if (num6 > y2)
								{
									num5 = Mathf.Lerp(y3, num5, (y2 - num3) / a.y);
									num6 = y2;
								}
								while (num4 < x2)
								{
									float num7 = num4 + a.x;
									float num8 = UISprite.mTempUVs[num].x;
									if (num7 > x2)
									{
										num8 = Mathf.Lerp(x3, num8, (x2 - num4) / a.x);
										num7 = x2;
									}
									this.FillBuffers(num4, num7, num3, num6, x3, num8, y3, num5, c, verts, uvs, cols);
									num4 += a.x;
								}
							}
						}
						else if (this.centerType == UISprite.AdvancedType.Sliced)
						{
							this.FillBuffers(UISprite.mTempPos[i].x, UISprite.mTempPos[num].x, UISprite.mTempPos[j].y, UISprite.mTempPos[num2].y, UISprite.mTempUVs[i].x, UISprite.mTempUVs[num].x, UISprite.mTempUVs[j].y, UISprite.mTempUVs[num2].y, c, verts, uvs, cols);
						}
					}
					else if (i == 1)
					{
						if ((j == 0 && this.bottomType == UISprite.AdvancedType.Tiled) || (j == 2 && this.topType == UISprite.AdvancedType.Tiled))
						{
							float x4 = UISprite.mTempPos[i].x;
							float x5 = UISprite.mTempPos[num].x;
							float y4 = UISprite.mTempPos[j].y;
							float y5 = UISprite.mTempPos[num2].y;
							float x6 = UISprite.mTempUVs[i].x;
							float y6 = UISprite.mTempUVs[j].y;
							float y7 = UISprite.mTempUVs[num2].y;
							for (float num9 = x4; num9 < x5; num9 += a.x)
							{
								float num10 = num9 + a.x;
								float num11 = UISprite.mTempUVs[num].x;
								if (num10 > x5)
								{
									num11 = Mathf.Lerp(x6, num11, (x5 - num9) / a.x);
									num10 = x5;
								}
								this.FillBuffers(num9, num10, y4, y5, x6, num11, y6, y7, c, verts, uvs, cols);
							}
						}
						else if ((j == 0 && this.bottomType == UISprite.AdvancedType.Sliced) || (j == 2 && this.topType == UISprite.AdvancedType.Sliced))
						{
							this.FillBuffers(UISprite.mTempPos[i].x, UISprite.mTempPos[num].x, UISprite.mTempPos[j].y, UISprite.mTempPos[num2].y, UISprite.mTempUVs[i].x, UISprite.mTempUVs[num].x, UISprite.mTempUVs[j].y, UISprite.mTempUVs[num2].y, c, verts, uvs, cols);
						}
					}
					else if (j == 1)
					{
						if ((i == 0 && this.leftType == UISprite.AdvancedType.Tiled) || (i == 2 && this.rightType == UISprite.AdvancedType.Tiled))
						{
							float x7 = UISprite.mTempPos[i].x;
							float x8 = UISprite.mTempPos[num].x;
							float y8 = UISprite.mTempPos[j].y;
							float y9 = UISprite.mTempPos[num2].y;
							float x9 = UISprite.mTempUVs[i].x;
							float x10 = UISprite.mTempUVs[num].x;
							float y10 = UISprite.mTempUVs[j].y;
							for (float num12 = y8; num12 < y9; num12 += a.y)
							{
								float num13 = UISprite.mTempUVs[num2].y;
								float num14 = num12 + a.y;
								if (num14 > y9)
								{
									num13 = Mathf.Lerp(y10, num13, (y9 - num12) / a.y);
									num14 = y9;
								}
								this.FillBuffers(x7, x8, num12, num14, x9, x10, y10, num13, c, verts, uvs, cols);
							}
						}
						else if ((i == 0 && this.leftType == UISprite.AdvancedType.Sliced) || (i == 2 && this.rightType == UISprite.AdvancedType.Sliced))
						{
							this.FillBuffers(UISprite.mTempPos[i].x, UISprite.mTempPos[num].x, UISprite.mTempPos[j].y, UISprite.mTempPos[num2].y, UISprite.mTempUVs[i].x, UISprite.mTempUVs[num].x, UISprite.mTempUVs[j].y, UISprite.mTempUVs[num2].y, c, verts, uvs, cols);
						}
					}
					else
					{
						this.FillBuffers(UISprite.mTempPos[i].x, UISprite.mTempPos[num].x, UISprite.mTempPos[j].y, UISprite.mTempPos[num2].y, UISprite.mTempUVs[i].x, UISprite.mTempUVs[num].x, UISprite.mTempUVs[j].y, UISprite.mTempUVs[num2].y, c, verts, uvs, cols);
					}
				}
			}
		}
	}

	protected void CustomFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Texture mainTexture = this.material.mainTexture;
		if (mainTexture == null)
		{
			return;
		}
		Vector4 drawingDimensions = this.drawingDimensions;
		Vector4 vector;
		vector.x = this.mInnerUV.xMin;
		vector.z = this.mInnerUV.xMax;
		vector.y = this.mInnerUV.yMin;
		vector.w = this.mInnerUV.yMax;
		Vector2 a = Vector2.zero;
		a.x = this.mInnerUV.width * (float)mainTexture.width;
		a.y = this.mInnerUV.height * (float)mainTexture.height;
		a *= this.atlas.pixelSize;
		Color color = base.color;
		color.a = this.finalAlpha;
		Color32 col = (!this.atlas.premultipliedAlpha) ? color : NGUITools.ApplyPMA(color);
		float x = drawingDimensions.x;
		float y = drawingDimensions.y;
		float x2 = vector.x;
		float y2 = vector.y;
		float w = vector.w;
		float z = vector.z;
		if (this.maskCount == 1)
		{
			float num = this.customValue1.x;
			if (num < drawingDimensions.x)
			{
				num = drawingDimensions.x;
			}
			else if (num > drawingDimensions.z)
			{
				num = drawingDimensions.z;
			}
			float num2 = num + this.customValue1.z;
			if (num2 > drawingDimensions.z)
			{
				num2 = drawingDimensions.z;
			}
			float num3 = this.customValue1.y;
			if (num3 < drawingDimensions.y)
			{
				num3 = drawingDimensions.y;
			}
			else if (num3 > drawingDimensions.w)
			{
				num3 = drawingDimensions.w;
			}
			float num4 = num3 + this.customValue1.w;
			if (num4 > drawingDimensions.w)
			{
				num4 = drawingDimensions.w;
			}
			this.SetUVandVert(x, drawingDimensions.z, y, num3, x2, y2, z, w, col, verts, uvs, cols);
			this.SetUVandVert(x, drawingDimensions.z, num4, drawingDimensions.w, x2, y2, z, w, col, verts, uvs, cols);
			this.SetUVandVert(x, num, num3, num4, x2, y2, z, w, col, verts, uvs, cols);
			this.SetUVandVert(num2, drawingDimensions.z, num3, num4, x2, y2, z, w, col, verts, uvs, cols);
		}
		else
		{
			float num5 = this.customValue1.x;
			if (num5 < drawingDimensions.x)
			{
				num5 = drawingDimensions.x;
			}
			else if (num5 > drawingDimensions.z)
			{
				num5 = drawingDimensions.z;
			}
			float num6 = num5 + this.customValue1.z;
			if (num6 > drawingDimensions.z)
			{
				num6 = drawingDimensions.z;
			}
			float num7 = this.customValue1.y;
			if (num7 < drawingDimensions.y)
			{
				num7 = drawingDimensions.y;
			}
			else if (num7 > drawingDimensions.w)
			{
				num7 = drawingDimensions.w;
			}
			float num8 = num7 + this.customValue1.w;
			if (num8 > drawingDimensions.w)
			{
				num8 = drawingDimensions.w;
			}
			float num9 = this.customValue2.x;
			if (num9 < drawingDimensions.x)
			{
				num9 = drawingDimensions.x;
			}
			else if (num9 > drawingDimensions.z)
			{
				num9 = drawingDimensions.z;
			}
			float num10 = num9 + this.customValue2.z;
			if (num10 > drawingDimensions.z)
			{
				num10 = drawingDimensions.z;
			}
			float num11 = this.customValue2.y;
			if (num11 < drawingDimensions.y)
			{
				num11 = drawingDimensions.y;
			}
			else if (num11 > drawingDimensions.w)
			{
				num11 = drawingDimensions.w;
			}
			float num12 = num11 + this.customValue2.w;
			if (num12 > drawingDimensions.w)
			{
				num12 = drawingDimensions.w;
			}
			float num13 = (num5 >= num9) ? num9 : num5;
			float num14 = (num6 >= num10) ? num6 : num10;
			float num15 = (num7 >= num11) ? num11 : num7;
			float num16 = (num8 >= num12) ? num8 : num12;
			this.SetUVandVert(x, drawingDimensions.z, y, num15, x2, y2, z, w, col, verts, uvs, cols);
			this.SetUVandVert(x, drawingDimensions.z, num16, drawingDimensions.w, x2, y2, z, w, col, verts, uvs, cols);
			this.SetUVandVert(x, num13, num15, num16, x2, y2, z, w, col, verts, uvs, cols);
			this.SetUVandVert(num14, drawingDimensions.z, num15, num16, x2, y2, z, w, col, verts, uvs, cols);
			Vector2 zero = Vector2.zero;
			zero.x = (num5 + num6) / 2f;
			zero.y = (num7 + num8) / 2f;
			Vector2 zero2 = Vector2.zero;
			zero2.x = (num9 + num10) / 2f;
			zero2.y = (num11 + num12) / 2f;
			float num17 = (num5 >= num9) ? num5 : num9;
			float num18 = (num6 >= num10) ? num10 : num6;
			float num19 = (num7 >= num11) ? num7 : num11;
			float num20 = (num8 >= num12) ? num12 : num8;
			if (zero.y > zero2.y)
			{
				this.SetUVandVert(num13, num17, num15, num19, x2, y2, z, w, col, verts, uvs, cols);
				if (num17 > num18)
				{
					if (num20 < num19)
					{
						this.SetUVandVert(num18, num17, num19, num16, x2, y2, z, w, col, verts, uvs, cols);
						this.SetUVandVert(num17, num14, num20, num16, x2, y2, z, w, col, verts, uvs, cols);
					}
					else
					{
						this.SetUVandVert(num18, num14, num20, num16, x2, y2, z, w, col, verts, uvs, cols);
						this.SetUVandVert(num18, num17, num19, num20, x2, y2, z, w, col, verts, uvs, cols);
					}
				}
				else
				{
					this.SetUVandVert(num18, num14, num20, num16, x2, y2, z, w, col, verts, uvs, cols);
					if (num20 < num19)
					{
						this.SetUVandVert(num17, num18, num20, num19, x2, y2, z, w, col, verts, uvs, cols);
					}
				}
			}
			else
			{
				this.SetUVandVert(num13, num17, num20, num16, x2, y2, z, w, col, verts, uvs, cols);
				if (num17 > num18)
				{
					if (num20 < num19)
					{
						this.SetUVandVert(num17, num14, num15, num19, x2, y2, z, w, col, verts, uvs, cols);
						this.SetUVandVert(num18, num17, num15, num20, x2, y2, z, w, col, verts, uvs, cols);
					}
					else
					{
						this.SetUVandVert(num18, num14, num15, num19, x2, y2, z, w, col, verts, uvs, cols);
						this.SetUVandVert(num18, num17, num19, num20, x2, y2, z, w, col, verts, uvs, cols);
					}
				}
				else
				{
					this.SetUVandVert(num18, num14, num15, num19, x2, y2, z, w, col, verts, uvs, cols);
					if (num20 < num19)
					{
						this.SetUVandVert(num17, num18, num20, num19, x2, y2, z, w, col, verts, uvs, cols);
					}
				}
			}
		}
	}

	private void SetUVandVert(float left, float right, float bottom, float top, float u0, float v0, float u1, float v1, Color32 col, BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Vector3 zero = Vector3.zero;
		zero.x = left;
		zero.y = bottom;
		verts.Add(zero);
		zero.x = left;
		zero.y = top;
		verts.Add(zero);
		zero.x = right;
		zero.y = top;
		verts.Add(zero);
		zero.x = right;
		zero.y = bottom;
		verts.Add(zero);
		zero.x = u0;
		zero.y = v0;
		uvs.Add(zero);
		zero.x = u0;
		zero.y = v1;
		uvs.Add(zero);
		zero.x = u1;
		zero.y = v1;
		uvs.Add(zero);
		zero.x = u1;
		zero.y = v0;
		uvs.Add(zero);
		cols.Add(col);
		cols.Add(col);
		cols.Add(col);
		cols.Add(col);
	}

	private void FillBuffers(float v0x, float v1x, float v0y, float v1y, float u0x, float u1x, float u0y, float u1y, Color col, BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Vector3 zero = Vector3.zero;
		zero.x = v0x;
		zero.y = v0y;
		verts.Add(zero);
		zero.x = v0x;
		zero.y = v1y;
		verts.Add(zero);
		zero.x = v1x;
		zero.y = v1y;
		verts.Add(zero);
		zero.x = v1x;
		zero.y = v0y;
		verts.Add(zero);
		zero.x = u0x;
		zero.y = u0y;
		uvs.Add(zero);
		zero.x = u0x;
		zero.y = u1y;
		uvs.Add(zero);
		zero.x = u1x;
		zero.y = u1y;
		uvs.Add(zero);
		zero.x = u1x;
		zero.y = u0y;
		uvs.Add(zero);
		cols.Add(col);
		cols.Add(col);
		cols.Add(col);
		cols.Add(col);
	}
}
