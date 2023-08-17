using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Orleans;
using Orleans.Configuration;
using Orleans.Runtime;
using Orleans.Storage;

namespace Grain.Storage
{
    public class CustomStorage : IGrainStorage, ILifecycleParticipant<ISiloLifecycle>
    {
        readonly string _storageName;
        readonly CustomStorageOptions _customStorageOptions;
        readonly ClusterOptions _clusterOption;

        public CustomStorage(string storageName, CustomStorageOptions customStorageOptions,ClusterOptions clusterOption)
        {
            _storageName = storageName;
            _customStorageOptions = customStorageOptions;
            _clusterOption = clusterOption;
        }

        public void Participate(ISiloLifecycle lifecycle)
        {
            lifecycle.Subscribe(_storageName, ServiceLifecycleStage.ApplicationServices, Init);
        }

        public Task ClearStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
        {
            var fileName = GetKeyString(grainType, grainReference);
            var path = Path.Combine(_customStorageOptions.RootDirectory, fileName);
            var fileInfo = new FileInfo(path);
            if (fileInfo.Exists)
            {
                if (fileInfo.LastWriteTimeUtc.ToString() != grainState.ETag)
                {
                    throw new InconsistentStateException("version conflict", fileInfo.LastWriteTimeUtc.ToString(), grainState.ETag);
                }
                grainState.ETag = null;
                grainState.State = Activator.CreateInstance(grainState.State.GetType());
                fileInfo.Delete();
            }
            return Task.CompletedTask;
        }

        public async Task ReadStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
        {
            var fileName = GetKeyString(grainType, grainReference);
            var path = Path.Combine(_customStorageOptions.RootDirectory, fileName);
            var fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
            {
                grainState.State = Activator.CreateInstance(grainState.State.GetType());
                return;
            }

            using var reader = fileInfo.OpenText();
            var rawData = await reader.ReadToEndAsync();
            grainState.State = JsonConvert.DeserializeObject(rawData, grainState.State.GetType());
            grainState.ETag = fileInfo.LastWriteTimeUtc.ToString();
        }

        public async Task WriteStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
        {
            var rawData = JsonConvert.SerializeObject(grainState.State);
            var fileName = GetKeyString(grainType, grainReference);
            var path = Path.Combine(_customStorageOptions.RootDirectory, fileName);
            var fileInfo = new FileInfo(path);
            if (fileInfo.Exists && fileInfo.LastWriteTimeUtc.ToString() != grainState.ETag)
            {
                throw new InconsistentStateException("version conflict", fileInfo.LastWriteTimeUtc.ToString(), grainState.ETag);
            }

            using var writer = new StreamWriter(fileInfo.Open(FileMode.Create, FileAccess.Write));
            await writer.WriteAsync(rawData);
            fileInfo.Refresh();
            grainState.ETag = fileInfo.LastWriteTimeUtc.ToString();
            return;
        }

        private Task Init(CancellationToken ct)
        {
            var directory = new System.IO.DirectoryInfo(_customStorageOptions.RootDirectory);
            if (!directory.Exists)
                directory.Create();

            return Task.CompletedTask;
        }

        private string GetKeyString(string grainType, GrainReference grainReference)
        {
            return $"{_clusterOption.ServiceId}.{grainReference.ToKeyString()}.{grainType}";
        }

    }
}
