using System.Collections.Generic;
using System.Text.Json.Serialization;
using Redis.OM.Modeling;

namespace redisom
{
    public class LiveBookingEvent : AbstractPreloadEvent<LiveBookingEvent, LiveBookingEventParam>, IUpdatable<LiveBookingEvent>
    {
        #region Properties

        public override EnumScheduleType ScheduleType => EnumScheduleType.Live2;

        [Indexed]
        public bool ExecutedEndSlate { get; set; }

        [Indexed]
        public bool ExecutedPGMStart { get; set; }

        [Indexed(CascadeDepth = 1)]
        [JsonInclude]
        public EventSource LiveEventSource { get; internal set; }

        [Indexed]
        public List<EventSource> LiveEventSources { get; set; }
        #endregion Properties

        #region .ctor

        public LiveBookingEvent()
        { }

        public LiveBookingEvent(ResponseMessage msg) : base(msg)
        {
        }

        #endregion .ctor

        #region Methods

        public override void UpdateStatus(LiveBookingEvent other)
        {
            if (other != null && EventID == other.EventID)
            {
                base.UpdateStatus(other);
                ExecutedEndSlate = other.ExecutedEndSlate;
                ExecutedPGMStart = other.ExecutedPGMStart;
            }
        }

        #endregion Methods
    }
}