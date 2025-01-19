namespace Aero.Worker.WebApi.Models;

public record WorkerDto(
    string Name, 
    string Description, 
    IEnumerable<ModuleDto> Modules
    );