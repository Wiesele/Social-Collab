using System.Diagnostics;
using System.IO.Compression;
using Microsoft.Extensions.Configuration;
using MRO.SMK.SDK.Models;

namespace MRO.SKM.CSharpProvider.Documentation;

public class DocFxService
{
    private IConfiguration Configuration { get; set; }
    private HttpClient HttpClient { get; set; }
    private string TempPath { get; set; }
    private string DocFxFoder = ".docFx";

    private string DocFxFullPath
    {
        get { return Path.Combine(TempPath, DocFxFoder); }
    }

    private string DocFxExeName
    {
        get { return Path.Combine(DocFxFullPath, "docfx.exe"); }
    }

    public DocFxService(IConfiguration configuration, HttpClient httpClient)
    {
        this.HttpClient = httpClient;
        this.Configuration = configuration;

        this.TempPath = this.Configuration["TempPath"];
    }

    private bool DocFxExists()
    {
        if (!Directory.Exists(this.DocFxFullPath))
        {
            Directory.CreateDirectory(this.DocFxFullPath);

            return false;
        }

        return true;
    }

    private async Task DownloadDocFx()
    {
        var fileName = "docfx.zip";
        var destinationPath = Path.Combine(this.DocFxFullPath, fileName);

        var response =
            await this.HttpClient.GetAsync(
                "https://github.com/dotnet/docfx/releases/download/v2.78.4/docfx-win-x64-v2.78.4.zip");
        await using (var fs = new FileStream(
                         destinationPath,
                         FileMode.CreateNew))
        {
            await response.Content.CopyToAsync(fs);
        }

        ZipFile.ExtractToDirectory(destinationPath, this.DocFxFullPath);

        File.Delete(destinationPath);
    }

    private async Task<string> EnsureDocFxConfig(string repoPath)
    {
        var docfxConfigPath = Path.Combine(repoPath, "docfx.json");
        if (!File.Exists(docfxConfigPath))
        {
            var minimalConfig = """
                                {
                                  "metadata": [
                                    {
                                      "src": [
                                        {
                                          "files": [ "**/*.csproj" ],
                                          "exclude": [ "**/bin/**", "**/obj/**" ]
                                        }
                                      ],
                                      "dest": "api"
                                    }
                                  ],
                                  "build": {
                                    "content": [
                                      { "files": [ "api/**.yml" ] },
                                      { "files": [ "articles/**.md" ] }
                                    ],
                                    "resource": [
                                      { "files": [ "images/**" ] }
                                    ],
                                    "dest": "_site",
                                    "globalMetadata": {
                                      "enableSearch": true
                                    }
                                  }
                                }
                                """;

            await File.WriteAllTextAsync(docfxConfigPath, minimalConfig);
        }

        return docfxConfigPath;
    }

    private static async Task<int> RunProcessAsync(
        string fileName,
        string arguments,
        string workingDirectory,
        CancellationToken cancellationToken)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = arguments,
            WorkingDirectory = workingDirectory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = new Process { StartInfo = startInfo, EnableRaisingEvents = true };

        var tcs = new TaskCompletionSource<int>();

        process.OutputDataReceived += (_, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                // Hier könntest du logging integrieren
                Console.WriteLine("[docfx] " + e.Data);
            }
        };

        process.ErrorDataReceived += (_, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                Console.Error.WriteLine("[docfx:ERR] " + e.Data);
            }
        };

        process.Exited += (_, _) =>
        {
            tcs.TrySetResult(process.ExitCode);
        };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        using (cancellationToken.Register(() =>
               {
                   try { if (!process.HasExited) process.Kill(); }
                   catch { /* ignore */ }
               }))
        {
            return await tcs.Task;
        }
    }

    public async Task<byte[]> GenerateHtmlDocumentation(Repository repository)
    {
        if (!this.DocFxExists())
        {
            await this.DownloadDocFx();
        }

        var slnPath = System.IO.Directory.GetFiles(repository.Location, "*.sln", SearchOption.AllDirectories).First();
        var slnFolder = System.IO.Path.GetDirectoryName(slnPath);
        
        var configPath = await this.EnsureDocFxConfig(slnFolder);

        var buildExitCode = await RunProcessAsync(
            fileName: this.DocFxExeName,
            arguments: $"build \"{configPath}\"",
            workingDirectory: slnFolder,
            cancellationToken: CancellationToken.None);

        if (buildExitCode != 0)
        {
            throw new InvalidOperationException($"DocFX build ist fehlgeschlagen. ExitCode={buildExitCode}.");
        }

        var siteDir = Path.Combine(repository.Location, "_site");
        // if (!Directory.Exists(siteDir))
        //     throw new DirectoryNotFoundException($"DocFX-Output-Verzeichnis nicht gefunden: {siteDir}");

        
        return null;
    }
}