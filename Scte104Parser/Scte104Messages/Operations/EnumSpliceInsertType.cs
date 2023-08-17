/* =============================================
 * Copyright 2021 TVU Networks Co.,Ltd. All rights reserved.
 * For internal members in TVU Networks only.
 * FileName: EnumSpliceInsertType.cs
 * Purpose:  Enum of SCTE SpliceOperation
 * Author:   YulongLu added on 6.4th, 2021.
 * Since:    Microsoft Visual Studio 2019
 * =============================================*/

namespace Scte104Parser.Scte104Messages.Operations
{
    public enum EnumSpliceInsertType : ushort
    {
        SpliceNone = 0,
        SplicestartNormal,
        SplicestartImmediate,
        SpliceendNormal,
        SpliceendImmediate,
        SpliceCancel
    }
}
