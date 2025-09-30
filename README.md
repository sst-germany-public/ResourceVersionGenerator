# Resource Version Generator

A small .NET tool that generates a `resourceVersion.h` header file for your C++ projects. It is ideal for automatically integrating version information into your binaries.

The tool can obtain version information from two main sources:

1.  **Automatic:** For projects managed with [Nerdbank.GitVersioning](https://github.com/dotnet/Nerdbank.GitVersioning).
2.  **Manual:** By directly specifying a version number via the command line.

-----

### Installation

The tool is provided as a .NET tool via NuGet. You can install it globally or locally in your project.

**Global Installation:**
```bash
dotnet tool install --global ResourceVersionGenerator
```

**Local Installation:**
```bash
# In Ihrem Projektverzeichnis

dotnet new tool-manifest

dotnet tool install ResourceVersionGenerator
```
After installation, you can run the tool using the **`ResourceVersionGenerator`** command.

-----

### Usage

Simply run the command in the root directory of your C++ project.

**Example:**
```bash
ResourceVersionGenerator --company "My Awesome Company" --product "My Product Name" --output "./resourceVersion.h"
```
This generates a file named `resourceVersion.h` in the current directory with the version information from your environment variables.


**Example:**
```bash
ResourceVersionGenerator --company "My Awesome Company" --product "My.dll" --originalFilename "My.dll" --description "Do good stuff" --nbgv
```
-----

### Command Line Options

| Short Option | Long Option | Description |
| :--- | :--- | :--- |
| `-c`| `--company` | **Company name**. Used for the copyright text. Required.|
| `-o` | `--output` | **Output file**. Default: `./resourceVersion.h` |
| `-p` | `--product` | **Product name**. |
| `-d` | `--description` | **Product description**. |
| | `--originalFilename` | **Original file name** of the binary. |
| `-n` | `--nbgv` | Uses `nbgv` directly to determine the version information. |
| | `--forceVersionUpdate` | Forces execution of `nbgv` even if valid environment variables are available. Only active when using `nbgv`.|
| | `--forceVersion` | Manual **version number** (`Major.Minor.Patch.Build`). Overrides automatic versions. |
| | `--verbose` | Outputs **detailed console messages** to track the process. |
| | `--emergencyVersion` | Forces the version `1.0.0.0-emergency`, regardless of other settings. Default is `false`.|
| `-e` | `--encoding` | Defines the encoding for the generated file. The value is a number corresponding to the encoder `CodePage`. Default is 65001 (UTF8).|
| `-s` | `--silent` | Suppresses all output except in case of an error. |

-----

### Integration into Your Build Process

To integrate this tool into your C++ build process (e.g., in a `.vcxproj` or a CMake script), simply run the command before the compilation step. This ensures that the `resourceVersion.h` file is always up to date.

**Example for CMake integration:**
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

### License

This project is licensed under the **MIT License**. For more details, see the `LICENSE.md` file.
