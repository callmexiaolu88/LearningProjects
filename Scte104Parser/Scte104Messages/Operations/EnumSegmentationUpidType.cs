/* =============================================
 * Copyright 2022 TVU Networks Co.,Ltd. All rights reserved.
 * For internal members in TVU Networks only.
 * FileName: EnumSegmentationUpidType.cs
 * Purpose:
 * Author:   YulongLu added on 7.13th, 2022.
 * Since:    Microsoft Visual Studio 2019
 * =============================================*/

namespace Scte104Parser.Scte104Messages.Operations
{
    public enum EnumSegmentationUpidType
    {
        NotUsed = 0x00,
        UserDefined = 0x01,
        ISCI8 = 0x02, //ABCD1234
        AdID12 = 0x03, //ABCD0001000H
        UMID32 = 0x04, //060A2B34.01010105.01010D20.13000000.D2C9036C.8F195343.AB7014D2.D718BFDA
        ISAN8 = 0x05,
        ISAN12 = 0x06, //0000-0001-2C52-0000-P-0000-0000-0
        TID12 = 0x07, //MV0004146400
        TI8 = 0x08, //0x0A42235B81BC70FC
        ADI = 0x09,
        EIDR12 = 0x0A, //10.5240/0E4F892E-442F-6BD4-15B0-1 or 10.5239/C370-DCA5
        ATSC = 0x0B,
        MPU = 0x0C,
        MID = 0x0D,
        ADS = 0x0E,
        URI = 0x0F, //urn:uuid:f81d4fae7dec-11d0-a765-00a0c91e6bf6
        UUID16 = 0x10, //0xCB0350A948774CA7BB638730B37A98CF
    }
}