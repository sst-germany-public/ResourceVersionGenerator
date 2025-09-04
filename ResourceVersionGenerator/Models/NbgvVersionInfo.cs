using System.Diagnostics;
using System.Text.Json;

namespace ResourceVersionGenerator.Models
{
    internal class NbgvVersionInfo
    {
        public bool VersionFileFound { get; set; }
        public bool PublicRelease { get; set; }
        public string? PrereleaseVersion { get; set; }

        public Version? Version { get; set; }
        public Version? AssemblyVersion { get; set; }
        public Version? AssemblyFileVersion { get; set; }
        public string? AssemblyInformationalVersion { get; set; }
        public string? NuGetPackageVersion { get; set; }
        public string? GitCommitId { get; set; }
        public string? GitCommitIdShort { get; set; }
        //public int BuildNumber { get; set; }
        //public string? PrereleaseVersionNoLeadingHyphen { get; set; }
        //public Version? SimpleVersion { get; set; }
        //public int VersionRevision { get; set; }
        //public int VersionMajor { get; set; }
        //public int VersionMinor { get; set; }
        //public string? SemVer1 { get; set; }
        //public string? SemVer2 { get; set; }
        //public string? CloudBuildNumber { get; set; }

        internal static async Task<NbgvVersionInfo?> CreateFromNBGVAsync(Services.ConsoleWriter.IService log)
        {
            // Erstellt die Startinformationen für den Prozess
            var startInfo = new ProcessStartInfo
            {
                FileName = "nbgv", // Der Name des auszuführenden Programms
                Arguments = "get-version -f json", // Die Argumente für das Programm
                RedirectStandardOutput = true, // Leitet die Standardausgabe um, um sie in C# lesen zu können
                UseShellExecute = false, // Notwendig, um die Standardausgabe umzuleiten
                CreateNoWindow = true // Erstellt kein separates Konsolenfenster
            };

            try
            {
                // Startet den Prozess
                using (var process = Process.Start(startInfo))
                {
                    if (process == null)
                    {
                        log.Error("Fehler: Der NBGV-Prozess konnte nicht gestartet werden.");
                        return null;
                    }

                    // Wartet asynchron, bis der Prozess beendet ist
                    // und liest die gesamte Ausgabe als String
                    var jsonOutput = await process.StandardOutput.ReadToEndAsync();

                    // Wartet auf den Abschluss des Prozesses und gibt den Exit-Code aus
                    await process.WaitForExitAsync();

                    // Überprüft, ob der Befehl erfolgreich war (ExitCode 0)
                    if (process.ExitCode != 0)
                    {
                        log.Error($"Fehler beim Ausführen von NBGV. Exit-Code: {process.ExitCode}");
                        // Option: Lese auch die Standardfehler-Ausgabe (Stderr) für detailliertere Fehler
                        return null;
                    }

                    log.Verbose(jsonOutput);

                    // Deserialisiert den JSON-String direkt in ein Objekt und gibt es zurück
                    return JsonSerializer.Deserialize<NbgvVersionInfo>(jsonOutput);
                }
            }
            catch (FileNotFoundException)
            {
                log.Error("Fehler: 'nbgv' wurde nicht gefunden. Stellen Sie sicher, dass es im PATH liegt oder Sie den vollen Pfad verwenden.");
                return null;
            }
            catch (JsonException ex)
            {
                log.Error($"Fehler beim Deserialisieren des JSON-Strings: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                log.Error($"Ein unerwarteter Fehler ist aufgetreten (CreateFromNBGVAsync): {ex.Message}");
                return null;
            }
        }
        internal static Task<NbgvVersionInfo?> CreateFromEnvironmentAsync(Services.ConsoleWriter.IService log)
        {
            try
            {
                if (!doExists(nameof(NbgvVersionInfo.AssemblyVersion)))
                {
                    log.Verbose("No 'NBGV_' environment variables found.");
                    return Task.FromResult<NbgvVersionInfo?>(null);
                }

                var instance = new NbgvVersionInfo();
                instance.VersionFileFound = tryGetBoolean(nameof(NbgvVersionInfo.VersionFileFound));
                instance.PublicRelease = tryGetBoolean(nameof(NbgvVersionInfo.PublicRelease));
                instance.PrereleaseVersion = tryGetString(nameof(NbgvVersionInfo.PrereleaseVersion));
                instance.Version = tryGetVersion(nameof(NbgvVersionInfo.Version));
                instance.AssemblyVersion = tryGetVersion(nameof(NbgvVersionInfo.AssemblyVersion));
                instance.AssemblyFileVersion = tryGetVersion(nameof(NbgvVersionInfo.VersionFileFound));
                instance.AssemblyInformationalVersion = tryGetString(nameof(NbgvVersionInfo.AssemblyInformationalVersion));
                instance.NuGetPackageVersion = tryGetString(nameof(NbgvVersionInfo.NuGetPackageVersion));
                instance.GitCommitId = tryGetString(nameof(NbgvVersionInfo.GitCommitId));
                instance.GitCommitIdShort = tryGetString(nameof(NbgvVersionInfo.GitCommitIdShort));
                return Task.FromResult<NbgvVersionInfo?>(instance);

                bool doExists(string name)
                {
                    log.Verbose($"Checking environment variable 'NBGV_{name}'... (exists?)");
                    var value = Environment.GetEnvironmentVariable($"NBGV_{name}");
                    return string.IsNullOrWhiteSpace(value) == false;
                }
                Version tryGetVersion(string name)
                {
                    log.Verbose($"Checking environment variable 'NBGV_{name}'... (version)");
                    var value = Environment.GetEnvironmentVariable($"NBGV_{name}");
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        throw new InvalidOperationException($"Die Umgebungsvariable 'NBGV_{name}' ist nicht gesetzt oder leer.");
                    }
                    if (!Version.TryParse(value, out var version))
                    {
                        throw new InvalidOperationException($"Die Umgebungsvariable 'NBGV_{name}' enthält keinen gültigen Versionswert: '{value}'.");
                    }
                    return version;
                }
                string tryGetString(string name)
                {
                    log.Verbose($"Checking environment variable 'NBGV_{name}'... (as string)");
                    var value = Environment.GetEnvironmentVariable($"NBGV_{name}");
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        throw new InvalidOperationException($"Die Umgebungsvariable 'NBGV_{name}' ist nicht gesetzt oder leer.");
                    }
                    return value;
                }
                bool tryGetBoolean(string name)
                {
                    log.Verbose($"Checking environment variable 'NBGV_{name}'... (as bool)");
                    var value = Environment.GetEnvironmentVariable($"NBGV_{name}");
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        throw new InvalidOperationException($"Die Umgebungsvariable 'NBGV_{name}' ist nicht gesetzt oder leer.");
                    }
                    if (value.Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                    else if (value.Equals("false", StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }
                    else
                    {
                        throw new InvalidOperationException($"Die Umgebungsvariable 'NBGV_{name}' enthält einen ungültigen Wert: '{value}'.");
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"Ein unerwarteter Fehler ist aufgetreten (CreateFromEnvironmentAsync): {ex.Message}");
                return Task.FromResult<NbgvVersionInfo?>(null);
            }
        }
    }
}
