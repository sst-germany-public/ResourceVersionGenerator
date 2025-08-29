namespace ResourceVersionGenerator.Services.ConsoleWriter
{
    public class Service : IService
    {
        private bool _isVerbose;
        private ConsoleColor _foreColor = Console.ForegroundColor;
        private ConsoleColor _backColor = Console.BackgroundColor;


        public Service()
        {
            _isVerbose = Program.Options is null ? false : Program.Options.Verbose;
        }


        public void Info(string message)
        {
            Console.WriteLine(message);
        }
        public void Warn(string message)
        {
            Console.WriteLine(message);
        }
        public void Error(string message)
        {
            Console.WriteLine(message);
        }
        public void Verbose(string message)
        {
            Console.WriteLine(message);
        }
    }
}
