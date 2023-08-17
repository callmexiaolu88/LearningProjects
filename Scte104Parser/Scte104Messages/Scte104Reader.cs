/* =============================================
 * Copyright 2021 TVU Networks Co.,Ltd. All rights reserved.
 * For internal members in TVU Networks only.
 * FileName: Scte104Reader.cs
 * Purpose:
 * Author:   YulongLu added on 12.6th, 2021.
 * Since:    Microsoft Visual Studio 2019
 * =============================================*/

using System;
using System.Linq;
using System.Text;

namespace Scte104Parser.Scte104Messages
{
    public class Scte104Reader
    {
        #region Property & Fields

        private int _index;

        private readonly byte[] _scte104Bytes;

        private object _lock_sct104Bytes = new object();

        public byte[] this[Range range] => _scte104Bytes[range];

        public byte this[int index] => _scte104Bytes[index];

        public int Length => _scte104Bytes.Length;

        #endregion Property & Fields

        #region .ctor

        private Scte104Reader(byte[] bytes) : this(bytes, 0)
        {
        }

        private Scte104Reader(byte[] bytes, int index)
        {
            if (bytes == null)
                throw new ArgumentNullException($"scte104 bytes is null.");
            _scte104Bytes = bytes;
            _index = index;
        }

        #endregion .ctor

        #region Static methods

        public static Scte104Reader CreateBy(string scte104Hex)
        {
            if (CheckScteType(scte104Hex) != EnumScteType.Scte104)
                throw new Exception("Scte hex string is not scte104");
            return new Scte104Reader(StringToBytes(scte104Hex));
        }

        public static EnumScteType CheckScteType(string scteHex)
        {
            if (!string.IsNullOrWhiteSpace(scteHex))
            {
                var upperScteHex = scteHex.ToUpper();
                if (upperScteHex.Length >= 2 && upperScteHex.StartsWith("FC"))
                    return EnumScteType.Scte35;
                if (upperScteHex.Length >= 4 && upperScteHex.StartsWith("FFFF"))
                    return EnumScteType.Scte104;
            }
            return EnumScteType.Unknow;
        }

        public static byte[] StringToBytes(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                throw new Exception($"Incorrect scte104 hex string.");
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        #endregion Static methods

        #region Private methods

        private int getTimestampLength(int timeType)
            => timeType switch
            {
                0 => 0,
                1 => 6, //UTC_seconds(4) UTC_microseconds(2)
                2 => 4, //hours(1) minutes(1) seconds(1) frames(1)
                3 => 2, //GPI_number(1) GPI_edge(1)
                _ => throw new Exception($"Incorrect scte104 time type: [{timeType}]")
            };

        private byte[] toBigEndian(byte[] bytes)
        {
            if (BitConverter.IsLittleEndian && bytes.Length > 1 && (bytes.Length % 2) == 0)
            {
                var convertBytes = new byte[bytes.Length];
                for (int i = 0; i < bytes.Length; i += 2)
                {
                    convertBytes[i] = bytes[i + 1];
                    convertBytes[i + 1] = bytes[i];
                }
                return convertBytes;
            }
            else
            {
                return bytes;
            }
        }

        private byte[] retriveBytes(int count, bool peekData = false)
        {
            byte[] result = null;
            if (count > 0)
            {
                lock (_lock_sct104Bytes)
                {
                    result = _scte104Bytes[_index..(_index + count)];
                    if (!peekData)
                        _index += count;
                }
            }
            else
            {
                result = new byte[0];
            }
            return result;
        }

        #endregion Private methods

        #region Methods

        public Scte104Reader Scope(Range range)
            => new Scte104Reader(_scte104Bytes[range]);

        public Scte104Reader Scope(int count)
            => new Scte104Reader(count > 0 ? _scte104Bytes[_index..(_index + count)] : new byte[0]);

        public byte PeekByte()
            => retriveBytes(1, true).First();

        public byte[] PeekBytes(int count)
            => retriveBytes(count, true);

        public byte[] PeekBytesWithEndian(int count)
            => toBigEndian(PeekBytes(count));

        public ushort PeekUInt16()
            => BitConverter.ToUInt16(PeekBytesWithEndian(2), 0);

        public byte ReadByte()
            => retriveBytes(1).First();

        public byte[] ReadBytes(int count)
            => retriveBytes(count);

        public Scte104Reader ReadRawInfo(int count)
            => new Scte104Reader(ReadBytes(count));

        public byte[] ReadBytesWithEndian(int count)
            => toBigEndian(ReadBytes(count));

        public ushort ReadUInt16()
            => BitConverter.ToUInt16(ReadBytesWithEndian(2), 0);

        public uint ReadUInt32()
            => BitConverter.ToUInt32(ReadBytesWithEndian(4), 0);

        public Scte104Timestamp ReadTimestamp()
        {
            var timeType = ReadByte();
            var length = getTimestampLength(timeType);
            return new Scte104Timestamp
            {
                TimeType = timeType,
                RawData = ReadBytes(length)
            };
        }

        public string ToHex()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in _scte104Bytes)
            {
                sb.Append(item.ToString("X2"));
            }
            return sb.ToString();
        }

        #endregion Methods
    }

    public enum EnumScteType
    {
        Unknow = 0,
        Scte35,
        Scte104
    }
}