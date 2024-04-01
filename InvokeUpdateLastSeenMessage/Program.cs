using System.Collections.Concurrent;

var tasks = new ConcurrentBag<Task<HttpResponseMessage>>();
Parallel.For(1, 200, _ =>
{
    var httpClient = new HttpClient();
    var task = httpClient.GetAsync(
            "https://localhost:5001/chat/channels/6bb78018-1af0-415b-a685-851dfc07e750/messages/fdc04abc-d513-4d93-bf55-8ac56de5795e/lastseen"
        );
    tasks.Add(task);
});

await Task.WhenAll(tasks);
foreach (var task in tasks)
{
    var response = await task;
    Console.WriteLine(response.IsSuccessStatusCode
        ? "OK Response"
        : $"Bad response with code {response.StatusCode}"
    );
}