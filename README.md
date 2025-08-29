# Resource Version Generator

Ein kleines .NET-Tool, das eine `resourceVersion.h`-Headerdatei für Ihre C++-Projekte generiert. Es ist ideal, um Versionsinformationen automatisch in Ihre Binärdateien zu integrieren.

Das Tool kann Versionsinformationen aus zwei Hauptquellen beziehen:

1.  **Automatisch:** Bei Projekten welche mit [Nerdbank.GitVersioning](https://github.com/dotnet/Nerdbank.GitVersioning) verwaltet werden.
2.  **Manuell:** Durch direkte Angabe einer Versionsnummer über die Kommandozeile.

-----

### Installation

Das Tool wird als .NET-Tool über NuGet bereitgestellt. Sie können es global oder lokal in Ihrem Projekt installieren.

**Globale Installation:**

```bash
dotnet tool install --global ResourceVersionGenerator
```

**Lokale Installation:**

```bash
# In Ihrem Projektverzeichnis
dotnet new tool-manifest
dotnet tool install ResourceVersionGenerator
```

Nach der Installation können Sie das Tool über den Befehl **`ResourceVersionGenerator`** aufrufen.

-----

### Verwendung

Führen Sie den Befehl einfach im Stammverzeichnis Ihres C++-Projekts aus. 

**Beispiel:**

```bash
ResourceVersionGenerator --company "My Awesome Company" --product "My Product Name" --output "./resourceVersion.h"
```

Dies generiert eine Datei namens `resourceVersion.h` im aktuellen Verzeichnis mit den Versionsinformationen aus Ihren Umgebungsvariablen.

-----

### Kommandozeilenoptionen

| Kurze Option | Lange Option | Beschreibung |
| :--- | :--- | :--- |
| | `--company` | **Firmenname**. Wird für den Copyright-Text verwendet. Wird zwingend benötigt.|
| `-o` | `--output` | **Ausgabedatei**. Standard: `./resourceVersion.h` |
| `-p` | `--product` | **Produktname**. |
| `-d` | `--description` | **Produktbeschreibung**. |
| `-f` | `--originalFilename` | **Original-Dateiname** der Binärdatei. |
| `-n` | `--nbgv` | Verwendet `nbgv` direkt, um die Versionsinformationen zu ermitteln. |
| | `--forceGet` | Erzwingt die Ausführung von `nbgv`, auch wenn gültige Umgebungsvariablen verfügbar sind. Ist nur aktiv, wenn `nbgv` verwendet wird.|
| | `--version` | Manuelle **Versionsnummer** (`Major.Minor.Patch.Build`). Überschreibt automatische Versionen. |
| `-v` | `--verbose` | Gibt **ausführliche Konsolenmeldungen** aus, um den Prozess zu verfolgen. |

-----

### Integration in Ihren Build-Prozess

Um dieses Tool in Ihren C++-Build-Prozess (z.B. in eine `.vcxproj` oder ein CMake-Skript) zu integrieren, führen Sie einfach den Befehl vor dem Kompilierungsschritt aus. Das stellt sicher, dass die `resourceVersion.h`-Datei immer auf dem neuesten Stand ist.

**Beispiel für eine CMake-Integration:**

```cmake
# Führen Sie das Tool vor dem eigentlichen Build aus
execute_process(
    COMMAND ResourceVersionGenerator --company "My Awesome Company" --product "MyCoolApp"
    WORKING_DIRECTORY "${CMAKE_CURRENT_SOURCE_DIR}"
)

# Fügen Sie dann die generierte Datei Ihrem Projekt hinzu
add_executable(MyCoolApp main.cpp resourceVersion.h)
```

-----

### Lizenz

Dieses Projekt ist unter der **MIT-Lizenz** lizenziert. Weitere Details finden Sie in der `LICENSE.md`-Datei.
