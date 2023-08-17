/* =============================================
 * Copyright 2021 TVU Networks Co.,Ltd. All rights reserved.
 * For internal members in TVU Networks only.
 * FileName: Scte104MessageOperation.cs
 * Purpose:
 * Author:   YulongLu added on 12.7th, 2021.
 * Since:    Microsoft Visual Studio 2019
 * =============================================*/

namespace Scte104Parser.Scte104Messages.Operations
{
    public abstract class Scte104MessageOperation : IScte104Decoder, IScte104Encoder
    {
        public EnumOperationID OperationID { get; set; }

        public Scte104MessageOperation(EnumOperationID opID)
        {
            OperationID = opID;
        }

        public abstract void Decode(Scte104Reader reader);

        public abstract void Encode(Scte104Writer writer);

        public virtual void Validate()
        {
        }

        public virtual Scte104OperationSummary GetSummary()
            => new Scte104OperationSummary { OperationID = OperationID };
    }
}