using Newtonsoft.Json;

namespace jsonBookingGenerate
{
    public abstract class AbstractModule : IModule
    {
        [JsonIgnore]
        public int Index => (int)ModuleType;
        [JsonIgnore]
        public string Name => ModuleType.ToString();
        [JsonIgnore]
        public abstract EnumModuleType ModuleType { get; }

        public AbstractModule()
        {

        }

        public abstract string Excute(EnumOperationType operationType);

        public string ToJson() 
            => JsonConvert.SerializeObject(this);
    }
}
