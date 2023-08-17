using Newtonsoft.Json;
using Redis.OM.Modeling;

namespace redisom
{
    public class LiveBookingEventParam : SecondarySourceEventParam<LiveBookingEventParam>, IUpdatable<LiveBookingEventParam>
    {
        #region Properties

        [Indexed]
        [JsonProperty(PropertyName = "reporter")]
        public string Reporter { get; set; }

        [Indexed]
        [JsonProperty(PropertyName = "immediateStart")]
        public bool ImmediateStart { get; set; }

        [Indexed(CascadeDepth = 1)]
        [JsonProperty(PropertyName = "startTriggerCondition")]
        public EventStartTriggerCondition StartTriggerCondition { get; set; }

        [Indexed(CascadeDepth = 1)]
        [JsonProperty(PropertyName = "endTriggerCondition")]
        public EventEndTriggerCondition EndTriggerCondition { get; set; }

        [Indexed]
        [JsonProperty(PropertyName = "stopAdBreak")]
        public bool StopAdBreak { get; set; } = true;

        [Indexed(CascadeDepth = 1)]
        [JsonProperty(PropertyName = "endSlateSource")]
        public EventSource EndSlateSource { get; set; }

        [Indexed]
        [JsonProperty(PropertyName = "endSlateDuration")]
        public long EndSlateDuration { get; set; }

        [Indexed]
        [JsonProperty(PropertyName = "isTakeNext")]
        public bool IsTakeNext { get; set; }

        #endregion Properties

        #region IUpdatable

        public override bool CanUpdate(LiveBookingEventParam other)
        {
            var result = false;
            if (other != null)
            {
                result |= base.CanUpdate(other);
                result |= Reporter != other.Reporter;
                result |= ImmediateStart != other.ImmediateStart;
                result |= StartTriggerCondition.CanUpdate(other.StartTriggerCondition);
                result |= EndTriggerCondition.CanUpdate(other.EndTriggerCondition);
                result |= StopAdBreak != other.StopAdBreak;
                result |= EndSlateDuration != other.EndSlateDuration;
                result |= IsTakeNext != other.IsTakeNext;
                result |= EndSlateSource.CanUpdate(other.EndSlateSource);
            }
            return result;
        }

        public override void Update(LiveBookingEventParam other)
        {
            if (other != null)
            {
                base.Update(other);
                Reporter = other.Reporter;
                ImmediateStart = other.ImmediateStart;
                StartTriggerCondition.Update(other.StartTriggerCondition);
                EndTriggerCondition.Update(other.EndTriggerCondition);
                StopAdBreak = other.StopAdBreak;
                EndSlateDuration = other.EndSlateDuration;
                IsTakeNext = other.IsTakeNext;
                EndSlateSource.Update(other.EndSlateSource);
            }
        }

        #endregion IUpdatable
    }
}