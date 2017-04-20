using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Unity.IO.Compression;
using UnityEngine;
using ZXing;
using ZXing.QrCode;

public class AssetFileUtils
{
	private static StringBuilder mstrbuilder = new StringBuilder();

	private static byte[] gZipContent = new byte[4096];

	public static bool DeleteAsset(string strFilePath)
	{
		try
		{
			if (File.Exists(strFilePath))
			{
				File.Delete(strFilePath);
				return true;
			}
		}
		catch (Exception ex)
		{
			LogSystem.LogError(new object[]
			{
				ex.ToString()
			});
		}
		return false;
	}

	public static string StringBuilder(params object[] args)
	{
		AssetFileUtils.mstrbuilder.Remove(0, AssetFileUtils.mstrbuilder.Length);
		if (args != null)
		{
			int num = args.Length;
			for (int i = 0; i < num; i++)
			{
				AssetFileUtils.mstrbuilder.Append(args[i]);
			}
		}
		return AssetFileUtils.mstrbuilder.ToString();
	}

	public static float FloatParse(string value, float defaultValue = 0f)
	{
		if (string.IsNullOrEmpty(value))
		{
			return defaultValue;
		}
		value = value.Trim();
		float result;
		if (float.TryParse(value, out result))
		{
			return result;
		}
		return defaultValue;
	}

	public static string[] Split(string src, char p)
	{
		return src.Split(new char[]
		{
			p
		});
	}

	public static int IntParse(string value, int defaultValue = 0)
	{
		if (string.IsNullOrEmpty(value))
		{
			return defaultValue;
		}
		value = value.Trim();
		int result;
		if (int.TryParse(value, out result))
		{
			return result;
		}
		return defaultValue;
	}

	public static bool BoolParse(string value, bool defaultValue = false)
	{
		if (string.IsNullOrEmpty(value))
		{
			return defaultValue;
		}
		value = value.Trim();
		bool result;
		if (bool.TryParse(value, out result))
		{
			return result;
		}
		int num;
		if (int.TryParse(value, out num))
		{
			return num == 1;
		}
		return defaultValue;
	}

	public static int[,] ParseInts(string str)
	{
		int[,] array = null;
		if (!string.IsNullOrEmpty(str))
		{
			string[] array2 = str.Split(new char[]
			{
				','
			});
			if (array2.Length % 2 == 0)
			{
				int num = array2.Length / 2;
				array = new int[num, 2];
				for (int i = 0; i < num; i++)
				{
					array[i, 0] = int.Parse(array2[2 * i]);
					array[i, 1] = int.Parse(array2[2 * i + 1]);
				}
			}
		}
		return array;
	}

	public static void PropItem(string propvaule, ref string itemid, ref int itemcount)
	{
		if (!string.IsNullOrEmpty(propvaule))
		{
			string[] array = propvaule.Split(new char[]
			{
				':'
			});
			if (array.Length != 2)
			{
				LogSystem.LogWarning(new object[]
				{
					"propvaule can't split :" + propvaule
				});
				return;
			}
			itemid = array[0];
			int.TryParse(array[1], out itemcount);
		}
	}

