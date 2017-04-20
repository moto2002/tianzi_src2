using System;
using UnityEngine;

namespace com.snailgames.chosen.structs
{
	[Serializable]
	public class AssetInfo
	{
		public enum TYPERES
		{
			ETE_UNDEFINE,
			ETE_CONFIG_TXT,
			ETE_CONFIG_XML,
			ETE_CONFIG_INI,
			ETE_CONFIG_BYTES,
			ETE_PREFAB,
			ETE_FBX,
			ETE_MAT,
			ETE_TEXTURE2D_PNG,
			ETE_TEXTURE2D_TGA,
			ETE_TEXTURE2D_JPG,
			ETE_TEXTURE2D_BMP,
			ETE_SCENE,
			ETE_SCRIPT,
			ETE_MUSIC_OGG,
			ETE_MUSIC_WAV,
			ETE_FONT,
			ETE_ANIM,
			ETE_MAX
		}

		[SerializeField]
		public string mstrBundlePath;

		[SerializeField]
		public string mstrDependBundle;

		[SerializeField]
		public AssetInfo.TYPERES meAType;

		[SerializeField]
		public string[] mstrDependFiles;
	}
}
