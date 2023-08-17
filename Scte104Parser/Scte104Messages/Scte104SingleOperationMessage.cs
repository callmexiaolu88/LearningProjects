/* =============================================
 * Copyright 2021 TVU Networks Co.,Ltd. All rights reserved.
 * For internal members in TVU Networks only.
 * FileName: Scte104SingleOperationMessage.cs
 * Purpose:
 * Author:   YulongLu added on 12.6th, 2021.
 * Since:    Microsoft Visual Studio 2019
 * =============================================*/

using System;
using System.Collections.Generic;

namespace Scte104Parser.Scte104Messages
{
    internal class Scte104SingleOperationMessage : Scte104Message
    {
        public override Scte104MessageType Type => Scte104MessageType.SingleOperation;

        public ushort OperationID { get; set; }
        public ushort MessageSize { get; set; }
        public ushort Result { get; set; }
        public ushort ResultExtension { get; set; }
        public byte ProtocolVersion { get; set; }
        public byte AS_Index { get; set; }
        public byte MessageNumber { get; set; }
        public ushort DPI_PID_Index { get; set; }
        public byte[] Data { get; set; }

        public override void Decode(Scte104Reader reader)
        {
            OperationID = reader.ReadUInt16();
            MessageSize = reader.ReadUInt16();
            Result = reader.ReadUInt16();
            ResultExtension = reader.ReadUInt16();
            ProtocolVersion = reader.ReadByte();
            AS_Index = reader.ReadByte();
            MessageNumber = reader.ReadByte();
            DPI_PID_Index = reader.ReadUInt16();
            Data = reader.ReadBytes(MessageSize - 13);
        }

        public override void Encode(Scte104Writer writer)
        {
            writer.WriteUInt16(OperationID);
            writer.WriteUInt16(MessageSize);
            writer.WriteUInt16(Result);
            writer.WriteUInt16(ResultExtension);
            writer.WriteByte(ProtocolVersion);
            writer.WriteByte(AS_Index);
            writer.WriteByte(MessageNumber);
            writer.WriteUInt16(DPI_PID_Index);
            writer.WriteBytes(Data);
        }

        public override IEnumerable<Scte104OperationSummary> GetOperationSummaries()
            => new[] { new Scte104OperationSummary { OperationID = (EnumOperationID)OperationID } };

        public override void Validate()
        {
            base.Validate();
            if (!((OperationID >= 0x0000 && OperationID <= 0x0012) || (OperationID >= 0x8000 && OperationID <= 0xBFFF)))
            {
                throw new Exception($"OperationID [{OperationID:X4}] is not available.");
            }
        }
    }
}