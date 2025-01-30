using Aero.Base;

namespace Aero.Worker.WebApi.Models;

public record WorkerLaunchResponse(int WorkerId, WorkerStatus WorkerStatus);