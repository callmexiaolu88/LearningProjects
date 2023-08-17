using System;
using Newtonsoft.Json;

namespace jsonBookingGenerate
{
    class RemoteRecordingModule : AbstractModule
    {
        [JsonIgnore]
        public override EnumModuleType ModuleType => EnumModuleType.RemoteRecording;

        [JsonProperty(PropertyName = "recordId")]
        public string RecordId { get; private set; }

        [JsonProperty(PropertyName = "recordPath")]
        public string RecordPath { get; private set; }

        [JsonProperty(PropertyName = "previewPath")]
        public string PreviewPath { get; private set; }

        private const string DEFAULT_PATH = "362881b36f554796a15f43aadb877f4b/recordings";

        public override string Excute(EnumOperationType operationType)
        {
            string data = Guid.NewGuid().ToString();
            Console.WriteLine($"Type Recording id (default [{data}]):");
            string inputData = Helper.GetInput();
            if (string.IsNullOrWhiteSpace(inputData))
                inputData = data;
            RecordId = inputData;
            Console.WriteLine($"Type Recording Path (default [{DEFAULT_PATH}]):");
            inputData = Helper.GetInput();
            if (string.IsNullOrWhiteSpace(inputData))
                inputData = DEFAULT_PATH;
            RecordPath = inputData;
            Console.WriteLine($"Type Recording Path (default [{RecordPath}/Preview]):");
            inputData = Helper.GetInput();
            if (string.IsNullOrWhiteSpace(inputData))
                inputData = $"{RecordPath}/Preview";
            PreviewPath = inputData;
            return ToJson();
        }
    }
}
