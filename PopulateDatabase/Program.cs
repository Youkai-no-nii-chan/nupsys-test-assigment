using Model;

var cancellationTokenSource = new CancellationTokenSource();

var populateDatabaseTask = PopulateDatabase(cancellationTokenSource.Token);

Console.WriteLine("Populating database with random values...");
Console.WriteLine("Press any key to exit");
Console.Read();
cancellationTokenSource.Cancel();
Console.WriteLine("Stopping...");
populateDatabaseTask.Wait();
Console.WriteLine("Stopped");

async Task PopulateDatabase(CancellationToken cancellationToken)
{
    var random = new Random();
    var dataRepository = new DataRepository();

    while (!cancellationToken.IsCancellationRequested)
    {
        var iterationStartDateTimeUtc = DateTime.UtcNow;
        
        await dataRepository.InsertAsync(new Data
        {
            Timestamp = DateTime.UtcNow,
            Value = random.NextDouble()
        });

        await Task.Delay(iterationStartDateTimeUtc.AddSeconds(10) - DateTime.UtcNow);
    }
}