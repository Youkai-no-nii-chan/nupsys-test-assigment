using CommandLine;
using Model;

namespace QueryDatabase;

[Verb("consolidate-minutes", HelpText = "Compute average value per each minute for specific period and export it to CSV")]
public class ConsolidateMinutesCommand : ExportCommand
{
    protected override async Task WriteData(StreamWriter file, DataRepository dataRepository)
    {
        using var cursor = await dataRepository.ConsolidateMinutes(StartDateTimeUtc, EndDateTimeUtc);
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