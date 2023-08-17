using System;
using Newtonsoft.Json;

namespace redisom
{
    public class ResponseMessage
    {
        #region Properties

        [JsonProperty(PropertyName = "eventId")]
        public string EventID { get; set; }

        [JsonProperty(PropertyName = "uuId")]
        public string UUID { get; set; }

        [JsonProperty(PropertyName = "parentEventId")]
        public string ParentEventId { get; set; }

        [JsonProperty(PropertyName = "affiliate")]
        public string CompanyName { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        private string _startTime;

        [JsonProperty(PropertyName = "startTime")]
        public string StartTime
        {
            get { return _startTime; }
            set
            {
                if (_startTime != value)
                {
                    _startTime = value;
                    _dtStartTime = StrUnixMillisecondsToDateTimeOffSet(value);
                }
            }
        }

        private DateTimeOffset _dtStartTime;

        [JsonIgnore]
        public DateTimeOffset DTStartTime
        {
            get { return _dtStartTime; }
        }

        private string _endTime;

        [JsonProperty(PropertyName = "endTime")]
        public string EndTime
        {
            get { return _endTime; }
            set
            {
                if (_endTime != value)
                {
                    _endTime = value;
                    _dtEndTime = StrUnixMillisecondsToDateTimeOffSet(value);
                }
            }
        }

        private DateTimeOffset _dtEndTime;

        [JsonIgnore]
        public DateTimeOffset DTEndTime
        {
            get { return _dtEndTime; }
        }

        [JsonProperty(PropertyName = "parameters")]
        public string Parameters { get; set; }

        [JsonProperty(PropertyName = "scheduleType")]
        public EnumScheduleType ScheduleType { get; set; } = EnumScheduleType.Live;

        [JsonProperty(PropertyName = "takeNext")]
        public bool TakeNext { get; set; }

        [JsonProperty(PropertyName = "takeNextId")]
        public string TakeNextId { get; set; }

        [JsonIgnore]
        public bool IsValidResponse
        {
            get
            {
                return !string.IsNullOrEmpty(EventID) && ScheduleType switch
                {
                    EnumScheduleType.Transcriber => true,
                    _ => !string.IsNullOrEmpty(Parameters)
                };
            }
        }

        #endregion Properties

        #region Public Methods

        public static bool IsResponseMsgValid(ResponseMessage responseMsg)
        {
            if (responseMsg == null || !responseMsg.IsValidResponse)
                return false;
            else
                return true;
        }

        #endregion Public Methods

        #region Private Methods

        public static DateTimeOffset StrUnixMillisecondsToDateTimeOffSet(string strUTCTime)
        {
            long utcTime = long.Parse(strUTCTime);
            DateTimeOffset datetime =DateTimeOffset.FromUnixTimeMilliseconds(utcTime).ToLocalTime();
            return datetime;
        }

        #endregion Private Methods

        #region Json

        public static ResponseMessage FromJson(string rawString)
        {
            try
            {
                if (!string.IsNullOrEmpty(rawString))
                    return JsonConvert.DeserializeObject<ResponseMessage>(rawString);
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }

        public string ToJson()
        {
            try
            {
                return JsonConvert.SerializeObject(this);
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion Json
    }
}