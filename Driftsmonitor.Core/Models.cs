public record LogEntry(DateTime Time, string Level, string Message);

public record SensorReading(string SensorId, double Value, DateTime Time);

public record RequestLog(string Endpoint, int DurationMs, int StatusCode);