using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Redis.OM;
using Redis.OM.Modeling;

namespace redisom
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var provider = new RedisConnectionProvider("redis://10.12.32.91:16379");
            var repository = provider.RedisCollection<LiveBookingEvent>();
            var message = new ResponseMessage
            {
                Description = "Las noticias y reportajes más impactantes de Estados Unidos y del mundo.",
                EndTime = "1690765200000",
                EventID = "1595186",
                Parameters = "{\"secondaryType\":null,\"switchType\":null,\"isTakeNext\":false,\"offset\":0,\"bitrate\":\"5000\",\"reporter\":null,\"startTriggerCondition\":null,\"type\":\"video\",\"url\":\"https://pp-va-001.s3.amazonaws.com/344e3907cf4e4d0290bb53e329512618/INCOMING/LMPI100123002R2_TVU_MP4_4200.mp4\",\"peerName\":\"LMPI100123002R2_TVU_MP4_4200.mp4\",\"delay\":\"2\",\"secondaryPeerFullId\":null,\"secondaryUrl\":null,\"loop\":false,\"peerFullId\":\"55524c204558544c000011899c604a7c\",\"endTriggerCondition\":null,\"subtitleUrl\":null,\"stopAdBreak\":true,\"secondaryPeerName\":null}",
                ScheduleType = EnumScheduleType.Live2,
                StartTime = "1690761600000",
                Title = "Lo Mejor de Primer Impacto"
            };
            var liveEvent = new LiveBookingEvent(message);
            liveEvent.IsFloating = true;
            liveEvent.LiveEventSource = new EventSource
            {
                PeerName = "11111111111",
                Url = "1111111111111"
            };
            liveEvent.LiveEventSources = new List<EventSource> {
                new EventSource
            {
                PeerName = "222222222222222",
                Url = "222222222222222"
            },                new EventSource
            {
                PeerName = "333333333333333",
                Url = "333333333333333"
            }};
            liveEvent.FromMessage(message);
            var type = liveEvent.GetType();
            var attr = Attribute.GetCustomAttribute(type, typeof(DocumentAttribute)) as DocumentAttribute;
            var id = repository.InsertAsync(liveEvent).Result;
            System.Console.WriteLine(id);
            var testobj = repository.FindById(id);
            System.Console.WriteLine(JsonConvert.SerializeObject(testobj));

        }
    }
}
