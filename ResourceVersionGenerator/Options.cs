namespace ResourceVersionGenerator
{
    public class Options
    {
        [CommandLine.Option('c', "company", Required = true, HelpText = "Company name. Used to generate copyright text. Default: null")]
        public string? Company { get; set; } = null;

        [CommandLine.Option('o', "output", Required = false, HelpText = "The output file. Default: '.\\resourceVersion.h'")]
        public string? OutputFilename { get; set; } = ".\\resourceVersion.h";

        [CommandLine.Option('p', "product", Required = false, HelpText = "Product name. Default: null")]
        public string? Product { get; set; } = null;

        [CommandLine.Option('d', "description", Required = false, HelpText = "Product description. Default: null")]
        public string? Description { get; set; } = null;

        [CommandLine.Option("originalFilename", Required = false, HelpText = "Original filename of the file you are writing the version for. Default: null")]
        public string? OriginalFilename { get; set; } = null;

        [CommandLine.Option("forceVersionUpdate", Required = false, HelpText = "Forces the execution of 'nbgv' even if valid environment variables are available. Default: false")]
        public bool ForceNbgvVersionUpdate { get; set; } = false;
        
        [CommandLine.Option('n', "nbgv", Required = false, HelpText = "Use NBGV to process version")]
        public bool UseNBGV { get; set; } = false;

        [CommandLine.Option("forceVersion", Required = false, HelpText = "Force manual version (needs to be in 'a.b.c.d' format). Default: null")]
        public string? ForceVersion { get; set; } = null;

        [CommandLine.Option("verbose", Required = false, HelpText = "Verbose console messages. Default: false")]
        public bool Verbose { get; set; } = false;

        [CommandLine.Option('e', "encoding", Required = false, HelpText = "Encoding codepage. Default: 65001 (UTF8).")]
        public int Encoding { get; set; } = 65001;

        [CommandLine.Option("emergency", Required = false, HelpText = "If true, the version will be forced to v1.0.0.0-emergency. Default: false")]
        public bool EmergencyVersion { get; set; } = false;
    }
}