namespace Aero.Worker.WebApi.Models;

public record ModuleDto(
    string Code,
    string Name, 
    string Description, 
    IEnumerable<ArgumentDto> Arguments
    );