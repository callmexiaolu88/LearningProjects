/* =============================================
 * Copyright 2021 TVU Networks Co.,Ltd. All rights reserved.
 * For internal members in TVU Networks only.
 * FileName: TimeSignalRequestOperation.cs
 * Purpose:
 * Author:   YulongLu added on 12.7th, 2021.
 * Since:    Microsoft Visual Studio 2019
 * =============================================*/

namespace Scte104Parser.Scte104Messages.Operations
{
    internal class TimeSignalRequestOperation : Scte104MessageOperation
    {
        public ushort PrerollTime { get; set; }

        public TimeSignalRequestOperation() : base(EnumOperationID.TimeSignalRequestData)
        {
        }

        public override void Decode(Scte104Reader reader)
        {
            PrerollTime = reader.ReadUInt16();
        }

        public override void Encode(Scte104Writer writer)
        {
            writer.Write(PrerollTime);
        }
    }
}