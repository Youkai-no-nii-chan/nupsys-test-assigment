using CommandLine;
using Model;

namespace QueryDatabase;

[Verb("peaks-per-hour", HelpText = "Compute max value per each hour for specific period and export it to CSV")]
public class PeaksPerHourCommand : ExportCommand
{
    protected override async Task WriteData(StreamWriter file, DataRepository dataRepository)
    {
        using var cursor = await dataRepository.PeaksPerHour(StartDateTimeUtc, EndDateTimeUtc);
        await file.WriteLineAsync("Timestamp;MaxValue");
        while (await cursor.MoveNextAsync())
        {
            foreach (var data in cursor.Current)
            {
                await file.WriteLineAsync($"{data.Timestamp};{data.Value}");  
            }  
        }
    }
}