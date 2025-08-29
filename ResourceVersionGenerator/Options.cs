namespace ResourceVersionGenerator
{
    public class Options
    {
        [CommandLine.Option("company", Required = true, HelpText = "Company name. Used to generate copyright text.")]
        public string? Company { get; set; } = null;

        [CommandLine.Option('o', "output", Required = false, HelpText = "The output file.")]
        public string? OutputFilename { get; set; } = ".\\resourceVersion.h";

        [CommandLine.Option('p', "product", Required = false, HelpText = "Product name.")]
        public string? Product { get; set; } = null;

        [CommandLine.Option('d', "description", Required = false, HelpText = "Product description.")]
        public string? Description { get; set; } = null;

        [CommandLine.Option('f', "originalFilename", Required = false, HelpText = "Original filename of the file you are writing the version for.")]
        public string? OriginalFilename { get; set; } = null;

        [CommandLine.Option('f', "forceGet", Required = false, HelpText = "Forces the execution of 'nbgv' even if valid environment variables are available.")]
        public bool ForceGet { get; set; } = false;
        [CommandLine.Option('n', "nbgv", Required = false, HelpText = "Use NBGV to process version")]
        public bool UseNBGV { get; set; } = false;

        [CommandLine.Option("version", Required = false, HelpText = "Version")]
        public string? Version { get; set; } = null;

        [CommandLine.Option('v', "verbose", Required = false, HelpText = "Verbose console messages.")]
        public bool Verbose { get; set; } = false;

    }
}