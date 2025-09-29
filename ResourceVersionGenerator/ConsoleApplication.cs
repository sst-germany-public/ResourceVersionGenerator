using Microsoft.Extensions.DependencyInjection;
using ResourceVersionGenerator.Models;
using System.Text;

namespace ResourceVersionGenerator
{
    internal class ConsoleApplication : IDisposable
    {
        private Services.ConsoleWriter.IService _log;

        public ConsoleApplication()
        {
            _log = Program.ServiceProvider.GetRequiredService<Services.ConsoleWriter.IService>();
        }
        #region IDisposable (Full)
        public event Action? Disposed;
        /// <summary>
        /// Gets a value indicating whether this <see cref="Disposable"/> is disposed.
        /// </summary>
        /// <value><c>true</c> if disposed; otherwise, <c>false</c>.</value>
        public bool IsDisposed => _isDisposed;
        private bool _isDisposed;
        /// <summary>
        /// Finalizes an instance of the <see cref="Disposable"/> class. Releases unmanaged
        /// resources and performs other cleanup operations before the <see cref="Disposable"/>
        /// is reclaimed by garbage collection. Will run only if the
        /// Dispose method does not get called.
        /// </summary>
        ~ConsoleApplication()
        {
            Console.WriteLine("Hey Dumpfnase! You missed a Dispose() call!");
            Dispose(false);
        }
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources;
        /// <c>false</c> to release only unmanaged resources, called from the finalizer only.</param>
        private void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!_isDisposed)
            {
                // If disposing managed and unmanaged resources.
                if (disposing)
                {
                }

                _isDisposed = true;

                Disposed?.Invoke();
            }
        }
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // Dispose all managed and unmanaged resources.
            Dispose(true);

            // Take this object off the finalization queue and prevent finalization code for this
            // object from executing a second time.
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Throws a <see cref="ObjectDisposedException"/> if this instance is disposed.
        /// </summary>
        protected void ThrowIfDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }
        #endregion

        public async Task<ExitCodes> RunAsync()
        {
            // We start with an emergency type version, so we always have something to write.
            var version = new Version(0, 0, 0, 0);
            var versionText = "0.0.0.0-emergency";

            // Use NBGV to get version information, if requested...
            if (Program.Options.UseNBGV)
            {
                NbgvVersionInfo? nbgvVersionInfo = null;
                if (!Program.Options.ForceGet)
                {
                    nbgvVersionInfo = await NbgvVersionInfo.CreateFromEnvironmentAsync(_log);
                }
                if (nbgvVersionInfo is null)
                {
                    nbgvVersionInfo = await NbgvVersionInfo.CreateFromNBGVAsync(_log);
                }
                else
                {
                    _log.Verbose($"NBGV version information readed from environment.");
                }

                if ((nbgvVersionInfo is not null) && (nbgvVersionInfo.AssemblyFileVersion is not null))
                {
                    version = nbgvVersionInfo.AssemblyFileVersion;
                    versionText = nbgvVersionInfo.NuGetPackageVersion;
                    _log.Verbose($"NBGV version information successfully determined: version({version}), versionText({versionText})");
                }
                else
                {
                    _log.Verbose("NBGV version information could not be determined.");
                }
            }

            // If a commandline version is specified, it has the highest priority.
            if (!string.IsNullOrWhiteSpace(Program.Options.Version))
            {
                if (!Version.TryParse(Program.Options.Version, out version))
                {
                    version = new Version(0, 0, 0, 0);
                    _log.Error($"The specified version '{Program.Options.Version}' is not valid.");
                }
                versionText = Program.Options.Version;
            }

            if (!WriteOutputFile(version, versionText))
            {
                return ExitCodes.WriteOutputfile;
            }

            return ExitCodes.OK;
        }

        private bool WriteOutputFile(Version version, string? versionText)
        {
            var outputFilename = Program.Options.OutputFilename;
            if (string.IsNullOrWhiteSpace(outputFilename))
            {
                _log.Error("The parameter 'output' must not be null or empty.");
                return false;
            }

            try
            {
                _log.Info($"ResourceVersionGenerator: V{versionText} - '{outputFilename}'");

                var company = Program.Options.Company;
                if (string.IsNullOrWhiteSpace(company))
                {
                    _log.Error("Company parameter must not be null or empty.");
                    return false;
                }


                // Build Text for file:
                var lines = new List<string>();
                lines.Add($"// Copyright© {DateTime.Now.ToString("yyyy")} {company}. All rights reserved.");
                lines.Add("// ");
                lines.Add("// This code was generated by the 'ResourceVersionGenerator' tool (https://www.nuget.org/packages/ResourceVersionGenerator).");
                lines.Add("// Any changes made manually will be overwritten next time!");
                lines.Add("");
                lines.Add($"#define VER_FILE_VERSION\t\t\t{version.Major},{version.Minor},{version.Build},{version.Revision}"); // 1,0,0,0
                lines.Add($"#define VER_PRODUCT_VERSION\t\t\t{version.Major},{version.Minor},{version.Build},{version.Revision}"); // 1,0,0,0
                if (!string.IsNullOrWhiteSpace(versionText))
                {
                    lines.Add($"#define VER_FILE_VERSION_STR\t\t\"{versionText}\""); // 2.0.0-dev15.0+build(2233-54332)
                    lines.Add($"#define VER_PRODUCT_VERSION_STR\t\t\"{versionText}\""); // // 2.0.0-dev15.0+build(2233-54332)
                }
                lines.Add($"#define VER_COPYRIGHT_STR\t\t\t\"{company} ©{DateTime.Now.ToString("yyyy")}\"");

                if (!string.IsNullOrWhiteSpace(Program.Options.OriginalFilename))
                {
                    lines.Add($"#define VER_ORIGINAL_FILENAME_STR\t\"{Program.Options.OriginalFilename}\"");
                }

                if (!string.IsNullOrWhiteSpace(Program.Options.Product))
                {
                    lines.Add($"#define VER_INTERNAL_NAME_STR\t\t\"{Program.Options.Product}\"");
                    lines.Add($"#define VER_PRODUCTNAME_STR\t\t\t\"{Program.Options.Product}\"");
                }

                if (!string.IsNullOrWhiteSpace(Program.Options.Description))
                {
                    lines.Add($"#define VER_FILE_DESCRIPTION_STR\t\"{Program.Options.Description}\"");
                }

                // --------------------------------------------------------------------------------------------
                bool writeLinesToFile(IReadOnlyCollection<string> lines)
                {
                    try
                    {
                        Encoding getEncoding()
                        {
                            try
                            {
                                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                                return Encoding.GetEncoding(Program.Options.Encoding);
                            }
                            catch
                            {
                                _log.Error($"Unable to create encoding ({Program.Options.Encoding}). Falling back to UTF8(65001).");
                                return Encoding.UTF8;
                            }
                        }

                        using (var outputFile = new StreamWriter(path: outputFilename, append: false, encoding: getEncoding()))
                        {
                            foreach (var line in lines)
                            {
                                outputFile.WriteLine(line);
                            }
                        }

                        return true;
                    }
                    catch
                    {
                        _log.Error($"Unable to create file: {Program.Options.OutputFilename}");
                        return false;
                    }
                }
                return writeLinesToFile(lines);
            }
            catch (Exception ex)
            {
                _log.Error($"An unexpected error occurred: {ex.Message}");
                return false;
            }
        }
    }
}
