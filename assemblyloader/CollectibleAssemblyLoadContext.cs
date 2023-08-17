using System;
using System.Reflection;
using System.Runtime.Loader;
namespace assemblyloader
{
    public class CollectibleAssemblyLoadContext : AssemblyLoadContext
    {
        private readonly AssemblyDependencyResolver _resolver;
        public CollectibleAssemblyLoadContext(string loadPath = null) : base(isCollectible: true)
        {
            _resolver = new AssemblyDependencyResolver(loadPath ?? AppContext.BaseDirectory);
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            Assembly assembly = null;
            var path = _resolver.ResolveAssemblyToPath(assemblyName);
            if (!string.IsNullOrWhiteSpace(path))
            {
                assembly = LoadFromAssemblyPath(path);
            }
            return assembly;
        }

        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            return base.LoadUnmanagedDll(unmanagedDllName);
        }
    }
}