/* =============================================
 * Copyright 2021 TVU Networks Co.,Ltd. All rights reserved.
 * For internal members in TVU Networks only.
 * FileName: Scte104Writer.cs
 * Purpose:
 * Author:   YulongLu added on 12.6th, 2021.
 * Since:    Microsoft Visual Studio 2019
 * =============================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scte104Parser.Scte104Messages
{
    public class Scte104Writer
    {
        #region Property & Fields

        private object _lock_sct104Bytes = new object();

        private readonly List<byte> _scte104Bytes;

        public int Length => _scte104Bytes.Count;

        #endregion Property & Fields

        #region .ctor

        public Scte104Writer() : this(byte.MaxValue)
        {
        }

        public Scte104Writer(int capcity)
        {
            _scte104Bytes = new List<byte>(capcity);
        }

        #endregion .ctor

        #region Static methods

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

        #endregion Private methods

        #region Methods

        public Range Write(byte data)
            => WriteByte(data);

        public Range WriteByte(byte data)
        {
            lock (_lock_sct104Bytes)
            {
                int start = Length;
                _scte104Bytes.Add(data);
                return start..Length;
            }
        }

        public Range Write(IEnumerable<byte> data)
            => WriteBytes(data);

        public Range WriteBytes(IEnumerable<byte> data)
        {
            int start = Length;
            if (data?.Any() == true)
            {
                lock (_lock_sct104Bytes)
                {
                    _scte104Bytes.AddRange(data);
                }
            }
            return start..Length;
        }

        public Range Write(ushort data)
            => WriteUInt16(data);

        public Range WriteUInt16(ushort data)
        {
            lock (_lock_sct104Bytes)
            {
                int start = Length;
                var byteData = (byte)(data >> 8);
                _scte104Bytes.Add(byteData);
                byteData = (byte)(data & 0xff);
                _scte104Bytes.Add(byteData);
                return start..Length;
            }
        }

        public Range Write(uint data)
            => WriteUInt32(data);

        public Range WriteUInt32(uint data)
        {
            lock (_lock_sct104Bytes)
            {
                int start = Length;
                var byteData = (byte)((data >> 24) & 0xff);
                _scte104Bytes.Add(byteData);
                byteData = (byte)((data >> 16) & 0xff);
                _scte104Bytes.Add(byteData);
                byteData = (byte)((data >> 8) & 0xff);
                _scte104Bytes.Add(byteData);
                byteData = (byte)(data & 0xff);
                _scte104Bytes.Add(byteData);
                return start..Length;
            }
        }

        public Range Write(Scte104Timestamp data)
            => WriteTimestamp(data);

        public Range WriteTimestamp(Scte104Timestamp timestamp)
        {
            int start = Length;
            if (timestamp != null)
            {
                lock (_lock_sct104Bytes)
                {
                    switch (timestamp.TimeType)
                    {
                        case 1:
                        case 2:
                        case 3:
                            if (timestamp.RawData?.Any() == true)
                            {
                                _scte104Bytes.Add(timestamp.TimeType);
                                _scte104Bytes.AddRange(timestamp.RawData);
                            }
                            else
                                _scte104Bytes.Add(0);
                            break;

                        default:
                            _scte104Bytes.Add(0);
                            break;
                    }
                }
            }
            return start..Length;
        }

        public void Replace(Index index, byte data)
        {
            lock (_lock_sct104Bytes)
            {
                _scte104Bytes[index] = data;
            }
        }

        public void Replace(Index index, ushort data)
        {
            lock (_lock_sct104Bytes)
            {
                var byteData = (byte)(data >> 8);
                _scte104Bytes[index] = byteData;
                byteData = (byte)(data & 0xff);
                _scte104Bytes[index.Value + 1] = byteData;
            }
        }

        public string ToHex()
        {
            StringBuilder sb = new StringBuilder();
            lock (_lock_sct104Bytes)
            {
                foreach (var item in _scte104Bytes)
                {
                    sb.Append(item.ToString("X2"));
                }
            }
            return sb.ToString();
        }

        #endregion Methods
    }
}