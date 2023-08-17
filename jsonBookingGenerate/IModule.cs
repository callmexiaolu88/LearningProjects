namespace jsonBookingGenerate
{
    interface IModule
    {
        int Index { get; }
        string Name { get; }
        EnumModuleType ModuleType { get; }
        string Excute(EnumOperationType operationType);
    }
}
