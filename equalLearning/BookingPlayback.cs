using System;
using System.Text.Json;
namespace equalLearning
{
    class BookingPlayback<T> : IPlaybackShim<T> where T : IPlaybackSkeleton
    {
        public string ID => "BookingPlayback";

        public void Delete(T playbackSkeleton)
            => Console.WriteLine(JsonSerializer.Serialize(playbackSkeleton));

        public void FromBegin(T playbackSkeleton)
            => Console.WriteLine(JsonSerializer.Serialize(playbackSkeleton));


        public void Pause(T playbackSkeleton)
            => Console.WriteLine(JsonSerializer.Serialize(playbackSkeleton));

        public void Preload(T playbackSkeleton)
            => Console.WriteLine(JsonSerializer.Serialize(playbackSkeleton));

        public void PreloadAndPause(T playbackSkeleton)
            => Console.WriteLine(JsonSerializer.Serialize(playbackSkeleton));

        public void Resume(T playbackSkeleton)
            => Console.WriteLine(JsonSerializer.Serialize(playbackSkeleton));

        public void SetLoop(T playbackSkeleton, bool flag)
            => Console.WriteLine($"{JsonSerializer.Serialize(playbackSkeleton)}, flag:{flag}");

        public void Start(T playbackSkeleton)
            => Console.WriteLine(JsonSerializer.Serialize(playbackSkeleton));

        public void Stop(T playbackSkeleton)
            => Console.WriteLine(JsonSerializer.Serialize(playbackSkeleton));
    }
}