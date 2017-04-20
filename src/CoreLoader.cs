using System;
using System.Reflection;
using System.Security.Cryptography;

public class CoreLoader
{
	public static Assembly LoadDllFile(byte[] codeContext)
	{
		return CoreLoader.DecodeDllFile(codeContext);
	}

	private static Assembly DecodeDllFile(byte[] CodeContext)
	{
		if (CodeContext == null || CodeContext.Length < 4)
		{
			return null;
		}
		int num = (int)CodeContext[0];
		num += (int)CodeContext[1] << 8;
		num += (int)CodeContext[2] << 16;
		num += (int)CodeContext[3] << 24;
		byte[] array = new byte[num];
		Array.Copy(CodeContext, 9, array, 0, num);
		int num2 = (int)CodeContext[4];
		num2 += (int)CodeContext[5] << 8;
		num2 += (int)CodeContext[6] << 16;
		num2 += (int)CodeContext[7] << 24;
		byte bExType = CodeContext[8];
		byte[] array2 = new byte[num2];
		Array.Copy(CodeContext, 9 + num, array2, 0, num2);
		bool flag = CoreLoader.RecoverCode(bExType, ref array2);
		if (!flag)
		{
			return null;
		}
		byte[] md5Bytes = CoreLoader.GetMd5Bytes(array2);
		if (md5Bytes == null || array == null || md5Bytes.Length != array.Length)
		{
			return null;
		}
		for (int i = 0; i < md5Bytes.Length; i++)
		{
			if (md5Bytes[i] != array[i])
			{
				return null;
			}
		}
		return Assembly.Load(array2);
	}

	private static bool RecoverCode(byte bExType, ref byte[] context)
	{
		switch (bExType)
		{
		case 0:
			return CoreLoader.ExchangeContext0(ref context);
		case 1:
			return CoreLoader.ExchangeContext1(ref context);
		case 2:
			return CoreLoader.ExchangeContext2(ref context);
		case 3:
			return CoreLoader.ExchangeContext3(ref context);
		case 4:
			return CoreLoader.ExchangeContext4(ref context);
		case 5:
			return CoreLoader.ExchangeContext5(ref context);
		case 6:
			return CoreLoader.ExchangeContext6(ref context);
		case 7:
			return CoreLoader.ExchangeContext7(ref context);
		case 8:
			return CoreLoader.ExchangeContext8(ref context);
		case 9:
			return CoreLoader.ExchangeContext9(ref context);
		case 10:
			return CoreLoader.ExchangeContext10(ref context);
		case 11:
			return CoreLoader.ExchangeContext11(ref context);
		case 12:
			return CoreLoader.ExchangeContext12(ref context);
		case 13:
			return CoreLoader.ExchangeContext13(ref context);
		case 14:
			return CoreLoader.ExchangeContext14(ref context);
		case 15:
			return CoreLoader.ExchangeContext15(ref context);
		case 16:
			return CoreLoader.ExchangeContext16(ref context);
		case 17:
			return CoreLoader.ExchangeContext17(ref context);
		case 18:
			return CoreLoader.ExchangeContext18(ref context);
		case 19:
			return CoreLoader.ExchangeContext19(ref context);
		case 20:
			return CoreLoader.ExchangeContext20(ref context);
		case 21:
			return CoreLoader.ExchangeContext21(ref context);
		case 22:
			return CoreLoader.ExchangeContext22(ref context);
		case 23:
			return CoreLoader.ExchangeContext23(ref context);
		case 24:
			return CoreLoader.ExchangeContext24(ref context);
		case 25:
			return CoreLoader.ExchangeContext25(ref context);
		case 26:
			return CoreLoader.ExchangeContext26(ref context);
		case 27:
			return CoreLoader.ExchangeContext27(ref context);
		case 28:
			return CoreLoader.ExchangeContext28(ref context);
		case 29:
			return CoreLoader.ExchangeContext29(ref context);
		default:
			return false;
		}
	}

	private static bool ExchangeContext0(ref byte[] context)
	{
		if (context == null || context.Length < 1)
		{
			return false;
		}
		int i = 0;
		int num = context.Length - 1;
		while (i < num)
		{
			byte b = context[num];
			context[num] = context[i];
			context[i] = b;
			i++;
			num--;
		}
		return true;
	}

	private static bool ExchangeContext1(ref byte[] context)
	{
		if (context == null || context.Length < 1)
		{
			return false;
		}
		int i = 0;
		int num = context.Length - 1;
		while (i < num)
		{
			byte b = context[num];
			context[num] = context[i];
			context[i] = b;
			i++;
			num--;
		}
		return true;
	}

	private static bool ExchangeContext2(ref byte[] context)
	{
		if (context == null || context.Length < 1)
		{
			return false;
		}
		int i = 0;
		int num = context.Length - 1;
		while (i < num)
		{
			byte b = context[num];
			context[num] = context[i];
			context[i] = b;
			i++;
			num--;
		}
		return true;
	}

