using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using NLog;

namespace equalLearning
{
    internal class DefaultPlaybackShimHubOfT
    {
        #region Log

        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        #endregion Log

        #region Properties & Fields

        private ConcurrentDictionary<string, IPlaybackShim> _shims = new ConcurrentDictionary<string, IPlaybackShim>();
        private ConcurrentDictionary<Type, object> _typedShims = new ConcurrentDictionary<Type, object>();

        public string ID => "DefaultPlaybackShimHub";

        #endregion Properties & Fields

        #region IPlaybackShim

        public void Preload<T>(T playbackSkeleton) where T : IPlaybackSkeleton
            => dispatchCalling<T>(_typedShims => _typedShims.Preload(playbackSkeleton));

        public void PreloadAndPause<T>(T playbackSkeleton) where T : IPlaybackSkeleton
            => dispatchCalling<T>(_typedShims => _typedShims.PreloadAndPause(playbackSkeleton));

        public void Start<T>(T playbackSkeleton) where T : IPlaybackSkeleton
            => dispatchCalling<T>(_typedShims => _typedShims.Start(playbackSkeleton));

        public void Stop<T>(T playbackSkeleton) where T : IPlaybackSkeleton
            => dispatchCalling<T>(_typedShims => _typedShims.Stop(playbackSkeleton));

        public void Delete<T>(T playbackSkeleton) where T : IPlaybackSkeleton
            => dispatchCalling<T>(_typedShims => _typedShims.Delete(playbackSkeleton));

        public void FromBegin<T>(T playbackSkeleton) where T : IPlaybackSkeleton
            => dispatchCalling<T>(_typedShims => _typedShims.FromBegin(playbackSkeleton));

        public void Pause<T>(T playbackSkeleton) where T : IPlaybackSkeleton
            => dispatchCalling<T>(_typedShims => _typedShims.Pause(playbackSkeleton));

        public void Resume<T>(T playbackSkeleton) where T : IPlaybackSkeleton
            => dispatchCalling<T>(_typedShims => _typedShims.Resume(playbackSkeleton));

        public void SetLoop<T>(T playbackSkeleton, bool flag) where T : IPlaybackSkeleton
            => dispatchCalling<T>(_typedShims => _typedShims.SetLoop(playbackSkeleton, flag));

        #endregion IPlaybackShim

        public bool RegisterPlaybackShim<T>(IPlaybackShim<T> playbackShim) where T : IPlaybackSkeleton
        {
            if (playbackShim != null)
            {
                return _typedShims.TryAdd(typeof(T), playbackShim);
            }
            else
            {
                _logger.Warn("RegisterPlaybackShim() PlaybackShim is null.");
            }
            return false;
        }

        private void dispatchCalling<TImple>(Action<IPlaybackShim<TImple>> action, [CallerMemberName] string callerName = null) where TImple : IPlaybackSkeleton
        {
            if( _typedShims.TryGetValue(typeof(TImple), out object shim1) && shim1 is IPlaybackShim<TImple> shim)
            {
                try
                {
                    _logger.Info($"{callerName}() Dispatch [{callerName}] calling to [{shim.ID}].");
                    action(shim);
                    _logger.Info($"{callerName}() Dispatch [{callerName}] calling to [{shim.ID}] done.");
                }
                catch (Exception ex)
                {
                    _logger.Debug($"{callerName}() Exception:{ex}");
                    _logger.Error($"{callerName}() Exception:{ex.Message}");
                }
            }
            else
            {
                _logger.Warn($"{callerName}() Not find handler for {typeof(TImple).Name}");
            }
        }
    }
}