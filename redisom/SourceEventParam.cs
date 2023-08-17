using System;
using Newtonsoft.Json;
using Redis.OM.Modeling;

namespace redisom
{
    public abstract class SourceEventParam<TEventParam> : EventParam<TEventParam> where TEventParam : SourceEventParam<TEventParam>
    {
        #region Properties

        private string _peerFullID = string.Empty;

        [Indexed]
        [JsonProperty(PropertyName = "peerFullId")]
        public string PeerFullID
        {
            get { return _peerFullID; }
            set
            {
                if (_peerFullID != value)
                {
                    if (!string.IsNullOrEmpty(value))
                        _peerFullID = value.ToUpper();
                    else
                        _peerFullID = value;
                }
            }
        }

        [Indexed]
        [JsonProperty(PropertyName = "peerName")]
        public string PeerName { get; set; }

        [Indexed]
        [JsonProperty(PropertyName = "type")]
        public string PeerType { get; set; }

        [Indexed]
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [Indexed(CascadeDepth = 1)]
        [JsonProperty(PropertyName = "peerSource")]
        public EventSource Source { get; set; }

        [Indexed]
        [JsonProperty(PropertyName = "bitrate")]
        public ushort Bitrate { get; set; }

        [Indexed]
        [JsonProperty(PropertyName = "delay")]
        public double Delay { get; set; }

        [Indexed]
        [JsonProperty(PropertyName = "duration")]
        public long Duration { get; set; }

        [Indexed]
        [JsonProperty(PropertyName = "offset")]
        public long Offset { get; set; }

        [Indexed]
        [JsonProperty(PropertyName = "loop")]
        public bool Loop { get; set; }

        [Indexed]
        [JsonProperty(PropertyName = "subtitleUrl")]
        public string SubtitleUrl { get; set; }

        [Indexed]
        [JsonProperty(PropertyName = "audioShuffling")]
        public string AudioShuffling { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public TimeSpan Preroll
        {
            get => TimeSpan.FromSeconds(40);
            set { }
        }

        #endregion Properties

        #region Methods


        protected override void Correct()
        {
            Source ??= new EventSource
            {
                PeerFullID = PeerFullID,
                PeerName = PeerName,
                PeerType = PeerType,
                Url = Url,
            };
        }

        public override bool CanUpdate(TEventParam param)
        {
            var result = false;
            if (param != null)
            {
                result |= PeerFullID != param.PeerFullID;
                result |= PeerName != param.PeerName;
                result |= Url != param.Url;
                result |= Bitrate != param.Bitrate;
                result |= Delay != param.Delay;
                result |= Duration != param.Duration;
                result |= Offset != param.Offset;
                result |= Loop != param.Loop;
                result |= SubtitleUrl != param.SubtitleUrl;
                result |= AudioShuffling != param.AudioShuffling;
            }
            return result;
        }

        public override void Update(TEventParam param)
        {
            if (param != null)
            {
                PeerFullID = param.PeerFullID;
                PeerName = param.PeerName;
                Url = param.Url;
                Bitrate = param.Bitrate;
                Delay = param.Delay;
                Duration = param.Duration;
                Offset = param.Offset;
                Loop = param.Loop;
                SubtitleUrl = param.SubtitleUrl;
                AudioShuffling = param.AudioShuffling;
                Correct();
            }
        }

        #endregion Methods
    }

    public abstract class SecondarySourceEventParam<TEventParam> : SourceEventParam<TEventParam> where TEventParam : SecondarySourceEventParam<TEventParam>
    {
        #region Properties

        private string _secondaryPeerFullId = string.Empty;

        [JsonProperty(PropertyName = "secondaryPeerFullId")]
        public string SecondaryPeerFullID
        {
            get { return _secondaryPeerFullId; }
            set
            {
                if (_secondaryPeerFullId != value)
                {
                    if (!string.IsNullOrEmpty(value))
                        _secondaryPeerFullId = value.ToUpper();
                    else
                        _secondaryPeerFullId = value;
                }
            }
        }

        [JsonProperty(PropertyName = "secondaryPeerName")]
        public string SecondaryPeerName { get; set; }

        [JsonProperty(PropertyName = "secondaryType")]
        public string SecondaryPeerType { get; set; }

        [JsonProperty(PropertyName = "secondaryUrl")]
        public string SecondaryUrl { get; set; }

        [JsonProperty(PropertyName = "secondaryPeerSource")]
        public EventSource SecondarySource { get; set; }

        #endregion Properties

        #region Method

        protected override void Correct()
        {
            base.Correct();
            SecondarySource ??= new EventSource
            {
                PeerFullID = SecondaryPeerFullID,
                PeerName = SecondaryPeerName,
                PeerType = SecondaryPeerType,
                Url = SecondaryUrl,
            };
        }

        public override bool CanUpdate(TEventParam param)
        {
            var result = false;
            if (param != null)
            {
                result |= base.CanUpdate(param);
                result |= SecondaryPeerFullID != param.SecondaryPeerFullID;
                result |= SecondaryPeerName != param.SecondaryPeerName;
                result |= SecondaryPeerType != param.SecondaryPeerType;
                result |= SecondaryUrl != param.SecondaryUrl;
            }
            return result;
        }

        public override void Update(TEventParam param)
        {
            if (param != null)
            {
                base.Update(param);
                SecondaryPeerFullID = param.SecondaryPeerFullID;
                SecondaryPeerName = param.SecondaryPeerName;
                SecondaryPeerType = param.SecondaryPeerType;
                SecondaryUrl = param.SecondaryUrl;
                Correct();
            }
        }

        #endregion Method
    }
}