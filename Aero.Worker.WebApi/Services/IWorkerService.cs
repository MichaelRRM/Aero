using Aero.Worker.WebApi.Models;

namespace Aero.Worker.WebApi.Services;

public interface IWorkerService
{
    IEnumerable<WorkerDto> GetWorkers();
}