	private static bool ExchangeContext3(ref byte[] context)
	{
		if (context == null || context.Length < 1)
		{
			return false;
		}
		int i = 0;
		int num = context.Length - 1;
		while (i < num)
		{
			byte b = context[num];
			context[num] = context[i];
			context[i] = b;
			i++;
			num--;
		}
		return true;
	}

	private static bool ExchangeContext4(ref byte[] context)
	{
		if (context == null || context.Length < 1)
		{
			return false;
		}
		int i = 0;
		int num = context.Length - 1;
		while (i < num)
		{
			byte b = context[num];
			context[num] = context[i];
			context[i] = b;
			i++;
			num--;
		}
		return true;
	}

	private static bool ExchangeContext5(ref byte[] context)
	{
		if (context == null || context.Length < 1)
		{
			return false;
		}
		int i = 0;
		int num = context.Length - 1;
		while (i < num)
		{
			byte b = context[num];
			context[num] = context[i];
			context[i] = b;
			i++;
			num--;
		}
		return true;
	}

	private static bool ExchangeContext6(ref byte[] context)
	{
		if (context == null || context.Length < 1)
		{
			return false;
		}
		int i = 0;
		int num = context.Length - 1;
		while (i < num)
		{
			byte b = context[num];
			context[num] = context[i];
			context[i] = b;
			i++;
			num--;
		}
		return true;
	}

	private static bool ExchangeContext7(ref byte[] context)
	{
		if (context == null || context.Length < 1)
		{
			return false;
		}
		int i = 0;
		int num = context.Length - 1;
		while (i < num)
		{
			byte b = context[num];
			context[num] = context[i];
			context[i] = b;
			i++;
			num--;
		}
		return true;
	}

	private static bool ExchangeContext8(ref byte[] context)
	{
		if (context == null || context.Length < 1)
		{
			return false;
		}
		int i = 0;
		int num = context.Length - 1;
		while (i < num)
		{
			byte b = context[num];
			context[num] = context[i];
			context[i] = b;
			i++;
			num--;
		}
		return true;
	}

	private static bool ExchangeContext9(ref byte[] context)
	{
		if (context == null || context.Length < 1)
		{
			return false;
		}
		int i = 0;
		int num = context.Length - 1;
		while (i < num)
		{
			byte b = context[num];
			context[num] = context[i];
			context[i] = b;
			i++;
			num--;
		}
		return true;
	}

	private static bool ExchangeContext10(ref byte[] context)
	{
		if (context == null || context.Length < 1)
		{
			return false;
		}
		int i = 0;
		int num = context.Length - 1;
		while (i < num)
		{
			byte b = context[num];
			context[num] = context[i];
			context[i] = b;
			i++;
			num--;
		}
		return true;
	}

	private static bool ExchangeContext11(ref byte[] context)
	{
		if (context == null || context.Length < 1)
		{
			return false;
		}
		int i = 0;
		int num = context.Length - 1;
		while (i < num)
		{
			byte b = context[num];
			context[num] = context[i];
			context[i] = b;
			i++;
			num--;
		}
		return true;
	}

	private static bool ExchangeContext12(ref byte[] context)
	{
		if (context == null || context.Length < 1)
		{
			return false;
		}
		int i = 0;
		int num = context.Length - 1;
		while (i < num)
		{
			byte b = context[num];
			context[num] = context[i];
			context[i] = b;
			i++;
			num--;
		}
		return true;
	}

	private static bool ExchangeContext13(ref byte[] context)
	{
		if (context == null || context.Length < 1)
		{
			return false;
		}
		int i = 0;
		int num = context.Length - 1;
		while (i < num)
		{
			byte b = context[num];
			context[num] = context[i];
			context[i] = b;
			i++;
			num--;
		}
		return true;
	}

	private static bool ExchangeContext14(ref byte[] context)
	{
		if (context == null || context.Length < 1)
		{
			return false;
		}
		int i = 0;
		int num = context.Length - 1;
		while (i < num)
		{
			byte b = context[num];
			context[num] = context[i];
			context[i] = b;
			i++;
			num--;
		}
		return true;
	}

	private static bool ExchangeContext15(ref byte[] context)
	{
		if (context == null || context.Length < 1)
		{
			return false;
		}
		int i = 0;
		int num = context.Length - 1;
		while (i < num)
		{
			byte b = context[num];
			context[num] = context[i];
			context[i] = b;
			i++;
			num--;
		}
		return true;
	}

	private static bool ExchangeContext16(ref byte[] context)
	{
		if (context == null || context.Length < 1)
		{
			return false;
		}
		int i = 0;
		int num = context.Length - 1;
		while (i < num)
		{
			byte b = context[num];
			context[num] = context[i];
			context[i] = b;
			i++;
			num--;
		}
		return true;
	}

