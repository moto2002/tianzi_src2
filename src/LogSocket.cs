using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class LogSocket
{
	private class EncoderStruct
	{
		private class StructType
		{
			public const int TYPE_INT = 4;

			public const int TYPE_STRING = 1;
		}

		private List<byte> m_bytes = new List<byte>();

		public byte[] Bytes
		{
			get
			{
				return this.m_bytes.ToArray();
			}
		}

		public void AddString(string value)
		{
			int byteCount = Encoding.UTF8.GetByteCount(value);
			byte[] bytes = Encoding.UTF8.GetBytes(value);
			this.AddInt(byteCount);
			this.m_bytes.AddRange(bytes);
		}

		public void AddInt(int value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			this.m_bytes.AddRange(bytes);
		}
	}

	private const string LOGSOCKET_ADDRESS = "LogSocket_Address";

	private const string LOGSOCKET_PORT = "LogSocket_Port";

	private const string LOGSOCKET_EXECUTE = "LogSocket_Execute";

	private static Socket clientSocket;

	private static string m_address;

	private static bool m_isExecute;

	private static bool isExecute
	{
		get
		{
			return LogSocket.m_isExecute;
		}
	}

	public static void SetRemoteLogStatus(int status)
	{
		PlayerPrefs.SetInt("LogSocket_Execute", status);
	}

	public static void AddConncetInfo(Dictionary<string, string> dic)
	{
		string text = string.Empty;
		int port = 0;
		IPAddress currentIPV = LogSocket.GetCurrentIPV4();
		if (currentIPV != null)
		{
			foreach (KeyValuePair<string, string> current in dic)
			{
				if (!string.IsNullOrEmpty(current.Key) && !string.IsNullOrEmpty(current.Value) && current.Key == currentIPV.ToString())
				{
					string[] array = current.Value.Split(new char[]
					{
						':'
					}, StringSplitOptions.RemoveEmptyEntries);
					if (array.Length == 2)
					{
						text = array[0];
						port = AssetFileUtils.IntParse(array[1], 0);
					}
				}
			}
		}
		if (string.IsNullOrEmpty(text) && dic.ContainsKey("*"))
		{
			string text2 = dic["*"];
			string[] array2 = text2.Split(new char[]
			{
				':'
			}, StringSplitOptions.RemoveEmptyEntries);
			if (array2.Length == 2)
			{
				text = array2[0];
				port = AssetFileUtils.IntParse(array2[1], 0);
			}
		}
		LogSocket.SetAddressPort(text, port);
	}

	public static void Init()
	{
		int @int = PlayerPrefs.GetInt("LogSocket_Execute");
		LogSocket.m_isExecute = (@int == 1);
		LogSystem.LogWarning(new object[]
		{
			"RemoteLog Status : ",
			LogSocket.isExecute
		});
		if (LogSocket.isExecute)
		{
			LogSocket.m_address = PlayerPrefs.GetString("LogSocket_Address");
			int int2 = PlayerPrefs.GetInt("LogSocket_Port");
			LogSystem.LogWarning(new object[]
			{
				"RemoteLog Address : ",
				LogSocket.m_address,
				" : ",
				int2
			});
			if (!string.IsNullOrEmpty(LogSocket.m_address) && int2 != 0)
			{
				LogSocket.Connect(LogSocket.m_address, int2);
			}
		}
	}

	private static void SetAddressPort(string address, int port)
	{
		PlayerPrefs.SetString("LogSocket_Address", address);
		PlayerPrefs.SetInt("LogSocket_Port", port);
	}

	private static void ClearPrefs()
	{
		if (LogSocket.isExecute)
		{
			LogSocket.SetAddressPort(string.Empty, 0);
			LogSocket.SetRemoteLogStatus(0);
		}
	}

	private static IPAddress GetCurrentIPV4()
	{
		string hostName = Dns.GetHostName();
		if (!string.IsNullOrEmpty(hostName))
		{
			IPAddress[] hostAddresses = Dns.GetHostAddresses(hostName);
			IPAddress[] array = hostAddresses;
			for (int i = 0; i < array.Length; i++)
			{
				IPAddress iPAddress = array[i];
				if (iPAddress.AddressFamily == AddressFamily.InterNetwork)
				{
					return iPAddress;
				}
			}
		}
		return null;
	}

	private static void Connect(string address, int port)
	{
		if (LogSocket.isExecute)
		{
			try
			{
				IPAddress iPAddress = IPAddress.Parse(address);
				if (iPAddress != null)
				{
					LogSocket.clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
					LogSocket.clientSocket.Connect(new IPEndPoint(iPAddress, port));
				}
			}
			catch (SocketException ex)
			{
				LogSystem.LogWarning(new object[]
				{
					"RemoteLog Connect SocketException : ",
					ex.ToString()
				});
			}
		}
	}

	public static void Send(int type, string time, string value)
	{
		if (LogSocket.isExecute)
		{
			LogSocket.EncoderStruct encoderStruct = new LogSocket.EncoderStruct();
			encoderStruct.AddInt(type);
			encoderStruct.AddString(time);
			encoderStruct.AddString(value);
			if (LogSocket.clientSocket != null && LogSocket.clientSocket.Connected)
			{
				LogSocket.clientSocket.Send(encoderStruct.Bytes);
			}
		}
	}

	public static void Close()
	{
		if (LogSocket.isExecute && LogSocket.clientSocket != null)
		{
			LogSocket.clientSocket.Shutdown(SocketShutdown.Both);
			LogSocket.clientSocket.Close();
		}
	}

	public static void Trace()
	{
	}
}
