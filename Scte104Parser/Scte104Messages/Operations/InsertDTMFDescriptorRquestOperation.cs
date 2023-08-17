/* =============================================
 * Copyright 2021 TVU Networks Co.,Ltd. All rights reserved.
 * For internal members in TVU Networks only.
 * FileName: InsertDTMFDescriptorRquestOperation.cs
 * Purpose:
 * Author:   YulongLu added on 12.7th, 2021.
 * Since:    Microsoft Visual Studio 2019
 * =============================================*/

namespace Scte104Parser.Scte104Messages.Operations
{
    internal class InsertDTMFDescriptorRquestOperation : Scte104MessageOperation
    {
        public byte PrerollTime { get; set; }
        public byte DTMFLength { get; set; }
        public byte[] DTMF { get; set; }

        public InsertDTMFDescriptorRquestOperation() : base(EnumOperationID.InsertDTMFDescriptor)
        {
        }

        public override void Decode(Scte104Reader reader)
        {
            PrerollTime = reader.ReadByte();
            DTMFLength = reader.ReadByte();
            DTMF = reader.ReadBytes(DTMFLength);
        }

        public override void Encode(Scte104Writer writer)
        {
            writer.Write(PrerollTime);
            writer.Write((byte)DTMF.Length);
            writer.Write(DTMF);
        }
    }
}