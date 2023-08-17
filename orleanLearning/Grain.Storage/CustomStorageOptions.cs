using System;

namespace Grain.Storage
{
    public class CustomStorageOptions
    {
        public string RootDirectory { get; set; } = AppDomain.CurrentDomain.BaseDirectory;
    }
}