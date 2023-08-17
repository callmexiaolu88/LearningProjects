/* =============================================
 * Copyright 2021 TVU Networks Co.,Ltd. All rights reserved.
 * For internal members in TVU Networks only.
 * FileName: IPlaybackShim.cs
 * Purpose:  
 * Author:   YulongLu added on 8.27th, 2021.
 * Since:    Microsoft Visual Studio 2019
 * =============================================*/

namespace equalLearning
{
    public interface IPlaybackShim
    {
        string ID { get; }
        void Preload(string sourceId, string uniqueId);
        void PreloadAndPause(string sourceId, string uniqueId);
        void Start(string sourceId, string uniqueId);
        void Stop(string sourceId, string uniqueId);
        void Delete(string sourceId, string uniqueId);
        void FromBegin(string uniqueId);
        void Pause(string uniqueId);
        void Resume(string uniqueId);
        void SetLoop(string uniqueId, bool flag);
    }

    public interface IPlaybackShim<in T> where T : IPlaybackSkeleton
    {
        string ID { get; }
        void Preload(T playbackSkeleton);
        void PreloadAndPause(T playbackSkeleton);
        void Start(T playbackSkeleton);
        void Stop(T playbackSkeleton);
        void Delete(T playbackSkeleton);
        void FromBegin(T playbackSkeleton);
        void Pause(T playbackSkeleton);
        void Resume(T playbackSkeleton);
        void SetLoop(T playbackSkeleton, bool flag);
    }

    public interface IPlaybackSkeleton
    {
        string SourceId { get; }
        string UniqueId { get; }
    }

    public class PlaybackSkeleton : IPlaybackSkeleton
    {
        public PlaybackSkeleton(string id)
        {
            SourceId = UniqueId = id;
        }
        public virtual string SourceId { get; }

        public virtual string UniqueId{ get; }
    }

    public class BookingPlaybackSkeleton : PlaybackSkeleton
    {
        public BookingPlaybackSkeleton(string id) : base(id)
        {
            SourceId = UniqueId = id;
        }

        public override string SourceId { get; }

        public override string UniqueId { get; }
    }
}
