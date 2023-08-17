/* =============================================
 * Copyright 2021 TVU Networks Co.,Ltd. All rights reserved.
 * For internal members in TVU Networks only.
 * FileName: Scte104OperationSummary.cs
 * Purpose:
 * Author:   YulongLu added on 12.7th, 2021.
 * Since:    Microsoft Visual Studio 2019
 * =============================================*/

namespace Scte104Parser.Scte104Messages
{
    public class Scte104OperationSummary
    {
        public EnumOperationID OperationID { get; set; }
        public ushort ExtraID { get; set; }
        public string ExtraData { get; set; }
    }
}