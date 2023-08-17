using Newtonsoft.Json;

namespace redisom
{
    public class EventStartTriggerCondition : IUpdatable<EventStartTriggerCondition>
    {
        [JsonProperty(PropertyName = "type")]
        public EnumTriggerType Type { get; set; }

        private string _prerollSource = string.Empty;

        [JsonProperty(PropertyName = "prerollSource")]
        public string PrerollSource
        {
            get { return _prerollSource; }
            set
            {
                if (_prerollSource != value)
                {
                    if (!string.IsNullOrEmpty(value))
                        _prerollSource = value.ToUpper();
                    else
                        _prerollSource = value;
                }
            }
        }

        [JsonProperty(PropertyName = "condition")]
        public string Condition { get; set; }

        [JsonProperty(PropertyName = "reservedId")]
        public string ReservedId { get; set; }

        #region IUpdatable

        public bool CanUpdate(EventStartTriggerCondition other)
        {
            var result = false;
            if (other != null)
            {
                result |= Type != other.Type;
                result |= PrerollSource != other.PrerollSource;
                result |= Condition != other.Condition;
                result |= ReservedId != other.ReservedId;
            }
            return result;
        }

        public void Update(EventStartTriggerCondition other)
        {
            if (other != null)
            {
                Type = other.Type;
                PrerollSource = other.PrerollSource;
                Condition = other.Condition;
                ReservedId = other.ReservedId;
            }
        }

        #endregion IUpdatable
    }

    public class EventEndTriggerCondition : IUpdatable<EventEndTriggerCondition>
    {
        [JsonProperty(PropertyName = "type")]
        public EnumTriggerType Type { get; set; }

        [JsonProperty(PropertyName = "condition")]
        public string Condition { get; set; }

        private string _prematureSource = string.Empty;

        [JsonProperty(PropertyName = "prematureSource")]
        public string PrematureSource
        {
            get { return _prematureSource; }
            set
            {
                if (_prematureSource != value)
                {
                    if (!string.IsNullOrEmpty(value))
                        _prematureSource = value.ToUpper();
                    else
                        _prematureSource = value;
                }
            }
        }

        #region IUpdatable

        public bool CanUpdate(EventEndTriggerCondition other)
        {
            var result = false;
            if (other != null)
            {
                result |= Type != other.Type;
                result |= Condition != other.Condition;
                result |= PrematureSource != other.PrematureSource;
            }
            return result;
        }

        public void Update(EventEndTriggerCondition other)
        {
            if (other != null)
            {
                Type = other.Type;
                PrematureSource = other.PrematureSource;
                Condition = other.Condition;
            }
        }

        #endregion IUpdatable
    }
}