using Newtonsoft.Json;

namespace redisom
{
    public abstract class EventParam<TEventParam> : IUpdatable<TEventParam> where TEventParam : EventParam<TEventParam>
    {
        #region IUpdatable

        public abstract bool CanUpdate(TEventParam other);

        public abstract void Update(TEventParam other);

        #endregion IUpdatable

        protected virtual void Correct()
        {
        }

        public static TEventParam FromMessage(string rawString)
        {
            var param = JsonConvert.DeserializeObject<TEventParam>(rawString);
            param.Correct();
            return param;
        }
    }
}