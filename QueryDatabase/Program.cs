using CommandLine;
using QueryDatabase;

var command = Parser.Default.ParseArguments<
    FlatListCommand,
    ConsolidateHoursCommand,
    ConsolidateMinutesCommand,
    PeaksPerHourCommand>(args);

if (!command.Errors.Any())
{
    Console.WriteLine("Starting export...");
    await ((ExportCommand) command.Value).Run();
    Console.WriteLine("Export finished");
}