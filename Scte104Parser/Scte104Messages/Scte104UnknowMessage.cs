/* =============================================
 * Copyright 2021 TVU Networks Co.,Ltd. All rights reserved.
 * For internal members in TVU Networks only.
 * FileName: Scte104UnknowMessage.cs
 * Purpose:
 * Author:   YulongLu added on 12.6th, 2021.
 * Since:    Microsoft Visual Studio 2019
 * =============================================*/

using System.Collections.Generic;

namespace Scte104Parser.Scte104Messages
{
    internal class Scte104UnknowMessage : Scte104Message
    {
        public override Scte104MessageType Type => Scte104MessageType.Unknow;

        public byte[] Data { get; set; }

        public override void Decode(Scte104Reader reader)
        {
            Data = reader[..];
        }

        public override void Encode(Scte104Writer writer)
        {
            writer.WriteBytes(Data);
        }

        public override IEnumerable<Scte104OperationSummary> GetOperationSummaries()
            => new Scte104OperationSummary[0];
    }
}