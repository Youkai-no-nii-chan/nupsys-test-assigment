using CommandLine;
using Model;

namespace QueryDatabase;

[Verb("consolidate-hours", HelpText = "Compute average value per each hour for specific period and export it to CSV")]
public class ConsolidateHoursCommand : ExportCommand
{
    protected override async Task WriteData(StreamWriter file, DataRepository dataRepository)
    {
        using var cursor = await dataRepository.ConsolidateHours(StartDateTimeUtc, EndDateTimeUtc);
        await file.WriteLineAsync("Timestamp;AverageValue");
        while (await cursor.MoveNextAsync())
        {
            foreach (var data in cursor.Current)
            {
                await file.WriteLineAsync($"{data.Timestamp};{data.Value}");  
            }  
        }
    }
}