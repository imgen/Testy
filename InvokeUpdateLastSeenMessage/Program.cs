using System.Collections.Concurrent;

var tasks = new ConcurrentBag<Task<HttpResponseMessage>>();
Parallel.For(1, 200, _ =>
{
    var httpClient = new HttpClient();
    var task = httpClient.GetAsync(
            "https://localhost:5001/chat/channels/b8f42bfd-7f50-4ed1-bdd0-50f734b986af/messages/8ceb787e-ab5a-42ad-836b-34c3a5ac3c64/lastseen"
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