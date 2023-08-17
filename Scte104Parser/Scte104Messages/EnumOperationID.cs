/* =============================================
 * Copyright 2021 TVU Networks Co.,Ltd. All rights reserved.
 * For internal members in TVU Networks only.
 * FileName: EnumOperationID.cs
 * Purpose:
 * Author:   YulongLu added on 12.7th, 2021.
 * Since:    Microsoft Visual Studio 2019
 * =============================================*/

namespace Scte104Parser.Scte104Messages
{
    public enum EnumOperationID
    {
        SpliceRequestData = 0x0101,
        SpliceNull = 0x0102,
        TimeSignalRequestData = 0x0104,
        InsertDTMFDescriptor = 0x0109,
        InsertSegmentationDescriptor = 0x010B,
        TimeSignalMultipleDescriptors = 0xFF10,
    }
}