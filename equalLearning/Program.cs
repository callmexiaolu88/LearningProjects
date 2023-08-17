using System;
using System.Collections;
using System.Linq;

namespace equalLearning
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            DefaultPlaybackShimHubOfT defaultPlaybackShimHub = new DefaultPlaybackShimHubOfT();
            defaultPlaybackShimHub.RegisterPlaybackShim(new BookingPlayback<PlaybackSkeleton>());
            defaultPlaybackShimHub.RegisterPlaybackShim(new BookingPlayback<BookingPlaybackSkeleton>());
            defaultPlaybackShimHub.Preload(new BookingPlaybackSkeleton("BookingPlaybackSkeleton"));
            defaultPlaybackShimHub.Preload(new PlaybackSkeleton("PlaybackSkeleton"));
            System.Console.WriteLine("asd");
        }
    }
}
