﻿using System;
using Newtonsoft.Json;

namespace jsonBookingGenerate.Modules
{
    internal class UnderplayModule : AbstractModule
    {
        public override EnumModuleType ModuleType => EnumModuleType.Underplay;

        [JsonProperty(PropertyName = "peerFullId")]
        public string PeerFullID { get; set; }

        [JsonProperty(PropertyName = "peerName")]
        public string PeerName { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string PeerType { get; set; }

        [JsonProperty(PropertyName = "stopCurrent")]
        public bool StopCurrent { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        public override string Excute(EnumOperationType operationType)
        {
            PeerFullID = "fffffff1100aeb01".ToUpper();
            PeerName = "repeat_57101_3689_Replay_20211230_230500_3689.json";
            PeerType = "file";
            Url = "https://mma-video-new.s3.us-east-1.amazonaws.com/ts/FFFFFFF0000AEB00/45632147896512550000000000000001/211201-22-18-39-780/repeat_57101_3689.m3u8";
            StopCurrent = true;
            return ToJson();
        }
    }
}