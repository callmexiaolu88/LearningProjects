using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace jsonBookingGenerate
{
    public enum EnumModuleType
    {
        [Description("Peer live schedule event")]
        Live = 0,

        [Description("SCTE control schedule event")]
        SCTE = 1,

        [Description("Recording control schedule event")]
        Recording = 2,

        [Description("Transcriber control schedule event")]
        Transcriber = 3,

        [Description("Underplay control schedule event")]
        Underplay = 4,

        [Description("New peer live schedule event")]
        Live2 = 5,

        [Description("Overlay schedule event")]
        Crawl = 6,

        [Description("Emergency schedule event")]
        Emergency = 7,

        [Description("Remte recording control schedule event")]
        RemoteRecording = 8,
    }
}
