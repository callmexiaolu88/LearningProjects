/* =============================================
 * Copyright 2021 TVU Networks Co.,Ltd. All rights reserved.
 * For internal members in TVU Networks only.
 * FileName: InsertSegmentationDescriptorRequestOperation.cs
 * Purpose:
 * Author:   YulongLu added on 12.7th, 2021.
 * Since:    Microsoft Visual Studio 2019
 * =============================================*/

using System;
using Newtonsoft.Json;

namespace Scte104Parser.Scte104Messages.Operations
{
    internal class InsertSegmentationDescriptorRequestOperation : Scte104MessageOperation
    {
        public uint SegmentationEventID { get; set; }
        public byte SegmentationEventCancelIndicator { get; set; }
        public ushort Duration { get; set; }
        public EnumSegmentationUpidType SegmentationUpidType { get; set; }
        public byte SegmentationUpidLength { get; set; }
        public byte[] SegmentationUpid { get; set; }
        public EnumSegmentationTypeID SegmentationTypeId { get; set; }
        public byte SegmentNum { get; set; }
        public byte SegmentsExpected { get; set; }
        public byte DurationExtensionFrames { get; set; }
        public byte DeliveryNotRestrictedFlag { get; set; }
        public byte WebDeliveryAllowedFlag { get; set; }
        public byte NoRegionalBlackoutFlag { get; set; }
        public byte ArchiveAllowedFlag { get; set; }
        public byte DeviceRestrictions { get; set; }

        #region Optional

        public byte InsertSubSegmentInfo { get; set; }
        public byte SubSegmentNum { get; set; }
        public byte SubSegmentsExpected { get; set; }

        #endregion Optional

        public InsertSegmentationDescriptorRequestOperation() : base(EnumOperationID.InsertSegmentationDescriptor)
        {
            ArchiveAllowedFlag = 1;
            WebDeliveryAllowedFlag = 1;
            SegmentationUpid = new byte[0];
        }

        public override void Decode(Scte104Reader reader)
        {
            SegmentationEventID = reader.ReadUInt32();
            SegmentationEventCancelIndicator = reader.ReadByte();
            Duration = reader.ReadUInt16();
            SegmentationUpidType = (EnumSegmentationUpidType)reader.ReadByte();
            SegmentationUpidLength = reader.ReadByte();
            SegmentationUpid = reader.ReadBytes(SegmentationUpidLength);
            SegmentationTypeId = (EnumSegmentationTypeID)reader.ReadByte();
            SegmentNum = reader.ReadByte();
            SegmentsExpected = reader.ReadByte();
            DurationExtensionFrames = reader.ReadByte();
            DeliveryNotRestrictedFlag = reader.ReadByte();
            WebDeliveryAllowedFlag = reader.ReadByte();
            NoRegionalBlackoutFlag = reader.ReadByte();
            ArchiveAllowedFlag = reader.ReadByte();
            DeviceRestrictions = reader.ReadByte();
            var availableBytes = reader.Length - 18 - SegmentationUpidLength;
            if (availableBytes == 3)
            {
                InsertSubSegmentInfo = reader.ReadByte();
                SubSegmentNum = reader.ReadByte();
                SubSegmentsExpected = reader.ReadByte();
            }
            else if (availableBytes != 0)
            {
                throw new Exception($"Segmentation descriptor length is incorrect. Data: [{reader.ToHex()}]");
            }
        }

        public override void Encode(Scte104Writer writer)
        {
            writer.Write(SegmentationEventID);
            writer.Write(SegmentationEventCancelIndicator);
            writer.Write(Duration);
            writer.Write((byte)SegmentationUpidType);
            writer.Write((byte)SegmentationUpid.Length);
            writer.Write(SegmentationUpid);
            writer.Write((byte)SegmentationTypeId);
            writer.Write(SegmentNum);
            writer.Write(SegmentsExpected);
            writer.Write(DurationExtensionFrames);
            writer.Write(DeliveryNotRestrictedFlag);
            writer.Write(WebDeliveryAllowedFlag);
            writer.Write(NoRegionalBlackoutFlag);
            writer.Write(ArchiveAllowedFlag);
            writer.Write(DeviceRestrictions);
            writer.Write(InsertSubSegmentInfo);
            writer.Write(SubSegmentNum);
            writer.Write(SubSegmentsExpected);
        }

        public override Scte104OperationSummary GetSummary()
            => new Scte104OperationSummary
            {
                OperationID = OperationID,
                ExtraID = (ushort)SegmentationTypeId,
                ExtraData = JsonConvert.SerializeObject(new
                {
                    duration = (long)Duration * 1000 // millisecond
                })
            };
    }
}