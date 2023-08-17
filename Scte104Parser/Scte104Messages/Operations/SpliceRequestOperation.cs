/* =============================================
 * Copyright 2021 TVU Networks Co.,Ltd. All rights reserved.
 * For internal members in TVU Networks only.
 * FileName: SpliceRequestOperation.cs
 * Purpose:
 * Author:   YulongLu added on 12.7th, 2021.
 * Since:    Microsoft Visual Studio 2019
 * =============================================*/

using Newtonsoft.Json;

namespace Scte104Parser.Scte104Messages.Operations
{
    internal class SpliceRequestOperation : Scte104MessageOperation
    {
        public EnumSpliceInsertType SpliceInsertType { get; set; }
        public uint SpliceEventID { get; set; }
        public ushort UniqueProgramID { get; set; }
        public ushort PrerollTime { get; set; }
        public ushort BreakDuration { get; set; }
        public byte AvailNum { get; set; }
        public byte AvailsExpected { get; set; }
        public byte AutoReturnFlag { get; set; }

        public SpliceRequestOperation() : base(EnumOperationID.SpliceRequestData)
        {
        }

        public override void Decode(Scte104Reader reader)
        {
            SpliceInsertType = (EnumSpliceInsertType)reader.ReadByte();
            SpliceEventID = reader.ReadUInt32();
            UniqueProgramID = reader.ReadUInt16();
            PrerollTime = reader.ReadUInt16();
            BreakDuration = reader.ReadUInt16();
            AvailNum = reader.ReadByte();
            AvailsExpected = reader.ReadByte();
            AutoReturnFlag = reader.ReadByte();
        }

        public override void Encode(Scte104Writer writer)
        {
            writer.Write((byte)SpliceInsertType);
            writer.Write(SpliceEventID);
            writer.Write(UniqueProgramID);
            writer.Write(PrerollTime);
            writer.Write(BreakDuration);
            writer.Write(AvailNum);
            writer.Write(AvailsExpected);
            writer.Write(AutoReturnFlag);
        }

        public override Scte104OperationSummary GetSummary()
            => new Scte104OperationSummary
            {
                OperationID = OperationID,
                ExtraID = (ushort)SpliceInsertType,
                ExtraData = JsonConvert.SerializeObject(new
                {
                    duration = calculateDuration(),
                })
            };

        private long calculateDuration()
        {
            long duration = 0;
            switch (SpliceInsertType)
            {
                case EnumSpliceInsertType.SplicestartNormal:
                case EnumSpliceInsertType.SplicestartImmediate:
                    duration = BreakDuration * 100; // millisecond
                    break;
            }
            return duration;
        }
    }
}