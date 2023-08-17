using System;
using System.Threading.Tasks;
using Grain.Infrastructures.Streams;
using Grain.Interfaces;
using Microsoft.Extensions.Logging;
using Orleans.Runtime;
using Orleans.Storage;

namespace Grain.Implement
{
    public class UserGrain : Orleans.Grain, IUserGrain
    {
        readonly ILogger<UserGrain> _logger;
        readonly IPersistentState<int> _persistentState;
        readonly IGrainStorage _grainStorage;

        public UserGrain(ILogger<UserGrain> logger/*, IGrainStorage grainStorage*/, [PersistentState("testint", "customStorageProvider")]IPersistentState<int> persistentState)
        {
            _logger = logger;
            _persistentState = persistentState;
            //_grainStorage = grainStorage;
        }
        public async Task<bool> Exist()
        {
            var streamProvider = GetStreamProvider("customStreamProvider");
            var stream = streamProvider.GetStream<CustomStreamData>(Guid.NewGuid(), "Default");

            await _persistentState.ReadStateAsync();
            _persistentState.State++;
            await _persistentState.WriteStateAsync();

            _logger.LogInformation($"Entrance Exist(): {{Count:{_persistentState.State}, Name:{GrainReference.GrainIdentity.PrimaryKeyString}}};");
            var guid = GrainReference.GrainIdentity.GetPrimaryKey(out var stringValue);
            var @long = GrainReference.GrainIdentity.GetPrimaryKeyLong(out stringValue);
            return GrainReference.GrainIdentity.PrimaryKeyString == "121212";
        }

        public override Task OnActivateAsync()
        {
            return base.OnActivateAsync();
        }
    }
}   