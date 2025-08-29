namespace ResourceVersionGenerator
{
    public enum ExitCodes : int
    {
        OK = 0,
        Unknown = 1,
        CommandLineError = 2,
        ServiceInitializationError = 3,
        WriteOutputfile = 4,
    }
}
