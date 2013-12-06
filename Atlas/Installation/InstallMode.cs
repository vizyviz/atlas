namespace Atlas.Installation
{
    internal enum InstallMode : byte
    {
        NotSet = 0,
        Install = 1,
        InstallAndStart = 2,
        Uninstall = 3
    }
}
