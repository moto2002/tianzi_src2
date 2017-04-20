using System;
using System.Collections.Generic;
using System.Text;

public class EncoderStruct
{
	private List<byte> m_bytes = new List<byte>();

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

	public void AddFloat(float value)
	{
	}
}
