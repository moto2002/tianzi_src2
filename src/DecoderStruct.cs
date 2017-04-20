using System;
using System.Text;

public class DecoderStruct
{
	private int m_position;

	private int m_length;

	private byte[] m_bytes;

	public DecoderStruct(byte[] bytes)
	{
		this.m_bytes = bytes;
		this.m_length = this.m_bytes.Length;
		this.m_position = 0;
	}

	public string GetString()
	{
		int @int = this.GetInt();
		if (this.m_length < @int)
		{
			Console.WriteLine("Struct GetString Failure");
			return string.Empty;
		}
		byte[] array = new byte[@int];
		Buffer.BlockCopy(this.m_bytes, this.m_position, array, 0, @int);
		string @string = Encoding.UTF8.GetString(array);
		this.m_position += @int;
		this.m_length -= @int;
		return @string;
	}

	public int GetInt()
	{
		if (this.m_length < 4)
		{
			LogSystem.LogError(new object[]
			{
				"DecoderStruct::GetInt() ",
				this.m_length
			});
			return 0;
		}
		byte[] dst = new byte[4];
		Buffer.BlockCopy(this.m_bytes, this.m_position, dst, 0, 4);
		this.m_position += 4;
		this.m_length -= 4;
		return 0;
	}
}
