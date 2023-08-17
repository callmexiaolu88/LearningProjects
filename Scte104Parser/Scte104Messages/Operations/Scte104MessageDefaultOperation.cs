/* =============================================
 * Copyright 2021 TVU Networks Co.,Ltd. All rights reserved.
 * For internal members in TVU Networks only.
 * FileName: Scte104MessageDefaultOperation.cs
 * Purpose:
 * Author:   YulongLu added on 12.7th, 2021.
 * Since:    Microsoft Visual Studio 2019
 * =============================================*/

namespace Scte104Parser.Scte104Messages.Operations
{
    internal class Scte104MessageDefaultOperation : Scte104MessageOperation
    {
        public byte[] Data { get; set; }

        public Scte104MessageDefaultOperation(EnumOperationID opID) : base(opID)
        {
            Data = new byte[0];
        }

        public override void Decode(Scte104Reader reader)
        {
            Data = reader[..];
        }

        public override void Encode(Scte104Writer writer)
        {
            writer.WriteBytes(Data);
        }
    }
}