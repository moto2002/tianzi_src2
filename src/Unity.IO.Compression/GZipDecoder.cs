using System;

namespace Unity.IO.Compression
{
	internal class GZipDecoder : IFileFormatReader
	{
		internal enum GzipHeaderState
		{
			ReadingID1,
			ReadingID2,
			ReadingCM,
			ReadingFLG,
			ReadingMMTime,
			ReadingXFL,
			ReadingOS,
			ReadingXLen1,
			ReadingXLen2,
			ReadingXLenData,
			ReadingFileName,
			ReadingComment,
			ReadingCRC16Part1,
			ReadingCRC16Part2,
			Done,
			ReadingCRC,
			ReadingFileSize
		}

		[Flags]
		internal enum GZipOptionalHeaderFlags
		{
			CRCFlag = 2,
			ExtraFieldsFlag = 4,
			FileNameFlag = 8,
			CommentFlag = 16
		}

		private GZipDecoder.GzipHeaderState gzipHeaderSubstate;

		private GZipDecoder.GzipHeaderState gzipFooterSubstate;

		private int gzip_header_flag;

		private int gzip_header_xlen;

		private uint expectedCrc32;

		private uint expectedOutputStreamSizeModulo;

		private int loopCounter;

		private uint actualCrc32;

		private long actualStreamSizeModulo;

		public GZipDecoder()
		{
			this.Reset();
		}

		public void Reset()
		{
			this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.ReadingID1;
			this.gzipFooterSubstate = GZipDecoder.GzipHeaderState.ReadingCRC;
			this.expectedCrc32 = 0u;
			this.expectedOutputStreamSizeModulo = 0u;
		}

		public bool ReadHeader(InputBuffer input)
		{
			int bits;
			switch (this.gzipHeaderSubstate)
			{
			case GZipDecoder.GzipHeaderState.ReadingID1:
				bits = input.GetBits(8);
				if (bits < 0)
				{
					return false;
				}
				if (bits != 31)
				{
					throw new InvalidDataException(SR.GetString("Corrupted gzip header"));
				}
				this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.ReadingID2;
				break;
			case GZipDecoder.GzipHeaderState.ReadingID2:
				break;
			case GZipDecoder.GzipHeaderState.ReadingCM:
				goto IL_BB;
			case GZipDecoder.GzipHeaderState.ReadingFLG:
				goto IL_EF;
			case GZipDecoder.GzipHeaderState.ReadingMMTime:
				goto IL_11A;
			case GZipDecoder.GzipHeaderState.ReadingXFL:
				goto IL_15F;
			case GZipDecoder.GzipHeaderState.ReadingOS:
				goto IL_17C;
			case GZipDecoder.GzipHeaderState.ReadingXLen1:
				goto IL_199;
			case GZipDecoder.GzipHeaderState.ReadingXLen2:
				goto IL_1CF;
			case GZipDecoder.GzipHeaderState.ReadingXLenData:
				goto IL_204;
			case GZipDecoder.GzipHeaderState.ReadingFileName:
				goto IL_24F;
			case GZipDecoder.GzipHeaderState.ReadingComment:
				goto IL_297;
			case GZipDecoder.GzipHeaderState.ReadingCRC16Part1:
				goto IL_2E0;
			case GZipDecoder.GzipHeaderState.ReadingCRC16Part2:
				goto IL_318;
			case GZipDecoder.GzipHeaderState.Done:
				return true;
			default:
				throw new InvalidDataException(SR.GetString("Unknown state"));
			}
			bits = input.GetBits(8);
			if (bits < 0)
			{
				return false;
			}
			if (bits != 139)
			{
				throw new InvalidDataException(SR.GetString("Corrupted gzip header"));
			}
			this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.ReadingCM;
			IL_BB:
			bits = input.GetBits(8);
			if (bits < 0)
			{
				return false;
			}
			if (bits != 8)
			{
				throw new InvalidDataException(SR.GetString("Unknown compression mode"));
			}
			this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.ReadingFLG;
			IL_EF:
			bits = input.GetBits(8);
			if (bits < 0)
			{
				return false;
			}
			this.gzip_header_flag = bits;
			this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.ReadingMMTime;
			this.loopCounter = 0;
			IL_11A:
			while (this.loopCounter < 4)
			{
				bits = input.GetBits(8);
				if (bits < 0)
				{
					return false;
				}
				this.loopCounter++;
			}
			this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.ReadingXFL;
			this.loopCounter = 0;
			IL_15F:
			bits = input.GetBits(8);
			if (bits < 0)
			{
				return false;
			}
			this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.ReadingOS;
			IL_17C:
			bits = input.GetBits(8);
			if (bits < 0)
			{
				return false;
			}
			this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.ReadingXLen1;
			IL_199:
			if ((this.gzip_header_flag & 4) == 0)
			{
				goto IL_24F;
			}
			bits = input.GetBits(8);
			if (bits < 0)
			{
				return false;
			}
			this.gzip_header_xlen = bits;
			this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.ReadingXLen2;
			IL_1CF:
			bits = input.GetBits(8);
			if (bits < 0)
			{
				return false;
			}
			this.gzip_header_xlen |= bits << 8;
			this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.ReadingXLenData;
			this.loopCounter = 0;
			IL_204:
			while (this.loopCounter < this.gzip_header_xlen)
			{
				bits = input.GetBits(8);
				if (bits < 0)
				{
					return false;
				}
				this.loopCounter++;
			}
			this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.ReadingFileName;
			this.loopCounter = 0;
			IL_24F:
			if ((this.gzip_header_flag & 8) == 0)
			{
				this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.ReadingComment;
			}
			else
			{
				while (true)
				{
					bits = input.GetBits(8);
					if (bits < 0)
					{
						break;
					}
					if (bits == 0)
					{
						goto Block_20;
					}
				}
				return false;
				Block_20:
				this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.ReadingComment;
			}
			IL_297:
			if ((this.gzip_header_flag & 16) == 0)
			{
				this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.ReadingCRC16Part1;
			}
			else
			{
				while (true)
				{
					bits = input.GetBits(8);
					if (bits < 0)
					{
						break;
					}
					if (bits == 0)
					{
						goto Block_23;
					}
				}
				return false;
				Block_23:
				this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.ReadingCRC16Part1;
			}
			IL_2E0:
			if ((this.gzip_header_flag & 2) == 0)
			{
				this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.Done;
				return true;
			}
			bits = input.GetBits(8);
			if (bits < 0)
			{
				return false;
			}
			this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.ReadingCRC16Part2;
			IL_318:
			bits = input.GetBits(8);
			if (bits < 0)
			{
				return false;
			}
			this.gzipHeaderSubstate = GZipDecoder.GzipHeaderState.Done;
			return true;
		}

