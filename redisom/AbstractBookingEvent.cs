using System.Text.Json.Serialization;
using Redis.OM.Modeling;

namespace redisom
{
    [Document(StorageType = StorageType.Json, IndexName ="1111111111111")]
    public abstract class AbstractBookingEvent
    {
        #region Properties

        /// <summary>
        /// Specified by Booking Service.
        /// </summary>
        [RedisIdField]
        [Indexed]
        public string EventID { get; set; }

        [Indexed]
        public string UUID { get; set; }

        [Indexed]
        public string ParentEventId { get; set; }

        public string Affiliate { get; set; }

        /// <summary>
        /// When event starts
        /// </summary>
        [Indexed(Sortable = true, Aggregatable = true)]
        public long Start { get; set; }

        /// <summary>
        /// When event ends
        /// </summary>
        [Indexed(Sortable = true, Aggregatable = true)]
        public long End { get; set; }

        /// <summary>
        /// Event title
        /// </summary>
        [Searchable]
        public string Title { get; set; }

        /// <summary>
        /// Event description
        /// </summary>
        [Searchable]
        public string Description { get; set; }

        [Indexed]
        public EnumEventStatus Status { get; set; }

        [Searchable]
        public string FailedReason { get; set; }

        [Indexed]
        public abstract EnumScheduleType ScheduleType { get; }

        public bool TakeNext { get; set; }

        public string TakeNextId { get; set; }

        #endregion Properties

        #region .ctor

        public AbstractBookingEvent()
        { }

        public AbstractBookingEvent(ResponseMessage msg)
        {
            long.TryParse(msg.StartTime, out long startL);
            long.TryParse(msg.EndTime, out long endL);

            EventID = msg.EventID;
            UUID = msg.UUID;
            ParentEventId = msg.ParentEventId;
            Title = msg.Title;
            Start = startL;
            End = endL;
            Affiliate = msg.CompanyName;
            Description = msg.Description;
            TakeNext = msg.TakeNext;
            TakeNextId = msg.TakeNextId;
        }

        #endregion .ctor

        #region Methods

        public virtual void Verify()
        {
        }

        #endregion Methods
    }

    public abstract class AbstractBookingEvent<TEvent> : AbstractBookingEvent, IUpdatable<TEvent>
        where TEvent : AbstractBookingEvent<TEvent>
    {
        #region .ctor

        public AbstractBookingEvent()
        { }

        public AbstractBookingEvent(ResponseMessage msg) : base(msg)
        {
        }

        #endregion .ctor

        #region IUpdatable

        public virtual bool CanUpdate(TEvent other)
        {
            var result = false;
            if (other != null && EventID == other.EventID)
            {
                result |= UUID != other.UUID;
                result |= Affiliate != other.Affiliate;
                result |= Start != other.Start;
                result |= End != other.End;
                result |= Title != other.Title;
                result |= Description != other.Description;
                result |= TakeNext != other.TakeNext;
                result |= TakeNextId != other.TakeNextId;
                result |= ParentEventId != other.ParentEventId;
            }
            return result;
        }

        public virtual void Update(TEvent other)
        {
            if (other != null && EventID == other.EventID)
            {
                UUID = other.UUID;
                Affiliate = other.Affiliate;
                Start = other.Start;
                End = other.End;
                Title = other.Title;
                Description = other.Description;
                TakeNext = other.TakeNext;
                TakeNextId = other.TakeNextId;
                ParentEventId = other.ParentEventId;
            }
        }

        #endregion IUpdatable

        #region Methods

        public virtual void UpdateStatus(TEvent other)
        {
            if (other != null && EventID == other.EventID)
            {
                Status = other.Status;
                FailedReason = other.FailedReason;
            }
        }

        #endregion Methods
    }

    public abstract class AbstractBookingEvent<TEvent, TEventParam> : AbstractBookingEvent<TEvent>
        where TEvent : AbstractBookingEvent<TEvent, TEventParam>
        where TEventParam : EventParam<TEventParam>
    {
        [Indexed(CascadeDepth = 1)]
        [JsonInclude]
        public TEventParam Param { get; protected set; }

        #region .ctor

        public AbstractBookingEvent()
        { }

        public AbstractBookingEvent(ResponseMessage msg) : base(msg)
        {
        }

        #endregion .ctor

        #region IUpdatable

        public override bool CanUpdate(TEvent other)
        {
            var result = false;
            if (other != null && EventID == other.EventID)
            {
                result = base.CanUpdate(other);
                result |= Param.CanUpdate(other.Param);
            }
            return result;
        }

        public override void Update(TEvent other)
        {
            if (other != null && EventID == other.EventID)
            {
                base.Update(other);
                Param = other.Param;
            }
        }

        #endregion IUpdatable

        #region Methods

        public override void Verify()
        {
            if (Param == null)
                throw new System.Exception("Parameter is null.");
        }

        public void FromMessage(ResponseMessage msg)
        {
            TEventParam param = EventParam<TEventParam>.FromMessage(msg.Parameters);
            Param = param;
        }

        #endregion Methods
    }
}