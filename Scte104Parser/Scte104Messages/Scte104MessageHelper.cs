/* =============================================
 * Copyright 2021 TVU Networks Co.,Ltd. All rights reserved.
 * For internal members in TVU Networks only.
 * FileName: Scte104MessageHelper.cs
 * Purpose:
 * Author:   YulongLu added on 12.6th, 2021.
 * Since:    Microsoft Visual Studio 2019
 * =============================================*/

namespace Scte104Parser.Scte104Messages
{
    public static class Scte104MessageHelper
    {
        public static Scte104Message ParseScte104Message(string scte104Hex)
        {
            Scte104Message message = null;
            var rawInfo = Scte104Reader.CreateBy(scte104Hex);
            var condition = rawInfo.PeekUInt16();
            if (condition == 0xFFFF)
                message = new Scte104MultipleOperationMessage();
            else if ((condition >= 0x0000 && condition <= 0x0012) || (condition >= 0x8000 && condition <= 0xBFFF))
                message = new Scte104SingleOperationMessage();
            else
                message = new Scte104UnknowMessage();
            message.Decode(rawInfo);
            return message;
        }
    }
}