		public bool ReadFooter(InputBuffer input)
		{
			input.SkipToByteBoundary();
			if (this.gzipFooterSubstate == GZipDecoder.GzipHeaderState.ReadingCRC)
			{
				while (this.loopCounter < 4)
				{
					int bits = input.GetBits(8);
					if (bits < 0)
					{
						return false;
					}
					this.expectedCrc32 |= (uint)((uint)bits << 8 * this.loopCounter);
					this.loopCounter++;
				}
				this.gzipFooterSubstate = GZipDecoder.GzipHeaderState.ReadingFileSize;
				this.loopCounter = 0;
			}
			if (this.gzipFooterSubstate == GZipDecoder.GzipHeaderState.ReadingFileSize)
			{
				if (this.loopCounter == 0)
				{
					this.expectedOutputStreamSizeModulo = 0u;
				}
				while (this.loopCounter < 4)
				{
					int bits2 = input.GetBits(8);
					if (bits2 < 0)
					{
						return false;
					}
					this.expectedOutputStreamSizeModulo |= (uint)((uint)bits2 << 8 * this.loopCounter);
					this.loopCounter++;
				}
			}
			return true;
		}

		public void UpdateWithBytesRead(byte[] buffer, int offset, int copied)
		{
			this.actualCrc32 = Crc32Helper.UpdateCrc32(this.actualCrc32, buffer, offset, copied);
			long num = this.actualStreamSizeModulo + (long)((ulong)copied);
			if (num >= 4294967296L)
			{
				num %= 4294967296L;
			}
			this.actualStreamSizeModulo = num;
		}

		public void Validate()
		{
			if (this.expectedCrc32 != this.actualCrc32)
			{
				throw new InvalidDataException(SR.GetString("Invalid CRC"));
			}
			if (this.actualStreamSizeModulo != (long)((ulong)this.expectedOutputStreamSizeModulo))
			{
				throw new InvalidDataException(SR.GetString("Invalid stream size"));
			}
		}
	}
}
