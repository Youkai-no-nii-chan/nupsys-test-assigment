using CommandLine;
using Model;

namespace QueryDatabase;

public abstract class ExportCommand
{
    [Option('f', "file-path", Required = true, HelpText = "Resulting file path.")]
    public string FilePath { get; set; } = null!;
    
    [Option('s', "start", Required = false, HelpText = "Period start in UTC.")]
    public DateTime? StartDateTimeUtc { get; set; }
    
    [Option('e', "end", Required = false, HelpText = "Period end in UTC.")]
    public DateTime? EndDateTimeUtc { get; set; }

    public async Task Run()
    {
        if (File.Exists(FilePath))
        {
            File.Delete(FilePath);
        }
        await using var file = File.CreateText(FilePath);

        await WriteData(file, new DataRepository());
    }

    protected abstract Task WriteData(StreamWriter file, DataRepository dataRepository);
}