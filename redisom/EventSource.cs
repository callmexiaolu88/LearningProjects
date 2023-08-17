using Newtonsoft.Json;
using Redis.OM.Modeling;
namespace redisom
{
    public class EventSource : IUpdatable<EventSource>
    {
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

        #region IUpdatable

        public bool CanUpdate(EventSource other)
        {
            var result = false;
            if (other != null)
            {
                result |= PeerFullID != other.PeerFullID;
                result |= PeerName != other.PeerName;
                result |= PeerType != other.PeerType;
                result |= Url != other.Url;
            }
            return result;
        }

        public void Update(EventSource other)
        {
            if (other != null)
            {
                PeerFullID = other.PeerFullID;
                PeerName = other.PeerName;
                PeerType = other.PeerType;
                Url = other.Url;
            }
        }

        #endregion IUpdatable
    }
}