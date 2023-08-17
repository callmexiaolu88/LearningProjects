using System.ComponentModel;

namespace redisom
{
    public enum EnumScheduleType
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

        [Description("Remote recording control schedule event")]
        RemoteRecording = 8,

        [Description("Scte trigger underplay")]
        SCTETriggerUnderplay = 9,

        [Description("Logo schedule event")]
        Logo = 10,

        [Description("Overlay schedule event")]
        Overlay = 11,

        [Description("Break Underplay schedule event")]
        BreakUnderplay = 12,

        [Description("Live emergency schedule event")]
        LiveEmergency = 13,
    }
}