	private static bool ExchangeContext17(ref byte[] context)
	{
		if (context == null || context.Length < 1)
		{
			return false;
		}
		int i = 0;
		int num = context.Length - 1;
		while (i < num)
		{
			byte b = context[num];
			context[num] = context[i];
			context[i] = b;
			i++;
			num--;
		}
		return true;
	}

	private static bool ExchangeContext18(ref byte[] context)
	{
		if (context == null || context.Length < 1)
		{
			return false;
		}
		int i = 0;
		int num = context.Length - 1;
		while (i < num)
		{
			byte b = context[num];
			context[num] = context[i];
			context[i] = b;
			i++;
			num--;
		}
		return true;
	}

	private static bool ExchangeContext19(ref byte[] context)
	{
		if (context == null || context.Length < 1)
		{
			return false;
		}
		int i = 0;
		int num = context.Length - 1;
		while (i < num)
		{
			byte b = context[num];
			context[num] = context[i];
			context[i] = b;
			i++;
			num--;
		}
		return true;
	}

	private static bool ExchangeContext20(ref byte[] context)
	{
		if (context == null || context.Length < 1)
		{
			return false;
		}
		int i = 0;
		int num = context.Length - 1;
		while (i < num)
		{
			byte b = context[num];
			context[num] = context[i];
			context[i] = b;
			i++;
			num--;
		}
		return true;
	}

	private static bool ExchangeContext21(ref byte[] context)
	{
		if (context == null || context.Length < 1)
		{
			return false;
		}
		int i = 0;
		int num = context.Length - 1;
		while (i < num)
		{
			byte b = context[num];
			context[num] = context[i];
			context[i] = b;
			i++;
			num--;
		}
		return true;
	}

	private static bool ExchangeContext22(ref byte[] context)
	{
		if (context == null || context.Length < 1)
		{
			return false;
		}
		int i = 0;
		int num = context.Length - 1;
		while (i < num)
		{
			byte b = context[num];
			context[num] = context[i];
			context[i] = b;
			i++;
			num--;
		}
		return true;
	}

	private static bool ExchangeContext23(ref byte[] context)
	{
		if (context == null || context.Length < 1)
		{
			return false;
		}
		int i = 0;
		int num = context.Length - 1;
		while (i < num)
		{
			byte b = context[num];
			context[num] = context[i];
			context[i] = b;
			i++;
			num--;
		}
		return true;
	}

	private static bool ExchangeContext24(ref byte[] context)
	{
		if (context == null || context.Length < 1)
		{
			return false;
		}
		int i = 0;
		int num = context.Length - 1;
		while (i < num)
		{
			byte b = context[num];
			context[num] = context[i];
			context[i] = b;
			i++;
			num--;
		}
		return true;
	}

	private static bool ExchangeContext25(ref byte[] context)
	{
		if (context == null || context.Length < 1)
		{
			return false;
		}
		int i = 0;
		int num = context.Length - 1;
		while (i < num)
		{
			byte b = context[num];
			context[num] = context[i];
			context[i] = b;
			i++;
			num--;
		}
		return true;
	}

	private static bool ExchangeContext26(ref byte[] context)
	{
		if (context == null || context.Length < 1)
		{
			return false;
		}
		int i = 0;
		int num = context.Length - 1;
		while (i < num)
		{
			byte b = context[num];
			context[num] = context[i];
			context[i] = b;
			i++;
			num--;
		}
		return true;
	}

	private static bool ExchangeContext27(ref byte[] context)
	{
		if (context == null || context.Length < 1)
		{
			return false;
		}
		int i = 0;
		int num = context.Length - 1;
		while (i < num)
		{
			byte b = context[num];
			context[num] = context[i];
			context[i] = b;
			i++;
			num--;
		}
		return true;
	}

	private static bool ExchangeContext28(ref byte[] context)
	{
		if (context == null || context.Length < 1)
		{
			return false;
		}
		int i = 0;
		int num = context.Length - 1;
		while (i < num)
		{
			byte b = context[num];
			context[num] = context[i];
			context[i] = b;
			i++;
			num--;
		}
		return true;
	}

	private static bool ExchangeContext29(ref byte[] context)
	{
		if (context == null || context.Length < 1)
		{
			return false;
		}
		int i = 0;
		int num = context.Length - 1;
		while (i < num)
		{
			byte b = context[num];
			context[num] = context[i];
			context[i] = b;
			i++;
			num--;
		}
		return true;
	}

	private static string GetMd5(byte[] bytes)
	{
		MD5 mD = MD5.Create();
		byte[] array = mD.ComputeHash(bytes);
		string text = string.Empty;
		for (int i = 0; i < array.Length; i++)
		{
			text += array[i].ToString("X");
		}
		return text;
	}

	private static byte[] GetMd5Bytes(byte[] bytes)
	{
		MD5 mD = MD5.Create();
		return mD.ComputeHash(bytes);
	}
}
