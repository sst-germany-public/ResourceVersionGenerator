namespace ResourceVersionGenerator
{
    public enum ExitCodes : int
    {
        /// <summary>
        /// All OK, good to go...
        /// </summary>
        OK = 0,
        /// <summary>
        /// Unknown error.
        /// </summary>
        Unknown = 1,
        /// <summary>
        /// Some unknown problem with the commandline arguments.
        /// </summary>
        CommandLineError = 2,
        /// <summary>
        /// Failed to initialize the DependencyInjection services.
        /// </summary>
        ServiceInitializationError = 3,
        /// <summary>
        /// Unable to write the output file.
        /// </summary>
        WriteOutputfile = 4,
        /// <summary>
        /// Only one of the parameters 'emergency', 'forceVersion' or 'nbgv' can be specified.
        /// </summary>
        MultipleVersionsSpecifiersSet = 5,
    }
}
