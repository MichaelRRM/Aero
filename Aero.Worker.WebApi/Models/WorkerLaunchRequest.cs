namespace Aero.Worker.WebApi.Models;

public record WorkerLaunchRequest(string WorkerName, string Module, List<string>? Arguments);