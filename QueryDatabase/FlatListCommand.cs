using CommandLine;
using Model;

namespace QueryDatabase;

[Verb("flat-list", HelpText = "Exports all data for specific period to CSV")]
public class FlatListCommand : ExportCommand
{
    protected override async Task WriteData(StreamWriter file, DataRepository dataRepository)
    {
        using var cursor = await dataRepository.GetFlatList(StartDateTimeUtc, EndDateTimeUtc);
        await file.WriteLineAsync("Timestamp;Value");
        while (await cursor.MoveNextAsync())
        {
            foreach (var data in cursor.Current)
            {
                await file.WriteLineAsync($"{data.Timestamp};{data.Value}");  
            }  
        }
    }
}