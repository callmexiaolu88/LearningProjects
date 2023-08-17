/* =============================================
 * Copyright 2021 TVU Networks Co.,Ltd. All rights reserved.
 * For internal members in TVU Networks only.
 * FileName: Scte104Message.cs
 * Purpose:
 * Author:   YulongLu added on 12.6th, 2021.
 * Since:    Microsoft Visual Studio 2019
 * =============================================*/

using System.Collections.Generic;

namespace Scte104Parser.Scte104Messages
{
    public abstract class Scte104Message : IScte104Decoder, IScte104Encoder
    {
        public abstract Scte104MessageType Type { get; }

        public abstract void Encode(Scte104Writer writer);

        public abstract void Decode(Scte104Reader reader);

        public virtual void Validate()
        { }

        public abstract IEnumerable<Scte104OperationSummary> GetOperationSummaries();
    }

    public enum Scte104MessageType
    {
        Unknow = 0,
        SingleOperation,
        MultipleOperation
    }

    public class Scte104Timestamp
    {
        public byte TimeType { get; set; }
        public int Length => RawData.Length + 1;
        public byte[] RawData { get; set; } = new byte[0];
    }
}