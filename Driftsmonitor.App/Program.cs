List<string> window = [];
var watch = System.Diagnostics.Stopwatch.StartNew();

for (var i = 0; i < 200_000; i++)
{
    window.Add($"Behandlet ordre {i}");

    if (window.Count > 10_000)
        window.RemoveAt(0);
}

Console.WriteLine($"Antall: {window.Count}");
Console.WriteLine($"Tid: {watch.ElapsedMilliseconds} ms");