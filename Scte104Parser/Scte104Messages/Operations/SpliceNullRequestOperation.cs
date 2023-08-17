/* =============================================
 * Copyright 2021 TVU Networks Co.,Ltd. All rights reserved.
 * For internal members in TVU Networks only.
 * FileName: SpliceNullRequestOperation.cs
 * Purpose:  
 * Author:   YulongLu added on 12.7th, 2021.
 * Since:    Microsoft Visual Studio 2019
 * =============================================*/

namespace Scte104Parser.Scte104Messages.Operations
{
    internal class SpliceNullRequestOperation : Scte104MessageOperation
    {
        public SpliceNullRequestOperation(): base(EnumOperationID.SpliceNull)
        {
        }

        public override void Decode(Scte104Reader reader)
        {
        }

        public override void Encode(Scte104Writer writer)
        {
        }
    }
}