	public static Dictionary<string, int> FormatKeyNumber(string value, char[] first, char[] second)
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		string[] array = value.Split(first, StringSplitOptions.RemoveEmptyEntries);
		string text = string.Empty;
		for (int i = 0; i < array.Length; i++)
		{
			text = array[i];
			string[] array2 = text.Split(second, StringSplitOptions.RemoveEmptyEntries);
			if (array2.Length >= 2)
			{
				if (dictionary.ContainsKey(array2[0]))
				{
					string key = array2[0];
					int num = dictionary[key];
					int num2 = 0;
					int.TryParse(array2[1], out num2);
					dictionary[key] = num + num2;
				}
				else
				{
					int value2 = 0;
					int.TryParse(array2[1], out value2);
					dictionary.Add(array2[0], value2);
				}
			}
		}
		return dictionary;
	}

	public static Dictionary<int, int> FormatNumberKey(string value, char[] first, char[] second)
	{
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		string[] array = value.Split(first, StringSplitOptions.RemoveEmptyEntries);
		string text = string.Empty;
		for (int i = 0; i < array.Length; i++)
		{
			text = array[i];
			string[] array2 = text.Split(second, StringSplitOptions.RemoveEmptyEntries);
			if (array2.Length >= 2)
			{
				int key = AssetFileUtils.IntParse(array2[0], 0);
				if (dictionary.ContainsKey(key))
				{
					int num = dictionary[key];
					int num2 = AssetFileUtils.IntParse(array2[1], 0);
					dictionary[key] = num + num2;
				}
				else
				{
					dictionary.Add(key, AssetFileUtils.IntParse(array2[1], 0));
				}
			}
		}
		return dictionary;
	}

	public static int GenerateKey(int parameter1, int parameter2)
	{
		return parameter2 << 16 | parameter1;
	}

	public static int GenerateKey(int parameter1, int parameter2, int parameter3)
	{
		return parameter3 << 16 | parameter2 << 8 | parameter1;
	}

	public static void DecompressFileLZMA(string directory, string fileName, byte[] bytes)
	{
		using (MemoryStream memoryStream = new MemoryStream(bytes))
		{
			try
			{
				AssetFileUtils.CreateDirectory(directory);
				FileStream fileStream = new FileStream(AssetFileUtils.StringBuilder(new object[]
				{
					directory,
					fileName
				}), FileMode.Create);
				StreamZip.Unzip(memoryStream, fileStream);
				fileStream.Flush();
				fileStream.Close();
				memoryStream.Close();
			}
			catch (Exception ex)
			{
				LogSystem.LogError(new object[]
				{
					"DecompressFileLZMA : ",
					ex.ToString()
				});
			}
		}
	}

	public static byte[] DecompressFileLZMAByBytes(byte[] wwwbytes)
	{
		byte[] result;
		using (MemoryStream memoryStream = new MemoryStream(wwwbytes))
		{
			MemoryStream memoryStream2 = new MemoryStream();
			StreamZip.Unzip(memoryStream, memoryStream2);
			result = memoryStream2.ToArray();
		}
		return result;
	}

	public static void CreateDirectory(string directory)
	{
		FileInfo fileInfo = new FileInfo(directory);
		if (fileInfo.Exists)
		{
			AssetFileUtils.DeleteAsset(directory);
		}
		if (!fileInfo.Exists)
		{
			Directory.CreateDirectory(fileInfo.DirectoryName);
		}
	}

	public static bool WriteLocalAsset(string strPath, byte[] bytes)
	{
		FileInfo fileInfo = new FileInfo(strPath);
		if (fileInfo.Exists && !AssetFileUtils.DeleteAsset(strPath))
		{
			return false;
		}
		try
		{
			if (!fileInfo.Exists)
			{
				Directory.CreateDirectory(fileInfo.DirectoryName);
			}
			AssetFileUtils.WriteFile(strPath, bytes);
			return true;
		}
		catch (Exception ex)
		{
			LogSystem.LogWarning(new object[]
			{
				"WriteLocalAsset",
				ex.ToString()
			});
		}
		return false;
	}

	public static void WriteFile(string filePath, object data)
	{
		FileStream fileStream = File.OpenWrite(filePath);
		fileStream.Position = 0L;
		fileStream.SetLength(0L);
		if (data is string)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(data as string);
			fileStream.Write(bytes, 0, bytes.Length);
		}
		else
		{
			fileStream.Write(data as byte[], 0, (data as byte[]).Length);
		}
		fileStream.Flush();
		fileStream.Close();
	}

	public static byte[] DecompressGzip(byte[] bytes)
	{
		MemoryStream memoryStream = new MemoryStream(bytes);
		memoryStream.Position = 0L;
		byte[] result = AssetFileUtils.ReadGZipEncryptFile(memoryStream);
		memoryStream.Close();
		return result;
	}

	private static byte[] ReadGZipEncryptFile(MemoryStream zipStream)
	{
		zipStream.Position = 0L;
		GZipStream gZipStream = new GZipStream(zipStream, CompressionMode.Decompress, true);
		MemoryStream memoryStream = new MemoryStream();
		int num;
		do
		{
			num = gZipStream.Read(AssetFileUtils.gZipContent, 0, AssetFileUtils.gZipContent.Length);
			if (num > 0)
			{
				memoryStream.Write(AssetFileUtils.gZipContent, 0, num);
			}
		}
		while (num > 0);
		gZipStream.Close();
		memoryStream.Flush();
		byte[] result = null;
		if (memoryStream.Length > 0L)
		{
			result = memoryStream.ToArray();
		}
		memoryStream.Close();
		return result;
	}

	public static void DecompressGzipFiles(string strRootDestDir, byte[] zipBytes)
	{
		MemoryStream memoryStream = new MemoryStream(zipBytes);
		AssetFileUtils.ReadGZipFileToStream(strRootDestDir, memoryStream);
		memoryStream.Close();
	}

	public static void DecompressGzipFile(string strRootDestDir, byte[] zipBytes)
	{
		MemoryStream memoryStream = new MemoryStream(zipBytes);
		int num = strRootDestDir.LastIndexOf("/");
		if (num > 0)
		{
			string path = strRootDestDir.Substring(0, num);
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			if (File.Exists(strRootDestDir))
			{
				File.Delete(strRootDestDir);
			}
			FileStream fileStream = new FileStream(strRootDestDir, FileMode.CreateNew);
			byte[] array = AssetFileUtils.ReadGZipEncryptFile(memoryStream);
			fileStream.Write(array, 0, array.Length);
			fileStream.Flush();
			fileStream.Close();
		}
		memoryStream.Close();
	}

	public static byte[] GetMd5Bytes(byte[] bytes)
	{
		byte[] result;
		using (MD5 mD = MD5.Create())
		{
			byte[] array = mD.ComputeHash(bytes);
			result = array;
		}
		return result;
	}

	public static string GetStringMd5(string strValue)
	{
		string result;
		using (MD5 mD = MD5.Create())
		{
			byte[] bytes = Encoding.Default.GetBytes(strValue);
			byte[] array = mD.ComputeHash(bytes);
			string text = string.Empty;
			for (int i = 0; i < array.Length; i++)
			{
				text = AssetFileUtils.StringBuilder(new object[]
				{
					text,
					array[i].ToString("X")
				});
			}
			result = text;
		}
		return result;
	}

	public static string GetMd5(string directory)
	{
		string md;
		using (FileStream fileStream = new FileStream(directory, FileMode.Open))
		{
			md = AssetFileUtils.GetMd5(fileStream);
		}
		return md;
	}

	public static string GetMd5(Stream file)
	{
		string result;
		try
		{
			using (MD5 mD = MD5.Create())
			{
				byte[] array = mD.ComputeHash(file);
				string text = string.Empty;
				for (int i = 0; i < array.Length; i++)
				{
					text = AssetFileUtils.StringBuilder(new object[]
					{
						text,
						array[i].ToString("X2")
					});
				}
				result = text;
			}
		}
		catch (Exception ex)
		{
			LogSystem.LogError(new object[]
			{
				"AssetFileUtils::GetMd5() ",
				ex.ToString()
			});
			result = string.Empty;
		}
		return result;
	}

	public static string GetMd5(byte[] bytes)
	{
		string result;
		using (MD5 mD = MD5.Create())
		{
			byte[] array = mD.ComputeHash(bytes);
			string text = string.Empty;
			for (int i = 0; i < array.Length; i++)
			{
				text = AssetFileUtils.StringBuilder(new object[]
				{
					text,
					array[i].ToString("X2")
				});
			}
			result = text;
		}
		return result;
	}

	private static void ReadGZipFileToStream(string strRootDestDir, MemoryStream fStream)
	{
		while (fStream.Position < fStream.Length)
		{
			byte[] array = new byte[4];
			int num = fStream.Read(array, 0, array.Length);
			if (num != 4)
			{
				break;
			}
			int num2 = (int)array[0] | (int)array[1] << 8 | (int)array[2] << 16 | (int)array[3] << 24;
			byte[] array2 = new byte[num2];
			num = fStream.Read(array2, 0, num2);
			if (num != num2)
			{
				break;
			}
			byte[] array3 = new byte[4];
			num = fStream.Read(array3, 0, array3.Length);
			if (num != 4)
			{
				break;
			}
			int num3 = (int)array3[0] | (int)array3[1] << 8 | (int)array3[2] << 16 | (int)array3[3] << 24;
			byte[] buffer = new byte[num3];
			num = fStream.Read(buffer, 0, num3);
			if (num != num3)
			{
				break;
			}
			byte[] array4 = new byte[4];
			num = fStream.Read(array4, 0, array4.Length);
			if (num != 4)
			{
				break;
			}
			int num4 = (int)array4[0] | (int)array4[1] << 8 | (int)array4[2] << 16 | (int)array4[3] << 24;
			byte[] buffer2 = new byte[num4];
			num = fStream.Read(buffer2, 0, num4);
			if (num != num4)
			{
				break;
			}
			string @string = Encoding.ASCII.GetString(array2);
			string text = strRootDestDir + "/" + @string;
			int num5 = text.LastIndexOf("/");
			if (num5 > 0)
			{
				string path = text.Substring(0, num5);
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
				if (File.Exists(text))
				{
					File.Delete(text);
				}
				MemoryStream memoryStream = new MemoryStream();
				memoryStream.Position = 0L;
				memoryStream.Write(buffer2, 0, num4);
				memoryStream.Flush();
				byte[] array5 = AssetFileUtils.ReadGZipEncryptFile(memoryStream);
				FileStream fileStream = new FileStream(text, FileMode.CreateNew);
				fileStream.Write(array5, 0, array5.Length);
				fileStream.Flush();
				fileStream.Close();
				memoryStream.Close();
			}
		}
	}

	public static void ReadString(string text, ref string[] valuesArr, ref Dictionary<string, int> dictionary)
	{
		if (string.IsNullOrEmpty(text))
		{
			LogSystem.LogWarning(new object[]
			{
				"Config error , not found TaskString text!"
			});
			return;
		}
		List<string> list = new List<string>();
		string[] array = text.Split(new string[]
		{
			"\r\n"
		}, StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < array.Length; i++)
		{
			string[] array2 = array[i].Split(new string[]
			{
				"="
			}, 2, StringSplitOptions.RemoveEmptyEntries);
			if (array2.Length == 2)
			{
				string text2 = array2[0];
				string text3 = array2[1];
				text3 = text3.Replace("[n]", "\n");
				if (dictionary.ContainsKey(text2))
				{
					LogSystem.LogWarning(new object[]
					{
						"ReadString() has same key!:" + text2
					});
				}
				else
				{
					int num = list.IndexOf(text3);
					if (num > -1)
					{
						dictionary.Add(text2, num);
					}
					else
					{
						list.Add(text3);
						dictionary.Add(text2, list.Count - 1);
					}
				}
			}
		}
		valuesArr = list.ToArray();
	}

	public static void ReadString(string strText, ref Dictionary<string, string> DictMap)
	{
		if (string.IsNullOrEmpty(strText))
		{
			LogSystem.LogWarning(new object[]
			{
				"Config error , not found TaskString text!"
			});
			return;
		}
		string[] array = strText.Split(new string[]
		{
			"\r\n"
		}, StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < array.Length; i++)
		{
			string[] array2 = array[i].Split(new string[]
			{
				"="
			}, 2, StringSplitOptions.RemoveEmptyEntries);
			if (array2.Length == 2)
			{
				if (DictMap.ContainsKey(array2[0]))
				{
					LogSystem.LogWarning(new object[]
					{
						"the key is echo in local file!!! please check the key = ",
						array2[0]
					});
				}
				else
				{
					array2[1] = array2[1].Replace("[n]", "\n");
					DictMap[array2[0]] = array2[1];
				}
			}
		}
	}

	public static Texture GetQrCodeTexture(string textForEncoding, int width, int height)
	{
		Texture2D texture2D = new Texture2D(width, height);
		if (!string.IsNullOrEmpty(textForEncoding))
		{
			QrCodeEncodingOptions qrCodeEncodingOptions = new QrCodeEncodingOptions();
			qrCodeEncodingOptions.Height = texture2D.height;
			qrCodeEncodingOptions.Width = texture2D.width;
			Color32[] pixels = new BarcodeWriter
			{
				Format = BarcodeFormat.QR_CODE,
				Options = qrCodeEncodingOptions
			}.Write(textForEncoding);
			texture2D.SetPixels32(pixels);
			texture2D.Apply();
		}
		return texture2D;
	}
}
