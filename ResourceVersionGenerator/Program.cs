using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace ResourceVersionGenerator
{
    internal static class Program
    {
        public static IServiceProvider ServiceProvider
        {
            get
            {
                Debug.Assert(_serviceProvider is not null, "No access allowed, before method 'InitializeServices' was called.");
                return _serviceProvider;
            }
        }
        private static ServiceProvider? _serviceProvider = null;

        public static Options Options
        {
            get
            {
                Debug.Assert(s_options is not null, "No access allowed, before property was initialzed.");
                return s_options;
            }
            private set => s_options = value;
        }
        private static Options? s_options = null;


        private static async Task Main(string[] args)
        {
            var retValue = await Parser.Default.ParseArguments<Options>(args)
                .MapResult(parsedFunc, notParsedFunc);

            static async Task<int> parsedFunc(Options options)
            {
                Program.Options = options;

                if (InitializeServices())
                {
                    try
                    {
                        using (var c = new ConsoleApplication())
                        {
                            var exitCode = await c.RunAsync();
                            return (int)exitCode;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        return (int)ExitCodes.Unknown;
                    }
                }
                else
                {
                    return (int)ExitCodes.ServiceInitializationError;
                }
            }
            static Task<int> notParsedFunc(IEnumerable<Error> errors)
            {
                return Task.FromResult(Environment.ExitCode);
            }
        }


        private static bool InitializeServices()
        {
            if (_serviceProvider != null)
            {
                return false;
            }

            try
            {
                var services = new ServiceCollection();
                OnConfigureServices(services);
                _serviceProvider = services.BuildServiceProvider();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
        private static void OnConfigureServices(IServiceCollection services)
        {
            // Options
            services.AddSingleton<Services.ConsoleWriter.IService, Services.ConsoleWriter.Service>();
        }
    }
}