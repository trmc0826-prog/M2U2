
using DataStructures;

// Lagrer de 50 siste loggmeldingene
var blackBox = new RingBuffer<LogEntry>(50);

for (int i = 1; i <= 200; i++)
{
    blackBox.Add(new LogEntry(DateTime.Now, "INFO", $"Behandlet ordre {i}"));

    // Simulerer en feil ved ordre 137
    if (i == 137)
    {
        WriteErrorReport(blackBox, "Databasen svarte ikke");
        break;
    }
}

// Hver sensor får sin egen ringbuffer
var windows = new Dictionary<string, RingBuffer<SensorReading>>();

string[] sensors = ["temp-kjeller", "temp-loft", "temp-ute"];
var random = new Random(42);

for (int i = 0; i < 100; i++)
{
    foreach (var sensorId in sensors)
    {
        var reading = new SensorReading(
            sensorId,
            15 + random.NextDouble() * 10,
            DateTime.Now
        );

        if (!windows.TryGetValue(sensorId, out var window))
        {
            window = new RingBuffer<SensorReading>(10);
            windows[sensorId] = window;
        }

        window.Add(reading);
    }
}

// Skriver ut gjennomsnittet for hver sensor
Console.WriteLine("\n--- SENSORER ---");

foreach (var (sensorId, window) in windows)
{
    Console.WriteLine(
        $"{sensorId}: {MovingAverage(window):F1} °C (siste {window.Count} målinger)"
    );
}

var testWindow = windows["temp-kjeller"];
Console.WriteLine($"{MovingAverage(testWindow)}");

for (int i = 0; i < 10; i++)
{
    testWindow.Add(new SensorReading("temp-kjeller", 30, DateTime.Now));
    Console.WriteLine($"{MovingAverage(testWindow)}");
}

Console.WriteLine($"{MovingAverage(testWindow)}");

// Lagrer de 100 siste API-kallene
var requests = new RingBuffer<RequestLog>(100);
var requestRandom = new Random(42);

// Flere endepunkter for mer realistisk simulering
string[] endpoints = ["/api/orders", "/api/users", "/api/products"];

// Simulerer 500 API-kall
for (int i = 0; i < 10_000; i++)
{
    int durationMs = requestRandom.Next(50, 901);
    string endpoint = endpoints[requestRandom.Next(3)];

    requests.Add(new RequestLog(endpoint, durationMs, 200));
}

Console.WriteLine($"API requests stored: {requests.Count}/{requests.Capacity}");

// Antall av de siste 100 kallene som tok over 500 ms
int slowCalls = requests.Count(r => r.DurationMs > 500);

// Lengste responstid
int slowest = requests.Max(r => r.DurationMs);

// Henter hele objektet med lengst responstid
var slowestRequest = requests.MaxBy(r => r.DurationMs);

Console.WriteLine("\n--- API RESPONSTIDER ---");
Console.WriteLine($"Trege API-kall (>500 ms): {slowCalls}");
Console.WriteLine($"Lengste responstid: {slowest} ms");
Console.WriteLine($"Tregeste endepunkt: {slowestRequest?.Endpoint}");

// METODER

// Skriver ut loggene før en feil oppstår
static void WriteErrorReport(RingBuffer<LogEntry> log, string message)
{
    Console.WriteLine($"FEIL: {message}");
    Console.WriteLine($"--- de {log.Count} siste hendelsene før feilen ---");

    foreach (var entry in log)
    {
        Console.WriteLine($"{entry.Time:HH:mm:ss} [{entry.Level}] {entry.Message}");
    }
}

// Beregner gjennomsnittstemperatur med LINQ
static double MovingAverage(RingBuffer<SensorReading> window)
{
    if (window.Count == 0)
        return 0;

    return window.Average(reading => reading.Value);
}
