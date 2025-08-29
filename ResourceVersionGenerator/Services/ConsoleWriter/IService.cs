namespace ResourceVersionGenerator.Services.ConsoleWriter
{
    public interface IService
    {
        void Error(string message);
        void Info(string message);
        void Verbose(string message);
        void Warn(string message);
    }
}
