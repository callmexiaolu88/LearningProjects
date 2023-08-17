using Redis.OM.Modeling;

namespace redisom
{
    public abstract class AbstractPreloadEvent<TEvent, TEventParam> : AbstractBookingEvent<TEvent, TEventParam>
        where TEvent : AbstractPreloadEvent<TEvent, TEventParam>
        where TEventParam : SourceEventParam<TEventParam>
    {
        #region Properties

        [Indexed]
        public bool EventPreloaded { get; set; }

        [Indexed]
        public bool EventStarted { get; set; }

        [Indexed]
        public bool SourcePreloaded { get; set; }

        [Indexed]
        public bool IsFloating { get; set; }

        [Indexed]
        public long ScheduledCheckingEndTime { get; set; }

        #endregion Properties

        #region .ctor

        public AbstractPreloadEvent()
        { }

        public AbstractPreloadEvent(ResponseMessage msg) : base(msg)
        {
        }

        #endregion .ctor

        #region Methods

        public override void UpdateStatus(TEvent other)
        {
            if (other != null && EventID == other.EventID)
            {
                base.UpdateStatus(other);
                EventPreloaded = other.EventPreloaded;
                EventStarted = other.EventStarted;
                SourcePreloaded = other.SourcePreloaded;
                IsFloating = other.IsFloating;
                ScheduledCheckingEndTime = other.ScheduledCheckingEndTime;
            }
        }

        public override void Verify()
        {
            if (Param == null)
                throw new System.Exception("Parameter is null.");
            else if (Param.Source == null)
                throw new System.Exception("Source is null.");
        }

        #endregion Methods
    }
}